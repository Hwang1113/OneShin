using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance => _instance;
    
    public TextMeshProUGUI scoreText; // 점수를 표시할 Text
    public Transform effectSpawnPoint; // 이펙트가 생성될 위치

    private int currentScore = 0;
    private bool[] isEffectPlayed = new bool[4]; // 4개의 점수 구간에 대한 이펙트 실행 여부


    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateScoreUI(); // 시작 시 UI 초기화

        /*
        if (effectSpawnPoint != null)
        {
            FxManager.Instance.PlayScoreEffect(effectSpawnPoint);
        }
        */

    }


    // 점수 추가 메서드
    public void AddScore(int score)
    {
        currentScore += score; // 점수 누적
        UpdateScoreUI(); // UI 업데이트

        // 점수 구간 확인
        Debug.Log("Current Score: " + currentScore);

        // 점수 조건문 실행 여부 확인
        Debug.Log("Checking Score Conditions...");

        // 점수 구간 확인 및 이펙트 호출
        if (currentScore >= 900 && currentScore < 1200)
        {
            TriggerScoreEffectOnce(0);
        }
        else if (currentScore >= 1200 && currentScore < 1500)
        {
            TriggerScoreEffectOnce(1);
        }
        else if (currentScore >= 1500 && currentScore < 2000)
        {
            TriggerScoreEffectOnce(2);
        }
        else if (currentScore >= 2000)
        {
            TriggerScoreEffectOnce(3);
        }

        /*
        // 특정 점수 구간에서 이펙트 실행
        if (currentScore == 900 || currentScore == 1200 || currentScore == 1500 || currentScore == 2000)
        {
            Debug.Log("Triggering Score Effect for Score: " + currentScore);
            FxManager.Instance.PlayScoreEffect(effectSpawnPoint);
        }
        */

        // Stage1UIManager와 점수 동기화
        Stage1UIManager.Instance.SetScore(currentScore);

        // 서브댄서 UI 표시 호출
        JudgeUIManager.Instance?.ShowSubDancerUI(currentScore);
    }

    private void TriggerScoreEffectOnce(int index)
    {
        if (!isEffectPlayed[index])
        {
            Debug.Log("Triggering Score Effect for Score: " + currentScore);
            FxManager.Instance.PlayScoreEffect(effectSpawnPoint);
            isEffectPlayed[index] = true;
        }
    }


    // 점수 UI 업데이트
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString(); // 점수를 UI에 반영
        }
    }

    // 최종 점수 설정 (게임 오버 UI에서 사용)
    public int GetFinalScore()
    {
        return currentScore;
    }
}