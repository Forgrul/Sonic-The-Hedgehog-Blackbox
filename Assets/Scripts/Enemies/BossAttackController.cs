using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float spawnOffset = 20f;

    [SerializeField] 
    private float attackInterval = 3f;

    private float attackTimer;
    private bool stop = false;

    void Start()
    {
        attackTimer = attackInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if(stop) return;
        Vector3 mousePos = Input.mousePosition;
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePos);
        attackTimer -= Time.deltaTime;
        if(attackTimer < 0f)
        {
            attackTimer = attackInterval;
            Shoot(mousePosWorld);
        }
    }

    void Shoot(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        direction.z = 0f;
        direction = direction.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x);

        GameObject bullet = Instantiate(bulletPrefab, transform.position + direction * spawnOffset, Quaternion.Euler(0, 0, angle));
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetDirection(direction);
    }

    public void StopAttacking()
    {
        stop = true;
    }
}
