
using UnityEngine;
using UnityEngine.SceneManagement;

public class OS_StageUIManager : MonoBehaviour
{
    public GameObject[] songThumbnails; // �� ����ϵ�
    public GameObject xButton; // X ��ư

    public void OnSongSelected(string songName)
    {
        // ������ �� ������ �����ϰ� ���Ӿ����� �̵�. ���Ӿ��̶� �����������
        //PlayerPrefs.SetString("SelectedSong", songName);
        SceneManager.LoadScene("GameScene");
    }

    public void OnBackButtonClicked()
    {
        // ����Ÿ��Ʋ������ �̵�
        SceneManager.LoadScene("MainTitleScene");
    }
}