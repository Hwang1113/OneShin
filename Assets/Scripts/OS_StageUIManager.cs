
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using UnityEngine.SceneManagement; 

public class OS_StageUIManager : MonoBehaviour
{
    [SerializeField] private List<SongData> songList; // �� �����͸� �����ϴ� ����Ʈ
    private int currentSongIndex = 0; // ���� ���õ� ���� �ε���

    [SerializeField] private TMP_Text songTitleText; // �� ���� �ؽ�Ʈ
    [SerializeField] private Image thumbnailImage; // ����� �̹���
    [SerializeField] private Button playButton; // �÷��� ��ư
    [SerializeField] private Button leftButton; // ���� ��ư
    [SerializeField] private Button rightButton; // ������ ��ư
    [SerializeField] private AudioSource songAudioSource; // ����� �ҽ�

    void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ ����
        leftButton.onClick.AddListener(SelectPreviousSong);
        rightButton.onClick.AddListener(SelectNextSong);
        playButton.onClick.AddListener(LoadGameScene);

        // ù ��° ���� ǥ���ϰ� ���
        UpdateUI();
        PlayCurrentSong();
    }

    // UI�� ���� �� �����ͷ� ������Ʈ
    private void UpdateUI()
    {
        if (songList.Count == 0) return; // �� �����Ͱ� ������ �������� ����

        // ���� ���� ����, ����� �̹��� ����
        songTitleText.text = songList[currentSongIndex].songTitle;
        thumbnailImage.sprite = songList[currentSongIndex].thumbnailImage;
    }

    // ���� ���õ� ���� ���
    private void PlayCurrentSong()
    {
        if (songList.Count == 0) return; // �� �����Ͱ� ������ �������� ����

        // ���� �� ������ ���
        Debug.Log("Now playing: " + songList[currentSongIndex].songTitle);

        // ����� �ҽ��� �� ���� �� ���
        songAudioSource.clip = songList[currentSongIndex].songClip;
        songAudioSource.Play();
    }

    // ���� �� ����
    private void SelectPreviousSong()
    {
        currentSongIndex--; // �ε����� ����
        if (currentSongIndex < 0) currentSongIndex = songList.Count - 1; // ù �� �����̸� ������ ������ �̵�
        UpdateUI(); // UI ������Ʈ
        PlayCurrentSong(); // �� ���
    }

    // ���� �� ����
    private void SelectNextSong()
    {
        currentSongIndex++; // �ε����� ����
        if (currentSongIndex >= songList.Count) currentSongIndex = 0; // ������ �� ���ĸ� ù ������ �̵�
        UpdateUI(); // UI ������Ʈ
        PlayCurrentSong(); // �� ���
    }

    // ���õ� ���� ���� ������ �̵�
    private void LoadGameScene()
    {
        if (songList.Count == 0) return; // �� �����Ͱ� ������ �������� ����

        // ���� ���� �� �̸� ��������
        string sceneName = songList[currentSongIndex].sceneName;

        // �� �ε�
        SceneManager.LoadScene(sceneName);
    }
}
