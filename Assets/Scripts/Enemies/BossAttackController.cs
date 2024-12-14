using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float spawnOffset = 20f;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePos);
        if(Input.GetKeyDown(KeyCode.Mouse0))
            Shoot(mousePosWorld);
    }

    void Shoot(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        Vector3 direction2d = direction;
        direction2d = direction2d.normalized;

        float angle = Mathf.Atan2(direction2d.y, direction2d.x);

        GameObject bullet = Instantiate(bulletPrefab, transform.position + direction2d * spawnOffset, Quaternion.Euler(0, 0, angle));
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetDirection(direction2d);
    }
}
