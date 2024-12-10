
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    private static LifeManager _instance;
    public static LifeManager Instance => _instance;

    [SerializeField] private int maxLives = 5; // �ϴ� 5���� ����
    private int currentLives;

    // ������ �迭
    [SerializeField] private Image[] lifeIcons;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    public bool IsGameOver { get; private set; } = false; // ���ӿ��� ����

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
        // if (IsGameOver) return; // ���ӿ��� ���¿����� ������ ���� �Ұ�

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
        Stage1UIManager.Instance.ShowGameOverUI(); // ���� ���� UI ǥ��
        JudgeUIManager.Instance.OnGameOver(); // JudgeUIManager�� ���� ���� �˸�
    }


    /* ��Ʈ �߰��Ǵ� �̺�Ʈ ����� ��� (�ǹ���? �ƴϸ� �ٸ��ͻ���)
    public void GainLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            UpdateLivesUI();
        }
    }
    */

    private void UpdateLivesUI() // ������ UI�ٲ� ��ɷ�
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].sprite = i < currentLives ? fullHeart : emptyHeart;
        }
    }
}