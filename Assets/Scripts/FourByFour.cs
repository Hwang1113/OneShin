using UnityEngine;
using Melanchall.DryWetMidi.Core;// MIDI ������ ���� ���� ���
using Melanchall.DryWetMidi.Interaction;




public class FourByFour : MonoBehaviour
{
    [SerializeField]
    private ONEShin_UINoteManager1 UINMg = null;
    //[SerializeField]
    //public MidiFile midiFile = null; // �̵������� �ҷ��� Ŭ���� test  //https://www.youtube.com/watch?v=YODaXBKT7gE
    //private MidiReader midiFileReader = null; //test
    //private float songDelayInSeconds = 0f;
    //private string fileLocation = "Assets/MidiFiles/Track_1.mid"; // �̵� ������ġ
    //private float noteTime = 0f;
    //private Note[] notes = new Note[5];
    //private float[] lastNoteValues = new float[128];
    //private float[] lanes = { 2,3,1,0 };


    public AudioSource audioSource = null;

    private void Awake()
    {
        UINMg = GetComponentInChildren<ONEShin_UINoteManager1>();
        //// MIDI ���� �ε�
        //midiFile = MidiFile.Read("Assets/MidiFiles/Track_1.mid");
        //ReadFromFile();
        //if (midiFile == null)
        //    Debug.Log("�̵� ���� ����");
        //if (midiFile != null)
        //    Debug.Log("�̵� ���� �ҷ���"); //�ϴ� ����

        //GetDataFromMidi();?
        //public TimedEvent GetTimedNoteOnEvent()?
    }
    private void Start()
    {
        UINMg.Score0();
        //// MIDI ���� �ε�
        //midiFile = MidiFile.Read("Assets/MidiFiles/Track_1.mid");
        //if (midiFile == null)
        //    Debug.Log("�̵� ���� ����");
        //if (midiFile != null)
        //    Debug.Log("�̵� ���� �ҷ���");
        sampleNotesComming();
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
        
        // ��Ʈ Ǫ��
        if (Input.GetKeyDown(KeyCode.U))
            UINMg.PushNote(2);
        if (Input.GetKeyDown(KeyCode.I))
            UINMg.PushNote(3);
        if (Input.GetKeyDown(KeyCode.O))
            UINMg.PushNote(1);
        if (Input.GetKeyDown(KeyCode.P))
            UINMg.PushNote(0);

        //if (Input.GetKeyDown(KeyCode.J))
        //    UINMg.;
        //if (Input.GetKeyDown(KeyCode.K))
        //    UINMg.;
        //if (Input.GetKeyDown(KeyCode.L))
        //    UINMg.;
        //if (Input.GetKeyDown(KeyCode.Semicolon))
        //    UINMg.;

    }

    private void sampleNotesComming()
    {
        int[] sampleQWER = { 1, 0, 1, 0 };
        float[] sampleBarNBeat = { 1f, 0f };
        UINMg.WhenPushNotes(sampleQWER, sampleBarNBeat);
        //Debug.Log("sampleNotesComming");
    }
    //private void ReadFromFile()
    //{
    //    //midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation); ���� �ڵ忡�� ��Ʈ��ũ�� ���ؼ� �ҷ��Դ�
    //    midiFile = MidiFile.Read("Assets/MidiFiles/Track_1.mid"); // ���� ��ġ ���� ȣ��
    //    GetDataFromMidi();
    //}
    //public void GetDataFromMidi()
    //{
    //    var notes = midiFile.GetNotes(); // ����� ���Ե� ��Ʈ�������� �Լ�
    //    var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count]; //��Ʈ�� ��� (��������)�� ��´�
    //    notes.CopyTo(array, 0); //���� �ڵ忡�� �ε����� �� 0���θ� ������?
    //    Debug.Log("array : " + array +"notes : " + notes);
    //    //foreach (var lane in lanes) lane.SetTimeStamps(array); // ���� �ڵ忡�� ���� ���� Ŭ������ ������༭ Ŭ���� ����Ʈ�� �����ؼ� �ڵ� ��ȯ�� �ʿ���
    //    //UINMg.SetTimeStamps(array);
    //    // ���� ���� ����� ���� Ŭ���� �ȿ��� index ��ȣ�� �ٸ��� �Ͽ� ����Ʈ�� �ҷ� ��Ʈ�� �����ϰ� �ִ�.

    //    Invoke(nameof(StartSong), songDelayInSeconds);
    //}

    //public void StartSong()
    //{
    //   // audioSource.Play();
    //}


}
