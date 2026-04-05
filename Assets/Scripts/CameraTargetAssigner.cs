using UnityEngine;
using Unity.Cinemachine;

public class CameraTargetAssigner : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cineCam;

    private void Start()
    {
        if (cineCam == null)
            cineCam = FindFirstObjectByType<CinemachineCamera>();

        if (cineCam == null)
        {
            Debug.LogError("No active CinemachineCamera found.");
            return;
        }

        cineCam.Target.TrackingTarget = transform;
        Debug.Log("Camera now tracking: " + gameObject.name);
    }
}