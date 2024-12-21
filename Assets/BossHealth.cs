using UnityEngine;
using UnityEngine.UI;
using Giometric.UniSonic.Enemies;

public class BossHealth : MonoBehaviour
{
    public Text healthText; // 指向顯示血量的 UI 元件
    public Boss boss;       // 指向 Boss 物件

    void Start()
    {
        if (healthText == null)
            Debug.LogError("HealthText is not assigned in the Inspector!");

        if (boss == null)
            Debug.LogError("Boss is not assigned in the Inspector!");
    }

    void Update()
    {
        // 檢查引用是否為 null
        if (healthText != null && boss != null)
        {
            healthText.text = $"X {boss.GetCurrentHealth()}";
        } else
        {
            Debug.LogWarning("Missing reference: healthText or boss is not set.");
        }
    }
}
