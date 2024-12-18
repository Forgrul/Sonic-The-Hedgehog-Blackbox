using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Giometric.UniSonic;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float despawnTime = 10f;
    [SerializeField]
    private float speed = 60f;

    private Rigidbody2D rb;
    private float despawnTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        despawnTimer = despawnTime;
    }

    void Update()
    {
        if (rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        despawnTime -= Time.deltaTime;
        if(despawnTime < 0f)
            Destroy(gameObject);
    }

    public void SetDirection(Vector2 direction)
    {
        rb.velocity = direction * speed;
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
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
    }
}
