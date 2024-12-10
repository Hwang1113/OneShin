using UnityEngine;

public class FourByFour : MonoBehaviour
{

    [SerializeField]
    private ONEShin_UINoteManager1 UINMg = null;
    [SerializeField]
    //private AudioClip bgm = null;
    public AudioSource audioSource = null;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>(); //12.10 수정 
        UINMg = GetComponentInChildren<ONEShin_UINoteManager1>();
    }
    private void Start()
    {
        UINMg.Score0();
        UINMg.SetBpm(220); //220 BPM 으로설정 // 110 이 맞으나 정박노트만 만들수 밖에 없는 문제가 생겨 (반박자노트 생성 불가) 임시방편으로 220으로 맞춤
        UINMg.sampleNotesComming(audioSource);
        //audioSource.clip = bgm;
        //audioSource.Play();
    }

    private void Update()
    {
        // 노트 히트
        if (Input.GetKeyDown(KeyCode.S)) //GetKey로 하면 절대 안됌!!!!!  GetKeyDown!!!!!! 중요함!!!!!!
            UINMg.HitNote(2);
        if (Input.GetKeyDown(KeyCode.W))
            UINMg.HitNote(3);
        if (Input.GetKeyDown(KeyCode.I))
            UINMg.HitNote(1);
        if (Input.GetKeyDown(KeyCode.K))
            UINMg.HitNote(0);

        if (Input.GetKeyDown(KeyCode.A))
            UINMg.CheatLifeNtime();
        

            // 디버그용 노트 푸시
        if (Input.GetKeyDown(KeyCode.Z))
            UINMg.HitNote(2);
        if (Input.GetKeyDown(KeyCode.X))
            UINMg.HitNote(3);
        if (Input.GetKeyDown(KeyCode.C))
            UINMg.HitNote(1);
        if (Input.GetKeyDown(KeyCode.V))
            UINMg.HitNote(0);
        
        if(UINMg.isGameover) // isGameover == true 면 오디오를 멈춤 12.10
            audioSource.Stop();


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
