using UnityEngine;

// 곡 데이터를 저장하는 클래스
[System.Serializable]
public class SongData
{
    public string songTitle; // 곡 제목
    public AudioClip songClip; // 곡 오디오 파일
    public Sprite thumbnailImage; // 썸네일 이미지
    public string sceneName; // 곡에 연결된 씬 이름
}

