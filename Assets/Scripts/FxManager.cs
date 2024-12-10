using UnityEngine;
using System.Collections;

public class FxManager : MonoBehaviour
{
    public GameObject scoreEffect; // 사용할 FX 프리팹
    public GameObject perfectEffect; // perfect 프리팹
    public GameObject goodEffect; // good 프리팹

    public float effectDuration = 1.5f; // FX가 표시되는 시간 (초 단위)
   
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

            // 이펙트를 생성하거나 재활용
            GameObject effectInstance = Instantiate(scoreEffect, spawnPoint.position, Quaternion.identity);
            effectInstance.SetActive(true);

            // 일정 시간 후 비활성화
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
            Destroy(effect); // 비활성화 대신 파괴할 경우
        }
        else
        {
            Debug.LogError("Effect is already null when trying to disable it.");
        }

    }


    // 퍼펙트일때 나오기
    public void PlayPerfectEffect(Vector3 position)
     {
         if (perfectEffect != null)
         {
                GameObject effect = Instantiate(perfectEffect, position, Quaternion.identity);
                effect.SetActive(true);

                // 일정 시간 후 파괴
                Destroy(effect, 1.0f); // 1초 후 파괴
         }
     }

     // 굿일때 나오기
    public void PlayGoodEffect(Vector3 position)
    {
         if (goodEffect != null)
         {
                GameObject effect = Instantiate(goodEffect, position, Quaternion.identity);
                effect.SetActive(true);

                // 일정 시간 후 파괴
                Destroy(effect, 1.0f); // 1초 후 파괴
         }
    }

  
}