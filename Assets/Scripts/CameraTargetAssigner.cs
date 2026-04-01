using UnityEngine;
using Unity.Cinemachine; 

public class CameraTargetAssigner : MonoBehaviour
{
    void Start()
    {
        // 1. Find your Cinemachine camera in the scene
        CinemachineCamera cineCam = Object.FindFirstObjectByType<CinemachineCamera>();

        if (cineCam != null)
        {
            cineCam.Target.TrackingTarget = this.transform;
        }
    }
}