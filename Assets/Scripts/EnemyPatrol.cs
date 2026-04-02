using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public enum PatrolAxis { Horizontal, Vertical }
    public enum HorizontalStart { Right, Left }
    public enum VerticalStart { Up, Down }

    [SerializeField] private PatrolAxis axis = PatrolAxis.Horizontal;
    [SerializeField] private HorizontalStart horizontalStart = HorizontalStart.Right; //starting direction when horizontal
    [SerializeField] private VerticalStart verticalStart = VerticalStart.Up; //starting direction when vertical
    [SerializeField] private float patrolDistance = 3f; //how far it goes each direction
    [SerializeField] private float speed = 2f; //movement speed

    [Header("Turn Behaviour")]
    [SerializeField] private float waitBeforeTurn = 0.5f; //pause before rotating
    [SerializeField] private float slowTurnSpeed = 80f;   //degrees per second during deliberate turn
    [SerializeField] private float waitAfterTurn = 0.3f;  //pause after rotating before walking

    [Header("Line of Sight")]
    [SerializeField] private float viewAngle = 70f;   //total cone angle in degrees
    [SerializeField] private float viewDistance = 5f; //how far the cone reaches

    private enum PatrolState { Moving, WaitBeforeTurn, Turning, WaitAfterTurn, PlayerSpotted }
    private PatrolState state = PatrolState.Moving;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private int direction = 1;
    private float stateTimer = 0f;
    private Vector2 facingDir = Vector2.right; //smoothed facing direction used for cone

    private float currentFacingAngle;
    private float targetFacingAngle;

    private Mesh coneMesh;
    private MeshFilter coneMeshFilter;
    private Material coneMaterial;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
        direction = axis == PatrolAxis.Horizontal
            ? (horizontalStart == HorizontalStart.Right ? 1 : -1)
            : (verticalStart == VerticalStart.Up ? 1 : -1);

        Vector2 initialDir = axis == PatrolAxis.Horizontal
            ? (direction == 1 ? Vector2.right : Vector2.left)
            : (direction == 1 ? Vector2.up : Vector2.down);

        currentFacingAngle = Mathf.Atan2(initialDir.y, initialDir.x) * Mathf.Rad2Deg;
        targetFacingAngle  = currentFacingAngle;
        facingDir = initialDir;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        SetupConeMesh();
        BuildConeMesh();
    }

    void SetupConeMesh()
    {
        GameObject coneObj = new GameObject("VisionCone");
        coneObj.transform.SetParent(transform);
        coneObj.transform.localPosition = Vector3.zero;
        coneObj.transform.localRotation = Quaternion.identity;
        coneObj.transform.localScale = Vector3.one;

        coneMeshFilter = coneObj.AddComponent<MeshFilter>();
        MeshRenderer mr = coneObj.AddComponent<MeshRenderer>();

        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = new Color(1f, 1f, 1f, 0.3f); //light white hue
        mr.material = mat;
        coneMaterial = mat;

        mr.sortingLayerName = "WalkInFront";
        mr.sortingOrder = 0;

        coneMesh = new Mesh();
        coneMeshFilter.mesh = coneMesh;
    }

    void BuildConeMesh()
    {
        int segments = 24;
        float halfAngle = viewAngle / 2f;

        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float angle = -halfAngle + (viewAngle / segments) * i;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * facingDir;
            vertices[i + 1] = new Vector3(dir.x, dir.y, 0f) * viewDistance;
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3]     = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        coneMesh.Clear();
        coneMesh.vertices = vertices;
        coneMesh.triangles = triangles;
        coneMesh.RecalculateNormals();
    }

    void Update()
    {
        //rotate toward target at slow speed during turn
        float turnSpd = (state == PatrolState.Turning) ? slowTurnSpeed : 9999f;
        currentFacingAngle = Mathf.MoveTowardsAngle(currentFacingAngle, targetFacingAngle, turnSpd * Time.deltaTime);
        float rad = currentFacingAngle * Mathf.Deg2Rad;
        facingDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        if (state != PatrolState.PlayerSpotted && PlayerInCone())
        {
            state = PatrolState.PlayerSpotted;
            GameOverScreen.Show();
        }

        BuildConeMesh();
        UpdateConeColor();
    }

    bool PlayerInCone()
    {
        if (player == null) return false;
        Vector2 toPlayer = (Vector2)player.position - (Vector2)transform.position;
        if (toPlayer.magnitude > viewDistance) return false;
        return Vector2.Angle(facingDir, toPlayer) <= viewAngle / 2f;
    }

    void UpdateConeColor()
    {
        if (coneMaterial == null) return;
        coneMaterial.color = (state == PatrolState.PlayerSpotted)
            ? new Color(1f, 0.3f, 0.3f, 0.35f)  //light red — stays red once spotted
            : new Color(1f, 1f, 1f, 0.3f);       //light white
    }

    void FixedUpdate()
    {
        Vector2 moveAxis = axis == PatrolAxis.Horizontal ? Vector2.right : Vector2.up;

        switch (state)
        {
            case PatrolState.Moving:
                rb.linearVelocity = moveAxis * direction * this.speed;

                float offset = axis == PatrolAxis.Horizontal
                    ? rb.position.x - startPosition.x
                    : rb.position.y - startPosition.y;

                if ((offset >= patrolDistance && direction == 1) ||
                    (offset <= -patrolDistance && direction == -1))
                {
                    rb.linearVelocity = Vector2.zero;
                    state = PatrolState.WaitBeforeTurn;
                    stateTimer = waitBeforeTurn;
                }
                break;

            case PatrolState.WaitBeforeTurn:
                rb.linearVelocity = Vector2.zero;
                stateTimer -= Time.fixedDeltaTime;
                if (stateTimer <= 0f)
                {
                    direction = -direction; //flip direction
                    Vector2 newDir = moveAxis * direction;
                    targetFacingAngle = Mathf.Atan2(newDir.y, newDir.x) * Mathf.Rad2Deg;
                    state = PatrolState.Turning;
                }
                break;

            case PatrolState.Turning:
                rb.linearVelocity = Vector2.zero;
                //wait until cone finishes rotating
                if (Mathf.Abs(Mathf.DeltaAngle(currentFacingAngle, targetFacingAngle)) < 1f)
                {
                    state = PatrolState.WaitAfterTurn;
                    stateTimer = waitAfterTurn;
                }
                break;

            case PatrolState.WaitAfterTurn:
                rb.linearVelocity = Vector2.zero;
                stateTimer -= Time.fixedDeltaTime;
                if (stateTimer <= 0f)
                    state = PatrolState.Moving;
                break;

            case PatrolState.PlayerSpotted:
                rb.linearVelocity = Vector2.zero; //stop and stay stopped
                break;
        }
    }

    //draw line of sight cone
    void OnDrawGizmos()
    {
        Vector2 origin = (Vector2)transform.position;

        Vector2 facing;
        if (Application.isPlaying)
            facing = facingDir;
        else
            facing = axis == PatrolAxis.Horizontal
                ? (horizontalStart == HorizontalStart.Right ? Vector2.right : Vector2.left)
                : (verticalStart == VerticalStart.Up ? Vector2.up : Vector2.down);

        float halfAngle = viewAngle / 2f;
        Vector2 leftEdge  = (Vector2)(Quaternion.Euler(0, 0,  halfAngle) * facing) * viewDistance;
        Vector2 rightEdge = (Vector2)(Quaternion.Euler(0, 0, -halfAngle) * facing) * viewDistance;

        Gizmos.color = new Color(1f, 1f, 1f, 0.6f);
        Gizmos.DrawLine(origin, origin + leftEdge);
        Gizmos.DrawLine(origin, origin + rightEdge);

        int arcSegments = 20;
        for (int i = 0; i < arcSegments; i++)
        {
            float angleA = -halfAngle + (viewAngle / arcSegments) * i;
            float angleB = -halfAngle + (viewAngle / arcSegments) * (i + 1);
            Vector2 pointA = origin + (Vector2)(Quaternion.Euler(0, 0, angleA) * facing) * viewDistance;
            Vector2 pointB = origin + (Vector2)(Quaternion.Euler(0, 0, angleB) * facing) * viewDistance;
            Gizmos.DrawLine(pointA, pointB);
        }
    }

    //draw patrol range when selected
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 origin = Application.isPlaying ? startPosition : (Vector2)transform.position;

        if (axis == PatrolAxis.Horizontal)
        {
            Gizmos.DrawLine(origin + Vector2.left * patrolDistance, origin + Vector2.right * patrolDistance);
            Gizmos.DrawWireSphere(origin + Vector2.left * patrolDistance, 0.15f);
            Gizmos.DrawWireSphere(origin + Vector2.right * patrolDistance, 0.15f);
        }
        else
        {
            Gizmos.DrawLine(origin + Vector2.down * patrolDistance, origin + Vector2.up * patrolDistance);
            Gizmos.DrawWireSphere(origin + Vector2.down * patrolDistance, 0.15f);
            Gizmos.DrawWireSphere(origin + Vector2.up * patrolDistance, 0.15f);
        }
    }
}
