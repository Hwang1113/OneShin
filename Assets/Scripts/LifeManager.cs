
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    private static LifeManager _instance;
    public static LifeManager Instance => _instance;

    [SerializeField] private int maxLives = 5; // 일단 5개로 설정
    private int currentLives;

    // 라이프 배열
    [SerializeField] private Image[] lifeIcons;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    public bool IsGameOver { get; private set; } = false; // 게임오버 상태

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        currentLives = maxLives;
        UpdateLivesUI();
    }

    public void LoseLife()
    {
        // if (IsGameOver) return; // 게임오버 상태에서는 라이프 감소 불가

        currentLives--;
        UpdateLivesUI();
        
        if (currentLives <= 0)
        {
            GameOver();
        }

    }

    private void GameOver()
    {
        IsGameOver = true;
        Debug.Log("Game Over");
        Stage1UIManager.Instance.ShowGameOverUI(); // 게임 오버 UI 표시
        JudgeUIManager.Instance.OnGameOver(); // JudgeUIManager에 게임 오버 알림
    }


    /* 하트 추가되는 이벤트 생기면 사용 (피버때? 아니면 다른것생각)
    public void GainLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            UpdateLivesUI();
        }
    }
    */

    private void UpdateLivesUI() // 라이프 UI바뀜 빈걸로
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].sprite = i < currentLives ? fullHeart : emptyHeart;
        }
    }
}