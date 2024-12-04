using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FourByFour : MonoBehaviour
{
    [SerializeField]
    private ONEShin_UINoteManager1 UINMg = null;
    [SerializeField]
    private AudioClip bgm = null;
    private AudioSource audioSource = null;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        UINMg = GetComponentInChildren<ONEShin_UINoteManager1>();
    }
    private void Start()
    {
        UINMg.Score0();
        UINMg.SetBpm(240); //240 BPM 으로설정
        UINMg.sampleNotesComming();
        audioSource.clip = bgm;
        audioSource.Play();
    }

    private void Update()
    {
        // 노트 히트
        if (Input.GetKeyDown(KeyCode.Q))
            UINMg.HitNote(2);
        if (Input.GetKeyDown(KeyCode.W))
            UINMg.HitNote(3);
        if (Input.GetKeyDown(KeyCode.E))
            UINMg.HitNote(1);
        if (Input.GetKeyDown(KeyCode.R))
            UINMg.HitNote(0);

        // 디버그용 노트 푸시
        if (Input.GetKeyDown(KeyCode.A))
            UINMg.HitNote(2);
        if (Input.GetKeyDown(KeyCode.S))
            UINMg.HitNote(3);
        if (Input.GetKeyDown(KeyCode.D))
            UINMg.HitNote(1);
        if (Input.GetKeyDown(KeyCode.F))
            UINMg.HitNote(0);

        //// 디버그용 롱노트 푸시
        //if (Input.GetKeyDown(KeyCode.U))
        //    UINMg.PushLongNote(2);
        //if (Input.GetKeyDown(KeyCode.I))
        //    UINMg.PushLongNote(3);
        //if (Input.GetKeyDown(KeyCode.O))
        //    UINMg.PushLongNote(1);
        //if (Input.GetKeyDown(KeyCode.P))
        //    UINMg.PushLongNote(0);
    }
}
