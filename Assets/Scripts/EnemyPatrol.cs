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
    [SerializeField] private float waitTime = 0.5f; //time stopped at the edges

    [Header("Line of Sight")]
    [SerializeField] private float viewAngle = 70f;    //total cone angle in degrees
    [SerializeField] private float viewDistance = 5f;  //how far the cone reaches
    [SerializeField] private float turnSpeed = 240f;   //degrees per second for the 180 turn

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private int direction = 1;
    private float waitTimer = 0f; //tracks remaining wait time
    private bool isWaiting = false;
    private Vector2 facingDir = Vector2.right; //smoothed facing direction used for cone

    private float currentFacingAngle;
    private float targetFacingAngle;

    private Mesh coneMesh;
    private MeshFilter coneMeshFilter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
        direction = axis == PatrolAxis.Horizontal
            ? (horizontalStart == HorizontalStart.Right ? 1 : -1)
            : (verticalStart == VerticalStart.Up ? 1 : -1); //set starting direction from inspector

        Vector2 initialDir = axis == PatrolAxis.Horizontal
            ? (direction == 1 ? Vector2.right : Vector2.left)
            : (direction == 1 ? Vector2.up : Vector2.down);

        currentFacingAngle = Mathf.Atan2(initialDir.y, initialDir.x) * Mathf.Rad2Deg;
        targetFacingAngle  = currentFacingAngle;
        facingDir = initialDir;

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

        mr.sortingLayerName = "WalkInFront";
        mr.sortingOrder = 0;

        coneMesh = new Mesh();
        coneMeshFilter.mesh = coneMesh;
    }

    void BuildConeMesh()
    {
        int segments = 24;
        float halfAngle = viewAngle / 2f;

        Vector3[] vertices = new Vector3[segments + 2]; //origin + arc points
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
        //smoothly rotate toward target angle
        currentFacingAngle = Mathf.MoveTowardsAngle(currentFacingAngle, targetFacingAngle, turnSpeed * Time.deltaTime);
        float rad = currentFacingAngle * Mathf.Deg2Rad;
        facingDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        BuildConeMesh();
    }

    void FixedUpdate()
    {
        if (isWaiting)
        {
            rb.linearVelocity = Vector2.zero; //stop while waiting
            waitTimer -= Time.fixedDeltaTime;
            if (waitTimer <= 0f)
                isWaiting = false;
            return;
        }

        Vector2 moveDir = axis == PatrolAxis.Horizontal ? Vector2.right : Vector2.up;

        float currentOffset = axis == PatrolAxis.Horizontal
            ? rb.position.x - startPosition.x
            : rb.position.y - startPosition.y;

        if (currentOffset >= patrolDistance && direction == 1)
        {
            direction = -1;
            isWaiting = true;
            waitTimer = waitTime; //start wait at far end
        }
        else if (currentOffset <= -patrolDistance && direction == -1)
        {
            direction = 1;
            isWaiting = true;
            waitTimer = waitTime; //start wait at near end
        }

        rb.linearVelocity = moveDir * direction * speed;

        //set target angle — Update() will rotate smoothly toward it
        Vector2 targetDir = moveDir * direction;
        targetFacingAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
    }

    //draw line of sight cone in scene view
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

        Gizmos.color = new Color(1f, 1f, 1f, 0.6f); //white
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
