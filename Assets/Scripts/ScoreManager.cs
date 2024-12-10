using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance => _instance;
    
    public TextMeshProUGUI scoreText; // ������ ǥ���� Text
    public Transform effectSpawnPoint; // ����Ʈ�� ������ ��ġ

    private int currentScore = 0;
    private bool[] isEffectPlayed = new bool[4]; // 4���� ���� ������ ���� ����Ʈ ���� ����


    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateScoreUI(); // ���� �� UI �ʱ�ȭ

        /*
        if (effectSpawnPoint != null)
        {
            FxManager.Instance.PlayScoreEffect(effectSpawnPoint);
        }
        */

    }


    // ���� �߰� �޼���
    public void AddScore(int score)
    {
        currentScore += score; // ���� ����
        UpdateScoreUI(); // UI ������Ʈ

        // ���� ���� Ȯ��
        Debug.Log("Current Score: " + currentScore);

        // ���� ���ǹ� ���� ���� Ȯ��
        Debug.Log("Checking Score Conditions...");

        // ���� ���� Ȯ�� �� ����Ʈ ȣ��
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
        // Ư�� ���� �������� ����Ʈ ����
        if (currentScore == 900 || currentScore == 1200 || currentScore == 1500 || currentScore == 2000)
        {
            Debug.Log("Triggering Score Effect for Score: " + currentScore);
            FxManager.Instance.PlayScoreEffect(effectSpawnPoint);
        }
        */

        // Stage1UIManager�� ���� ����ȭ
        Stage1UIManager.Instance.SetScore(currentScore);

        // ����� UI ǥ�� ȣ��
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


    // ���� UI ������Ʈ
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString(); // ������ UI�� �ݿ�
        }
    }

    // ���� ���� ���� (���� ���� UI���� ���)
    public int GetFinalScore()
    {
        return currentScore;
    }
}