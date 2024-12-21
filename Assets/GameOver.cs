using System.Collections; // 引入 System.Collections 命名空間
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // 引入 TextMeshPro 命名空間
using Giometric.UniSonic.Enemies;
using Giometric.UniSonic;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI victoryText; // Victory Text UI (TextMeshProUGUI)
    [SerializeField] private TextMeshProUGUI HintText;
    [SerializeField] private float duration = 2f;

    public Boss boss;
    public Movement Sonic;
    private bool isVictory = false;
    private Vector3 originalScale = Vector3.zero;
    private Vector3 targetScale = new Vector3(4f, 2f, 1f);
    private float waitTimer = 0f;

    void Start()
    {
        // 初始化為隱藏狀態
        // targetScale = new Vector3(4f, 2f, 1f);
        victoryText.transform.localScale = originalScale;
        HintText.transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (boss.GetCurrentHealth() <= 0 && !isVictory)
        {
            if (waitTimer == 0f)
            {
                waitTimer = Time.time;
            }
            if (Time.time - waitTimer >= 5f)
            {
                isVictory = true;
                if(Sonic.health <= 0) {
                    victoryText.text = "Victory!";
                } else {
                    victoryText.text = "Lose...";
                }
                StartCoroutine(ScaleText());
            }
        } 
        if (isVictory && Time.time - waitTimer >= 7f) {
                HintText.gameObject.SetActive(true);
                HintText.transform.localScale = new Vector3(1f, 1f, 0f);
        }

        // 監聽玩家按下Enter來加載新場景
        if (isVictory && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("SonicTitle");
        }
    }

    public void TriggerVictory()
    {
        // 當 Boss 死亡時調用此方法
        isVictory = true;
        victoryText.gameObject.SetActive(true); // 顯示 Victory 文字
    }

    private IEnumerator ScaleText()
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            // 平滑縮放，使用 Lerp
            victoryText.transform.localScale = Vector3.Lerp(originalScale, targetScale, timeElapsed / duration);
            yield return null;
        }

        // 確保最後達到精確的目標大小
        victoryText.transform.localScale = targetScale;
    }
}
