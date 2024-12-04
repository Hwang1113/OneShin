using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FourByFour : MonoBehaviour
{
    [SerializeField]
    private ONEShin_UINoteManager1 UINMg = null;
    private void Awake()
    {
        UINMg = GetComponentInChildren<ONEShin_UINoteManager1>();
    }
    private void Start()
    {
        UINMg.Score0();
        UINMg.SetBpm(120); //120 BPM 설정
        sampleNotesComming();

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

        // 노트 푸시
        if (Input.GetKeyDown(KeyCode.U))
            UINMg.PushNote(2);
        if (Input.GetKeyDown(KeyCode.I))
            UINMg.PushNote(3);
        if (Input.GetKeyDown(KeyCode.O))
            UINMg.PushNote(1);
        if (Input.GetKeyDown(KeyCode.P))
            UINMg.PushNote(0);

    }

    private void sampleNotesComming()
    {
        int[] sampleQWERAll = { 1, 1, 1, 1 };
        int[] sampleQWER1 = { 1, 0, 0, 0 };
        int[] sampleQWER2 = { 0, 1, 0, 0 };
        int[] sampleQWER3 = { 0, 0, 1, 0 };
        int[] sampleQWER4 = { 0, 0, 0, 1 };
        float[] sampleBarNBeat = { 1f, 0f };
        float[] sampleBarNBeat1 = { 0f, 0f };
        float[] sampleBarNBeat2 = { 0f, 1f };
        float[] sampleBarNBeat3 = { 0f, 2f };
        float[] sampleBarNBeat4 = { 0f, 3f };
        //UINMg.WhenPushNotes(sampleQWER, sampleBarNBeat);
        UINMg.WhenPushNotes(sampleQWER1, sampleBarNBeat1);
        UINMg.WhenPushNotes(sampleQWER2, sampleBarNBeat2);
        UINMg.WhenPushNotes(sampleQWER3, sampleBarNBeat3);
        UINMg.WhenPushNotes(sampleQWER4, sampleBarNBeat4);
        Debug.Log("sampleNotesComming");
    }
}
