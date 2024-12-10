using UnityEngine;
using System.Collections;

public class UIFader : MonoBehaviour
{
    public CanvasGroup canvasGroup; // UI의 CanvasGroup 컴포넌트
    public float fadeDuration = 0.5f; // Fade 속도

    // Fade In 함수
    public void FadeInUI()
    {
        StartCoroutine(FadeInCoroutine());
    }

    // Fade Out 함수 (필요 시)
    public void FadeOutUI()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    // Fade In 동작
    private IEnumerator FadeInCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f; // 보장
    }

    // Fade Out 동작
    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            yield return null;
        }
        canvasGroup.alpha = 0f; // 보장
        gameObject.SetActive(false); // UI 비활성화
    }
}