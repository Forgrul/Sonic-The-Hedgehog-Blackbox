using UnityEngine;
using UnityEngine.UI;
using Giometric.UniSonic;

public class SonicHealthHealth : MonoBehaviour
{
    public Text healthText; // 指向顯示血量的 UI 元件
    public Movement Sonic;       // 指向 Boss 物件

    void Update()
    {
        healthText.text = $"X {Sonic.health}";
    }
}
