
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using TMPro;

public class Stage1UIManager : MonoBehaviour
{
    // ���� ���� UI ���� ����
    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel; // ���� ���� �г�
    [SerializeField] private TextMeshProUGUI gameOverScoreText; // ���� ���� ���� �ؽ�Ʈ
    [SerializeField] private Button retryButtonGameOver; // ���� ���� �г��� �ٽ� �ϱ� ��ư
    [SerializeField] private Button stageSelectButtonGameOver; // ���� ���� �г��� �������� ���� ��ư

    // Ŭ���� UI ���� ����
    [Header("Clear UI")]
    [SerializeField] private GameObject clearPanel; // Ŭ���� �г�
    [SerializeField] private TextMeshProUGUI clearScoreText; // Ŭ���� ���� �ؽ�Ʈ
    [SerializeField] private Button retryButtonClear; // Ŭ���� �г��� �ٽ� �ϱ� ��ư
    [SerializeField] private Button stageSelectButtonClear; // Ŭ���� �г��� �������� ���� ��ư
    [SerializeField] private GameObject rankImage; // Ŭ���� �� ǥ�õ� ��ũ �̹���
    [SerializeField] private Sprite rankS; // S��ũ �̹���
    [SerializeField] private Sprite rankA; // A��ũ �̹���
    [SerializeField] private Sprite rankF; // F��ũ �̹���

    private int score = 0; // ���� ���� ����

    private void Start()
    {
        // �ʱ� UI ���� ����
        gameOverPanel.SetActive(false); // ���� ���� �г� �����
        clearPanel.SetActive(false); // Ŭ���� �г� �����

        // ��ư Ŭ�� �̺�Ʈ ����
        retryButtonGameOver.onClick.AddListener(RetryGame); // ���� ����: �ٽ� �ϱ� ��ư Ŭ�� �� RetryGame ȣ��
        stageSelectButtonGameOver.onClick.AddListener(GoToStageSelect); // ���� ����: �������� ���� ��ư Ŭ�� �� GoToStageSelect ȣ��

        retryButtonClear.onClick.AddListener(RetryGame); // Ŭ����: �ٽ� �ϱ� ��ư Ŭ�� �� RetryGame ȣ��
        stageSelectButtonClear.onClick.AddListener(GoToStageSelect); // Ŭ����: �������� ���� ��ư Ŭ�� �� GoToStageSelect ȣ��

        if (rankImage != null) rankImage.SetActive(false); // ��ũ �̹����� �⺻������ �����
    }

    // ���� ���� (���� �������� ȣ��)
    public void SetScore(int newScore)
    {
        score = newScore; // ���� ������Ʈ
    }

    // ���� ���� UI ǥ��
    public void ShowGameOverUI()
    {
        gameOverPanel.SetActive(true); // ���� ���� �г� Ȱ��ȭ
        gameOverScoreText.text = score.ToString(); // ���� �ؽ�Ʈ ������Ʈ
    }

    // Ŭ���� UI ǥ��
    public void ShowClearUI()
    {
        clearPanel.SetActive(true); // Ŭ���� �г� Ȱ��ȭ
        clearScoreText.text = score.ToString(); // ���� �ؽ�Ʈ ������Ʈ

        // Ŭ���� �� ������ ���� ��ũ �̹��� ǥ��
        ShowRank();
    }

    // Ŭ���� �� ��ũ ǥ��
    private void ShowRank()
    {
        if (rankImage == null) return; // ��ũ �̹����� ������ �������� ����

        rankImage.SetActive(true); // ��ũ �̹��� Ȱ��ȭ

        // ������ ���� ��ũ ����
        if (score >= 1000)
        {
            rankImage.GetComponent<Image>().sprite = rankS; // S��ũ
        }
        else if (score >= 500)
        {
            rankImage.GetComponent<Image>().sprite = rankA; // A��ũ
        }
        else
        {
            rankImage.GetComponent<Image>().sprite = rankF; // F��ũ
        }
    }

    // �ٽ� �ϱ� ��ư ����
    private void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���� �� �ٽ� �ε�
    }

    // �������� ���� ��ư ����
    private void GoToStageSelect()
    {
        SceneManager.LoadScene("Stage"); // �̵�
    }
}
