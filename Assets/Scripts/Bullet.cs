using UnityEngine;

public class Bullet : MonoBehaviour
{   
    public float bulletSpeed;
    public float lifeTime;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        
        rb.linearVelocity = transform.forward * bulletSpeed;
        
        Destroy(gameObject, lifeTime);
    }
}