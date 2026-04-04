using UnityEngine;

public class Bullet : MonoBehaviour
{   
    public float bulletSpeed = 20f;
    public float lifeTime = 3f;

    void Start()
    {
        Debug.Log("Bullet Start");
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        
        rb.linearVelocity = transform.right * bulletSpeed;
        
        Destroy(gameObject, lifeTime);
    }
}