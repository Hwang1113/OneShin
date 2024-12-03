using UnityEngine;

public class FourByFour : MonoBehaviour
{
    [SerializeField]
    private ONEShin_UINoteManager1 UINMg = null;



    public AudioSource audioSource = null;

    private void Awake()
    {
        UINMg = GetComponentInChildren<ONEShin_UINoteManager1>();

    }
    private void Start()
    {
        UINMg.Score0();

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
        int[] sampleQWER = { 1, 0, 1, 0 };
        float[] sampleBarNBeat = { 1f, 0f };
        UINMg.WhenPushNotes(sampleQWER, sampleBarNBeat);
        //Debug.Log("sampleNotesComming");
    }


}
