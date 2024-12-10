using UnityEngine;
using System.Collections;

public class UIFader : MonoBehaviour
{
    public CanvasGroup canvasGroup; // UI�� CanvasGroup ������Ʈ
    public float fadeDuration = 0.5f; // Fade �ӵ�

    // Fade In �Լ�
    public void FadeInUI()
    {
        StartCoroutine(FadeInCoroutine());
    }

    // Fade Out �Լ� (�ʿ� ��)
    public void FadeOutUI()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    // Fade In ����
    private IEnumerator FadeInCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f; // ����
    }

    // Fade Out ����
    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            yield return null;
        }
        canvasGroup.alpha = 0f; // ����
        gameObject.SetActive(false); // UI ��Ȱ��ȭ
    }
}