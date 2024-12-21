using UnityEngine;
using UnityEngine.UI;

public class SetTransparency : MonoBehaviour
{
    public Image fadeImage; // 拖入需要設置的 Image 元件

    private void Start()
    {
        SetAlphaToZero();
    }

    /// <summary>
    /// 將透明度設為 0
    /// </summary>
    private void SetAlphaToZero()
    {
        Color color = fadeImage.color; // 獲取當前顏色
        color.a = 0f;                 // 將透明度設為 0
        fadeImage.color = color;      // 設置回去
    }
}
