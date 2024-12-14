using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Giometric.UniSonic;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 60f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void SetDirection(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Movement player = other.gameObject.GetComponent<Movement>();
            if(!player.IsInvulnerable)
            {
                player.SetHitState(transform.position);
                Destroy(gameObject);
            }
        }
        else if(!other.gameObject.CompareTag("Boss") && !other.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
