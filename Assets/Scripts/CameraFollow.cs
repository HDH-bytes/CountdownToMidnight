using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(-1.6f, 0.2f, -10f);

    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}