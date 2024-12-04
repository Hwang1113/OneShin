
using UnityEngine;
using UnityEngine.SceneManagement;

public class OS_MainUIManager : MonoBehaviour
{
    public GameObject pressAnyKeyImage; // "Press Any Key" �̹���
    public GameObject tutorialImage; // Ʃ�丮�� �̹���
    public GameObject startButton; // "Start" ��ư

    private bool isKeyPressed = false; // Ű �Է� Ȯ�� ����

    void Start()
    {
        // ���� �� Ʃ�丮�� �̹����� Start ��ư�� ����
        tutorialImage.SetActive(false);
        startButton.SetActive(false);
    }

    void Update()
    {
        if (!isKeyPressed && Input.anyKeyDown)
        {
            isKeyPressed = true;
            ShowTutorial(); // Ʃ�丮�� �̹����� ��ư ǥ��
        }
    }

    void ShowTutorial()
    {
        // "Press Any Key" �̹��� �����
        pressAnyKeyImage.SetActive(false);

        // Ʃ�丮�� �̹����� Start ��ư Ȱ��ȭ
        tutorialImage.SetActive(true);
        startButton.SetActive(true);
    }

    public void OnStartButtonClicked()
    {
        // �������������� �̵�
        SceneManager.LoadScene("Stage");
    }
}