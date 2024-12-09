using UnityEngine;

public class FourByFour : MonoBehaviour
{
    [SerializeField]
    private ONEShin_UINoteManager1 UINMg = null;
    [SerializeField]
    private AudioSource audioSource = null;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        UINMg = GetComponentInChildren<ONEShin_UINoteManager1>();
    }
    private void Start()
    {
        UINMg.Score0();
        UINMg.SetBpm(220); // 곡은 110BPM으로 측정됌
        UINMg.NotebyBarintlist(0, UINMg.zerobox);
        //UINMg.sampleNotesComming();

        //audioSource.Play();
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
            UINMg.PushNote(2);
        if (Input.GetKeyDown(KeyCode.S))
            UINMg.PushNote(3);
        if (Input.GetKeyDown(KeyCode.D))
            UINMg.PushNote(1);
        if (Input.GetKeyDown(KeyCode.F))
            UINMg.PushNote(0);
        // 디버그용 콤보 99로 만들기
        //if (Input.GetKeyDown(KeyCode.H))
        //UINMg.AddCombo99();

        if (Input.GetKeyDown(KeyCode.Z))
        {

            int[] pattern1box =
            {
            1,0,0,0,
            0,0,0,0,
            0,0,0,0,
            1,0,0,1
            };
            int[] pattern2box =
            {
            0,1,1,0,
            0,0,0,0,
            0,0,1,0,
            0,0,0,0
            };
            int[] pattern3box =
            {
            1,0,0,0,
            0,0,0,1,
            0,0,0,0,
            0,0,0,0
            };
            int[] pattern4box =
            {
            1,1,0,0,
            0,0,0,0,
            0,0,1,1,
            0,0,0,0
            };
            int[] pattern5box =
            {
            0,1,1,0,
            0,0,0,0,
            1,0,0,1,
            0,0,0,0
            };
            int[] pattern6box =
            {
            1,1,1,1,
            0,0,0,0,
            1,0,0,1,
            0,0,0,0
            };
            int[] pattern7box =
            {
            1,0,0,1,
            0,0,0,0,
            0,1,1,0,
            0,0,0,0
            };
            int[] pattern8box =
            {
            1,0,0,0,
            0,1,0,0,
            0,0,1,0,
            0,0,0,1
            };
            int[] pattern9box =
            {
            1,0,0,0,
            0,0,0,0,
            1,0,0,0,
            0,0,0,0
            };
            int[] pattern10box =
            {
            1,0,0,0,
            0,0,0,0,
            1,0,0,0,
            0,0,0,0
            };
            audioSource.Play();
            //for (int i = 0; i < 85;  i++)   
            //UINMg.NotebyBarintlist(i,pattern1box);
            //8마디 9번 반복
            for (int i = 0; i < 80; i += 8)
            {
                UINMg.NotebyBarintlist(i, pattern1box); 
                UINMg.NotebyBarintlist(i + 1, pattern2box);   
                UINMg.NotebyBarintlist(i + 2, pattern3box);   
                UINMg.NotebyBarintlist(i + 3, pattern4box);   
                UINMg.NotebyBarintlist(i + 4, pattern5box);   
                UINMg.NotebyBarintlist(i + 5, pattern6box);   
                UINMg.NotebyBarintlist(i + 6, pattern7box);   
                UINMg.NotebyBarintlist(i + 7, pattern8box);   
            }
            //마지막 5마디
            UINMg.NotebyBarintlist(80, pattern1box);
            UINMg.NotebyBarintlist(81, pattern2box);
            UINMg.NotebyBarintlist(82, pattern3box);
            UINMg.NotebyBarintlist(83, pattern7box);
            UINMg.NotebyBarintlist(84, pattern7box);
        }
        //푸쉬용 주석
        //if (Input.GetKeyDown(KeyCode.X))

    }
}
