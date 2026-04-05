using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 5f;
    private Transform _player;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                _player = p.transform;
            }
            return;
        }
    }

    void FixedUpdate()
    {
        
        Vector2 direction = (_player.position - transform.position).normalized;
        _rb.linearVelocity = direction * speed;
    }
}