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
        audioSource = GetComponentInChildren<AudioSource>(); //12.10 ���� 
        UINMg = GetComponentInChildren<ONEShin_UINoteManager1>();
    }
    private void Start()
    {
        UINMg.Score0();
        UINMg.SetBpm(220); //220 BPM ���μ��� // 110 �� ������ ���ڳ�Ʈ�� ����� �ۿ� ���� ������ ���� (�ݹ��ڳ�Ʈ ���� �Ұ�) �ӽù������� 220���� ����
        UINMg.sampleNotesComming(audioSource);
        //audioSource.clip = bgm;
        //audioSource.Play();
    }

    private void Update()
    {
        // ��Ʈ ��Ʈ
        if (Input.GetKeyDown(KeyCode.S)) //GetKey�� �ϸ� ���� �ȉ�!!!!!  GetKeyDown!!!!!! �߿���!!!!!!
            UINMg.HitNote(2);
        if (Input.GetKeyDown(KeyCode.W))
            UINMg.HitNote(3);
        if (Input.GetKeyDown(KeyCode.I))
            UINMg.HitNote(1);
        if (Input.GetKeyDown(KeyCode.K))
            UINMg.HitNote(0);

        if (Input.GetKeyDown(KeyCode.A))
            UINMg.CheatLifeNtime();
        

            // ����׿� ��Ʈ Ǫ��
        if (Input.GetKeyDown(KeyCode.Z))
            UINMg.HitNote(2);
        if (Input.GetKeyDown(KeyCode.X))
            UINMg.HitNote(3);
        if (Input.GetKeyDown(KeyCode.C))
            UINMg.HitNote(1);
        if (Input.GetKeyDown(KeyCode.V))
            UINMg.HitNote(0);
        
        if(UINMg.isGameover) // isGameover == true �� ������� ���� 12.10
            audioSource.Stop();


        //// ����׿� �ճ�Ʈ Ǫ��
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
