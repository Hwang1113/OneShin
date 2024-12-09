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
        UINMg.SetBpm(110); // ���� 110BPM���� ������
        //UINMg.sampleNotesComming();

        //audioSource.Play();
    }

    private void Update()
    {
        // ��Ʈ ��Ʈ
        if (Input.GetKeyDown(KeyCode.Q))
            UINMg.HitNote(2);
        if (Input.GetKeyDown(KeyCode.W))
            UINMg.HitNote(3);
        if (Input.GetKeyDown(KeyCode.E))
            UINMg.HitNote(1);
        if (Input.GetKeyDown(KeyCode.R))
            UINMg.HitNote(0);

        // ����׿� ��Ʈ Ǫ��
        if (Input.GetKeyDown(KeyCode.A))
            UINMg.PushNote(2);
        if (Input.GetKeyDown(KeyCode.S))
            UINMg.PushNote(3);
        if (Input.GetKeyDown(KeyCode.D))
            UINMg.PushNote(1);
        if (Input.GetKeyDown(KeyCode.F))
            UINMg.PushNote(0);
        // ����׿� �޺� 99�� �����
        //if (Input.GetKeyDown(KeyCode.H))
        //UINMg.AddCombo99();

        if (Input.GetKeyDown(KeyCode.Z))
            UINMg.Stage1Note(audioSource);
    }
}
