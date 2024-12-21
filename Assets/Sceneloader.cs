using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public Image fadeImage; // UI 黑幕
    public float fadeDuration = 1f; // 淡出的時間

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // 按下 Enter 鍵
        {
            StartCoroutine(SwitchToScene("Boss"));
        }
    }

    /// <summary>
    /// 切換場景並淡出
    /// </summary>
    /// <param name="sceneName">要切換的場景名稱</param>
    private IEnumerator SwitchToScene(string sceneName)
    {
        // 開始淡出
        yield return StartCoroutine(FadeOut());

        // 加載場景
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// 淡出過程
    /// </summary>
    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration); // 設置透明度
            fadeImage.color = color;
            yield return null; // 等待下一幀
        }

        // 確保完全不透明
        color.a = 1f;
        fadeImage.color = color;
    }
}
