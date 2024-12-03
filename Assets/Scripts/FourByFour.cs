using UnityEngine;
using Melanchall.DryWetMidi.Core;// MIDI 파일을 쓰기 위한 헤더
using Melanchall.DryWetMidi.Interaction;




public class FourByFour : MonoBehaviour
{
    [SerializeField]
    private ONEShin_UINoteManager1 UINMg = null;
    //[SerializeField]
    //public MidiFile midiFile = null; // 미디파일을 불러올 클래스 test  //https://www.youtube.com/watch?v=YODaXBKT7gE
    //private MidiReader midiFileReader = null; //test
    //private float songDelayInSeconds = 0f;
    //private string fileLocation = "Assets/MidiFiles/Track_1.mid"; // 미디 파일위치
    //private float noteTime = 0f;
    //private Note[] notes = new Note[5];
    //private float[] lastNoteValues = new float[128];
    //private float[] lanes = { 2,3,1,0 };


    public AudioSource audioSource = null;

    private void Awake()
    {
        UINMg = GetComponentInChildren<ONEShin_UINoteManager1>();
        //// MIDI 파일 로드
        //midiFile = MidiFile.Read("Assets/MidiFiles/Track_1.mid");
        //ReadFromFile();
        //if (midiFile == null)
        //    Debug.Log("미디 파일 없음");
        //if (midiFile != null)
        //    Debug.Log("미디 파일 불러옴"); //일단 성공

        //GetDataFromMidi();?
        //public TimedEvent GetTimedNoteOnEvent()?
    }
    private void Start()
    {
        UINMg.Score0();
        //// MIDI 파일 로드
        //midiFile = MidiFile.Read("Assets/MidiFiles/Track_1.mid");
        //if (midiFile == null)
        //    Debug.Log("미디 파일 없음");
        //if (midiFile != null)
        //    Debug.Log("미디 파일 불러옴");
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
    //    //midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation); 참고 코드에선 네트워크를 통해서 불러왔다
    //    midiFile = MidiFile.Read("Assets/MidiFiles/Track_1.mid"); // 파일 위치 지정 호출
    //    GetDataFromMidi();
    //}
    //public void GetDataFromMidi()
    //{
    //    var notes = midiFile.GetNotes(); // 헤더에 포함된 노트가져오는 함수
    //    var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count]; //노트를 세어서 (지역변수)에 담는다
    //    notes.CopyTo(array, 0); //참조 코드에서 인덱스를 왜 0으로만 했을까?
    //    Debug.Log("array : " + array +"notes : " + notes);
    //    //foreach (var lane in lanes) lane.SetTimeStamps(array); // 참고 코드에선 레인 마다 클래스를 만들어줘서 클래스 리스트로 관리해서 코드 변환이 필요함
    //    //UINMg.SetTimeStamps(array);
    //    // 현재 관리 방식은 같은 클래스 안에서 index 번호를 다르게 하여 리스트를 불러 노트를 관리하고 있다.

    //    Invoke(nameof(StartSong), songDelayInSeconds);
    //}

    //public void StartSong()
    //{
    //   // audioSource.Play();
    //}


}
