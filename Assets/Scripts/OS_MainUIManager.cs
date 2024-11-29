
using UnityEngine;
using UnityEngine.SceneManagement;

public class OS_MainUIManager : MonoBehaviour
{
    public GameObject pressAnyKeyImage; // "Press Any Key" 이미지
    public GameObject tutorialImage; // 튜토리얼 이미지
    public GameObject startButton; // "Start" 버튼

    private bool isKeyPressed = false; // 키 입력 확인 변수

    void Start()
    {
        // 시작 시 튜토리얼 이미지와 Start 버튼을 숨김
        tutorialImage.SetActive(false);
        startButton.SetActive(false);
    }

    void Update()
    {
        if (!isKeyPressed && Input.anyKeyDown)
        {
            isKeyPressed = true;
            ShowTutorial(); // 튜토리얼 이미지와 버튼 표시
        }
    }

    void ShowTutorial()
    {
        // "Press Any Key" 이미지 숨기기
        pressAnyKeyImage.SetActive(false);

        // 튜토리얼 이미지와 Start 버튼 활성화
        tutorialImage.SetActive(true);
        startButton.SetActive(true);
    }

    public void OnStartButtonClicked()
    {
        // 스테이지씬으로 이동
        SceneManager.LoadScene("Stage");
    }
}