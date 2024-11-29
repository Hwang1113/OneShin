
using UnityEngine;
using UnityEngine.SceneManagement;

public class OS_StageUIManager : MonoBehaviour
{
    public GameObject[] songThumbnails; // 곡 썸네일들
    public GameObject xButton; // X 버튼

    public void OnSongSelected(string songName)
    {
        // 선택한 곡 정보를 저장하고 게임씬으로 이동. 게임씬이랑 연동방법생각
        //PlayerPrefs.SetString("SelectedSong", songName);
        SceneManager.LoadScene("GameScene");
    }

    public void OnBackButtonClicked()
    {
        // 메인타이틀씬으로 이동
        SceneManager.LoadScene("MainTitleScene");
    }
}