
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using TMPro;

public class Stage1UIManager : MonoBehaviour
{
    // 게임 오버 UI 관련 변수
    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel; // 게임 오버 패널
    [SerializeField] private TextMeshProUGUI gameOverScoreText; // 게임 오버 점수 텍스트
    [SerializeField] private Button retryButtonGameOver; // 게임 오버 패널의 다시 하기 버튼
    [SerializeField] private Button stageSelectButtonGameOver; // 게임 오버 패널의 스테이지 선택 버튼

    // 클리어 UI 관련 변수
    [Header("Clear UI")]
    [SerializeField] private GameObject clearPanel; // 클리어 패널
    [SerializeField] private TextMeshProUGUI clearScoreText; // 클리어 점수 텍스트
    [SerializeField] private Button retryButtonClear; // 클리어 패널의 다시 하기 버튼
    [SerializeField] private Button stageSelectButtonClear; // 클리어 패널의 스테이지 선택 버튼
    [SerializeField] private GameObject rankImage; // 클리어 시 표시될 랭크 이미지
    [SerializeField] private Sprite rankS; // S랭크 이미지
    [SerializeField] private Sprite rankA; // A랭크 이미지
    [SerializeField] private Sprite rankF; // F랭크 이미지

    private int score = 0; // 게임 점수 변수

    private void Start()
    {
        // 초기 UI 상태 설정
        gameOverPanel.SetActive(false); // 게임 오버 패널 숨기기
        clearPanel.SetActive(false); // 클리어 패널 숨기기

        // 버튼 클릭 이벤트 연결
        retryButtonGameOver.onClick.AddListener(RetryGame); // 게임 오버: 다시 하기 버튼 클릭 시 RetryGame 호출
        stageSelectButtonGameOver.onClick.AddListener(GoToStageSelect); // 게임 오버: 스테이지 선택 버튼 클릭 시 GoToStageSelect 호출

        retryButtonClear.onClick.AddListener(RetryGame); // 클리어: 다시 하기 버튼 클릭 시 RetryGame 호출
        stageSelectButtonClear.onClick.AddListener(GoToStageSelect); // 클리어: 스테이지 선택 버튼 클릭 시 GoToStageSelect 호출

        if (rankImage != null) rankImage.SetActive(false); // 랭크 이미지는 기본적으로 숨기기
    }

    // 점수 설정 (게임 로직에서 호출)
    public void SetScore(int newScore)
    {
        score = newScore; // 점수 업데이트
    }

    // 게임 오버 UI 표시
    public void ShowGameOverUI()
    {
        gameOverPanel.SetActive(true); // 게임 오버 패널 활성화
        gameOverScoreText.text = score.ToString(); // 점수 텍스트 업데이트
    }

    // 클리어 UI 표시
    public void ShowClearUI()
    {
        clearPanel.SetActive(true); // 클리어 패널 활성화
        clearScoreText.text = score.ToString(); // 점수 텍스트 업데이트

        // 클리어 시 점수에 따라 랭크 이미지 표시
        ShowRank();
    }

    // 클리어 시 랭크 표시
    private void ShowRank()
    {
        if (rankImage == null) return; // 랭크 이미지가 없으면 실행하지 않음

        rankImage.SetActive(true); // 랭크 이미지 활성화

        // 점수에 따른 랭크 결정
        if (score >= 1000)
        {
            rankImage.GetComponent<Image>().sprite = rankS; // S랭크
        }
        else if (score >= 500)
        {
            rankImage.GetComponent<Image>().sprite = rankA; // A랭크
        }
        else
        {
            rankImage.GetComponent<Image>().sprite = rankF; // F랭크
        }
    }

    // 다시 하기 버튼 동작
    private void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 다시 로드
    }

    // 스테이지 선택 버튼 동작
    private void GoToStageSelect()
    {
        SceneManager.LoadScene("Stage"); // 이동
    }
}
