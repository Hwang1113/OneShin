
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using UnityEngine.SceneManagement; 

public class OS_StageUIManager : MonoBehaviour
{
    [SerializeField] private List<SongData> songList; // 곡 데이터를 저장하는 리스트
    private int currentSongIndex = 0; // 현재 선택된 곡의 인덱스

    [SerializeField] private TMP_Text songTitleText; // 곡 제목 텍스트
    [SerializeField] private Image thumbnailImage; // 썸네일 이미지
    [SerializeField] private Button playButton; // 플레이 버튼
    [SerializeField] private Button leftButton; // 왼쪽 버튼
    [SerializeField] private Button rightButton; // 오른쪽 버튼
    [SerializeField] private AudioSource songAudioSource; // 오디오 소스

    void Start()
    {
        // 버튼 클릭 이벤트 연결
        leftButton.onClick.AddListener(SelectPreviousSong);
        rightButton.onClick.AddListener(SelectNextSong);
        playButton.onClick.AddListener(LoadGameScene);

        // 첫 번째 곡을 표시하고 재생
        UpdateUI();
        PlayCurrentSong();
    }

    // UI를 현재 곡 데이터로 업데이트
    private void UpdateUI()
    {
        if (songList.Count == 0) return; // 곡 데이터가 없으면 실행하지 않음

        // 현재 곡의 제목, 썸네일 이미지 설정
        songTitleText.text = songList[currentSongIndex].songTitle;
        thumbnailImage.sprite = songList[currentSongIndex].thumbnailImage;
    }

    // 현재 선택된 곡을 재생
    private void PlayCurrentSong()
    {
        if (songList.Count == 0) return; // 곡 데이터가 없으면 실행하지 않음

        // 현재 곡 정보를 출력
        Debug.Log("Now playing: " + songList[currentSongIndex].songTitle);

        // 오디오 소스에 곡 연결 및 재생
        songAudioSource.clip = songList[currentSongIndex].songClip;
        songAudioSource.Play();
    }

    // 이전 곡 선택
    private void SelectPreviousSong()
    {
        currentSongIndex--; // 인덱스를 감소
        if (currentSongIndex < 0) currentSongIndex = songList.Count - 1; // 첫 곡 이전이면 마지막 곡으로 이동
        UpdateUI(); // UI 업데이트
        PlayCurrentSong(); // 곡 재생
    }

    // 다음 곡 선택
    private void SelectNextSong()
    {
        currentSongIndex++; // 인덱스를 증가
        if (currentSongIndex >= songList.Count) currentSongIndex = 0; // 마지막 곡 이후면 첫 곡으로 이동
        UpdateUI(); // UI 업데이트
        PlayCurrentSong(); // 곡 재생
    }

    // 선택된 곡의 게임 씬으로 이동
    private void LoadGameScene()
    {
        if (songList.Count == 0) return; // 곡 데이터가 없으면 실행하지 않음

        // 현재 곡의 씬 이름 가져오기
        string sceneName = songList[currentSongIndex].sceneName;

        // 씬 로드
        SceneManager.LoadScene(sceneName);
    }
}
