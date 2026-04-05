using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public enum PatrolAxis { Horizontal, Vertical }

    [SerializeField] private PatrolAxis axis = PatrolAxis.Horizontal;
    [SerializeField] private bool reverseStart = false;   // start moving left/down instead of right/up
    [SerializeField] private float patrolDistance = 3f;
    [SerializeField] private float speed = 2f;

    [Header("Turn Behaviour")]
    [SerializeField] private float waitBeforeTurn = 0.5f;
    [SerializeField] private float turnSpeed = 80f;       // degrees per second
    [SerializeField] private float waitAfterTurn = 0.3f;

    [Header("Line of Sight")]
    [SerializeField] private float viewAngle = 70f;
    [SerializeField] private float viewDistance = 5f;

    private enum State { Moving, WaitBeforeTurn, Turning, WaitAfterTurn, PlayerSpotted }
    private State state = State.Moving;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private int direction;        // 1 = right/up, -1 = left/down
    private float stateTimer;

    private float facingAngle;    // current angle in degrees
    private float targetAngle;    // angle we're rotating toward
    private Vector2 facingDir;    // unit vector from facingAngle

    private Mesh coneMesh;
    private MeshFilter coneMeshFilter;
    private Material coneMaterial;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
        direction = reverseStart ? -1 : 1;

        Vector2 initialDir = GetMoveDir();
        facingAngle = Mathf.Atan2(initialDir.y, initialDir.x) * Mathf.Rad2Deg;
        targetAngle = facingAngle;
        facingDir = initialDir;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        SetupConeMesh();
        BuildConeMesh();
    }

    // returns the direction vector the enemy is currently moving
    Vector2 GetMoveDir() => axis == PatrolAxis.Horizontal
        ? (direction == 1 ? Vector2.right : Vector2.left)
        : (direction == 1 ? Vector2.up    : Vector2.down);

    // returns the axis vector (no direction applied)
    Vector2 GetMoveAxis() => axis == PatrolAxis.Horizontal ? Vector2.right : Vector2.up;

    void SetupConeMesh()
    {
        GameObject coneObj = new GameObject("VisionCone");
        coneObj.transform.SetParent(transform);
        coneObj.transform.localPosition = Vector3.zero;
        coneObj.transform.localRotation = Quaternion.identity;
        coneObj.transform.localScale    = Vector3.one;

        coneMeshFilter = coneObj.AddComponent<MeshFilter>();
        MeshRenderer mr = coneObj.AddComponent<MeshRenderer>();

        coneMaterial = new Material(Shader.Find("Sprites/Default"));
        coneMaterial.color = new Color(1f, 1f, 1f, 0.3f);
        mr.material = coneMaterial;
        mr.sortingLayerName = "WalkInFront";
        mr.sortingOrder = 0;

        coneMesh = new Mesh();
        coneMeshFilter.mesh = coneMesh;
    }

    void BuildConeMesh()
    {
        int segments = 24;
        float halfAngle = viewAngle / 2f;

        Vector3[] verts = new Vector3[segments + 2];
        int[]     tris  = new int[segments * 3];

        verts[0] = Vector3.zero;
        for (int i = 0; i <= segments; i++)
        {
            float a = -halfAngle + (viewAngle / segments) * i;
            Vector2 dir = Quaternion.Euler(0, 0, a) * facingDir;
            verts[i + 1] = (Vector3)dir * viewDistance;
        }

        for (int i = 0; i < segments; i++)
        {
            tris[i * 3]     = 0;
            tris[i * 3 + 1] = i + 1;
            tris[i * 3 + 2] = i + 2;
        }

        coneMesh.Clear();
        coneMesh.vertices  = verts;
        coneMesh.triangles = tris;
        coneMesh.RecalculateNormals();
    }

    void Update()
    {
        // rotate facing toward target angle (slow during Turning state, instant otherwise)
        float turnSpd = state == State.Turning ? turnSpeed : 9999f;
        facingAngle = Mathf.MoveTowardsAngle(facingAngle, targetAngle, turnSpd * Time.deltaTime);
        float rad = facingAngle * Mathf.Deg2Rad;
        facingDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        if (state != State.PlayerSpotted && PlayerInCone())
        {
            state = State.PlayerSpotted;
            GameOverScreen.Show();
        }

        BuildConeMesh();
        coneMaterial.color = state == State.PlayerSpotted
            ? new Color(1f, 0.3f, 0.3f, 0.35f)   // red when player spotted
            : new Color(1f, 1f,   1f,   0.3f);    // white normally
    }

    bool PlayerInCone()
    {
        if (player == null) return false;
        Vector2 toPlayer = (Vector2)player.position - (Vector2)transform.position;
        return toPlayer.magnitude <= viewDistance && Vector2.Angle(facingDir, toPlayer) <= viewAngle / 2f;
    }

    void FixedUpdate()
    {
        Vector2 moveAxis = GetMoveAxis();

        switch (state)
        {
            case State.Moving:
                rb.linearVelocity = moveAxis * direction * speed;

                float offset = axis == PatrolAxis.Horizontal
                    ? rb.position.x - startPosition.x
                    : rb.position.y - startPosition.y;

                if ((direction == 1 && offset >= patrolDistance) ||
                    (direction == -1 && offset <= -patrolDistance))
                {
                    rb.linearVelocity = Vector2.zero;
                    state = State.WaitBeforeTurn;
                    stateTimer = waitBeforeTurn;
                }
                break;

            case State.WaitBeforeTurn:
                rb.linearVelocity = Vector2.zero;
                stateTimer -= Time.fixedDeltaTime;
                if (stateTimer <= 0f)
                {
                    direction = -direction;
                    Vector2 newDir = GetMoveDir();
                    targetAngle = Mathf.Atan2(newDir.y, newDir.x) * Mathf.Rad2Deg;
                    state = State.Turning;
                }
                break;

            case State.Turning:
                rb.linearVelocity = Vector2.zero;
                // wait until the cone finishes rotating
                if (Mathf.Abs(Mathf.DeltaAngle(facingAngle, targetAngle)) < 1f)
                {
                    state = State.WaitAfterTurn;
                    stateTimer = waitAfterTurn;
                }
                break;

            case State.WaitAfterTurn:
                rb.linearVelocity = Vector2.zero;
                stateTimer -= Time.fixedDeltaTime;
                if (stateTimer <= 0f)
                    state = State.Moving;
                break;

            case State.PlayerSpotted:
                rb.linearVelocity = Vector2.zero;
                break;
        }
    }

    // draws the vision cone outline in the scene view
    void OnDrawGizmos()
    {
        Vector2 origin = transform.position;
        Vector2 facing = Application.isPlaying ? facingDir
            : (axis == PatrolAxis.Horizontal
                ? (reverseStart ? Vector2.left  : Vector2.right)
                : (reverseStart ? Vector2.down  : Vector2.up));

        float halfAngle = viewAngle / 2f;
        Gizmos.color = new Color(1f, 1f, 1f, 0.6f);
        Gizmos.DrawLine(origin, origin + (Vector2)(Quaternion.Euler(0, 0,  halfAngle) * facing) * viewDistance);
        Gizmos.DrawLine(origin, origin + (Vector2)(Quaternion.Euler(0, 0, -halfAngle) * facing) * viewDistance);

        for (int i = 0; i < 20; i++)
        {
            float a1 = -halfAngle + (viewAngle / 20f) * i;
            float a2 = -halfAngle + (viewAngle / 20f) * (i + 1);
            Vector2 p1 = origin + (Vector2)(Quaternion.Euler(0, 0, a1) * facing) * viewDistance;
            Vector2 p2 = origin + (Vector2)(Quaternion.Euler(0, 0, a2) * facing) * viewDistance;
            Gizmos.DrawLine(p1, p2);
        }
    }

    // draws patrol range when the object is selected
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 origin = Application.isPlaying ? startPosition : (Vector2)transform.position;

        if (axis == PatrolAxis.Horizontal)
        {
            Gizmos.DrawLine(origin + Vector2.left * patrolDistance, origin + Vector2.right * patrolDistance);
            Gizmos.DrawWireSphere(origin + Vector2.left  * patrolDistance, 0.15f);
            Gizmos.DrawWireSphere(origin + Vector2.right * patrolDistance, 0.15f);
        }
        else
        {
            Gizmos.DrawLine(origin + Vector2.down * patrolDistance, origin + Vector2.up * patrolDistance);
            Gizmos.DrawWireSphere(origin + Vector2.down * patrolDistance, 0.15f);
            Gizmos.DrawWireSphere(origin + Vector2.up   * patrolDistance, 0.15f);
        }
    }
}