using UnityEngine;
using Unity.Cinemachine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D mapBoundary;
    private CinemachineConfiner2D confiner;

    private void Awake()
    {
        confiner = FindFirstObjectByType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && confiner != null)
        {
            confiner.BoundingShape2D = mapBoundary;
            confiner.InvalidateBoundingShapeCache();
        }
    }
}
