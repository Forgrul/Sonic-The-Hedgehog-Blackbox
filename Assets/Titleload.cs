using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageFaderWithoutCanvasGroup : MonoBehaviour
{
    public Image image1;  // 拖入 Image-1
    public Image image2;  // 拖入 Image-2

    private void Start()
    {
        // 開始協程實現漸變
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        // 1. 確保 Image-1 初始狀態完全不透明，Image-2 初始透明
        SetAlpha(image1, 1f);
        SetAlpha(image2, 0f);

        // 2. 展示 Image-1 兩秒
        yield return new WaitForSeconds(2f);

        // 3. 淡出 Image-1 並淡入 Image-2
        yield return StartCoroutine(FadeOut(image1, 1f));
        yield return StartCoroutine(FadeIn(image2, 1f));
    }

    private IEnumerator FadeIn(Image img, float duration)
    {
        float elapsedTime = 0f;
        Color color = img.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / duration);
            img.color = color;
            yield return null;
        }

        color.a = 1f;
        img.color = color;
    }

    private IEnumerator FadeOut(Image img, float duration)
    {
        float elapsedTime = 0f;
        Color color = img.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(1f - (elapsedTime / duration));
            img.color = color;
            yield return null;
        }

        color.a = 0f;
        img.color = color;
    }

    private void SetAlpha(Image img, float alpha)
    {
        Color color = img.color;
        color.a = alpha;
        img.color = color;
    }
}
