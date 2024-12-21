using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSceneAttackController : MonoBehaviour
{
    [SerializeField] GameObject missilePrefab;
    [SerializeField] float attackInterval;
    [SerializeField] float aimTime;
    [SerializeField] float launchTime;  // time before launch, after aim

    [SerializeField] Transform indicator;
    public GameObject FloorPos;
    private FloorPosition floor_script;

    bool attacking = false;
    float attackTimer;
    Vector3 mousePos;

    Vector3 indicatorScreenPos; // (0-Screen.width, 0-Screen.height, 0)

    void Start()
    {
        attackTimer = attackInterval;
        indicator.gameObject.SetActive(false);
        floor_script = FloorPos.GetComponent<FloorPosition>();
    }

    void Update()
    {
        // temp: use mouse to aim
        mousePos = Input.mousePosition;

        Vector3 indicatorNewPos = Camera.main.ScreenToWorldPoint(indicatorScreenPos);
        indicator.position = new Vector3(indicatorNewPos.x, indicatorNewPos.y, 0f);

        if (!attacking && attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer < 0f)
            {
                StartCoroutine(MissileAttack());
                attacking = true;
                attackTimer = attackInterval;
            }
        }
    }

    IEnumerator MissileAttack()
    {
        // 0 for upward, 1 for leftward, 2 for downward, 3 for rightward
        int index = Random.Range(0, 4);
        indicator.gameObject.SetActive(true);

        // aim
        float[] angles = { 0f, 90f, 180f, 270f };
        indicator.eulerAngles = new Vector3(0, 0, angles[index]);
        float timer = 0f;
        while (timer < aimTime)
        {
            float[] xs = { (float)floor_script.OutPosVec.x, 0.98f, (float)floor_script.OutPosVec.x, 0.02f };
            float[] ys = { 0.02f, (float)floor_script.OutPosVec.y, 0.98f, (float)floor_script.OutPosVec.y / Screen.height };

            float x = xs[index];
            float y = ys[index];
            indicatorScreenPos = new Vector3(x * Screen.width, y * Screen.height);

            timer += Time.deltaTime;
            yield return null;
        }

        // launch
        yield return new WaitForSeconds(launchTime);

        Vector3[] directions = { Vector2.up, Vector2.left, Vector2.down, Vector2.right };
        Bullet missile = Instantiate(missilePrefab, indicator.position, Quaternion.identity).GetComponent<Bullet>();
        missile.SetDirection(directions[index]);

        attacking = false;
        indicator.gameObject.SetActive(false);
    }
}
