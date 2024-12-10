using UnityEngine;
using System.Collections;

public class FxManager : MonoBehaviour
{
    public GameObject scoreEffect; // ����� FX ������
    public GameObject perfectEffect; // perfect ������
    public GameObject goodEffect; // good ������

    public float effectDuration = 1.5f; // FX�� ǥ�õǴ� �ð� (�� ����)
   
    public static FxManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }




    public void PlayScoreEffect(Transform spawnPoint)
    {
        if (scoreEffect != null && spawnPoint != null)
        {
            Debug.Log("Spawning Score Effect at: " + spawnPoint.position);

            // ����Ʈ�� �����ϰų� ��Ȱ��
            GameObject effectInstance = Instantiate(scoreEffect, spawnPoint.position, Quaternion.identity);
            effectInstance.SetActive(true);

            // ���� �ð� �� ��Ȱ��ȭ
            StartCoroutine(DisableEffectAfterDelay(effectInstance, effectDuration));
        }
    }

    private IEnumerator DisableEffectAfterDelay(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (effect != null)
        {
            Debug.Log("Disabling Score Effect: " + effect.name);
            // effect.SetActive(false);
            Destroy(effect); // ��Ȱ��ȭ ��� �ı��� ���
        }
        else
        {
            Debug.LogError("Effect is already null when trying to disable it.");
        }

    }


    // ����Ʈ�϶� ������
    public void PlayPerfectEffect(Vector3 position)
     {
         if (perfectEffect != null)
         {
                GameObject effect = Instantiate(perfectEffect, position, Quaternion.identity);
                effect.SetActive(true);

                // ���� �ð� �� �ı�
                Destroy(effect, 1.0f); // 1�� �� �ı�
         }
     }

     // ���϶� ������
    public void PlayGoodEffect(Vector3 position)
    {
         if (goodEffect != null)
         {
                GameObject effect = Instantiate(goodEffect, position, Quaternion.identity);
                effect.SetActive(true);

                // ���� �ð� �� �ı�
                Destroy(effect, 1.0f); // 1�� �� �ı�
         }
    }

  
}