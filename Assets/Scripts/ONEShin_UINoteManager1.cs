using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ONEShin_UINoteManager1 : MonoBehaviour
{
    #region private 변수
    private List<Image> Hitboxes = new List<Image>(); // Q, W, E, R에 대한 히트박스를 리스트로 관리
    private List<Image> Noteboxes = new List<Image>(); // Q, W, E, R에 대한 노트박스를 리스트로 관리
    private float[] Stoptimings = new float[4]; // 스탑 타이밍 배열
    private List<Vector2> NoteboxStartPoints = new List<Vector2>(); // 각 노트 타입의 시작 위치를 리스트로 관리
    private List<Vector2> NoteBoxEndPoints = new List<Vector2>(); // 각 노트 타입의 끝 위치를 리스트로 관리
    private List<Queue<Image>> NoteQueues = new List<Queue<Image>>(); // 각 노트 타입의 큐를 리스트로 관리
    #endregion
    #region public 변수
    public int ComboCnt = 0; //콤보 카운트
    public int Score = 0; // 점수 
    public float Bpm = 60; // 음악 템포 또는 BPM 형식
    public float[] barNBeat = { 0, 0 }; // 몇마디 몇박에 나올지 형식 {bar,beat}
    public float maxBeat = 4f;  //4분의 4박자면 4를 입력 , 4분의 3박자면 3을 입력
    public int[] QWER = { 0, 0, 0, 0 }; // 어떤 노트가 동시에 나올지 형식


    #endregion
    private void Awake()
    {
      
        // Q, W, E, R에 대한 Hitbox와 Notebox 초기화
        for (int i = 0; i < 4; i++)
        {
            Hitboxes.Add(transform.GetChild(i).GetComponent<Image>());
            Noteboxes.Add(Hitboxes[i].GetComponentsInChildren<Image>()[1]);
            NoteQueues.Add(new Queue<Image>());
        }

        // 각 노트의 시작 위치와 끝 위치 설정
        for (int i = 0; i < 4; i++)
        {
            NoteboxStartPoints.Add(Noteboxes[i].rectTransform.anchoredPosition);
            NoteBoxEndPoints.Add(Hitboxes[i].rectTransform.anchoredPosition + new Vector2(-Hitboxes[i].rectTransform.sizeDelta.x, Hitboxes[i].rectTransform.sizeDelta.y));
        }
    }

    #region private
    // 노트를 생성하고 이동시키는 함수
    private void CreateMoveNoteToHit(int _noteIndex)
    {
        Image noteGo = Instantiate(Noteboxes[_noteIndex], Hitboxes[_noteIndex].transform);
        NoteQueues[_noteIndex].Enqueue(noteGo);
        StartCoroutine(CreateMoveNoteToHitCoroutine(noteGo, _noteIndex));
    }
    // 노트를 히트박스까지 이동시키는 코루틴
    private IEnumerator CreateMoveNoteToHitCoroutine(Image _Notebox, int _noteIndex)
    {
        _Notebox.gameObject.SetActive(true);
        Stoptimings[_noteIndex] = 0f;
        float time = 0f;
        while (time < 1f && _Notebox != null)
        {
            if (_Notebox != null)
            {
                _Notebox.rectTransform.anchoredPosition = Vector3.Lerp(Noteboxes[_noteIndex].rectTransform.anchoredPosition, Hitboxes[_noteIndex].rectTransform.anchoredPosition, time * 10 / 8f);
                _Notebox.rectTransform.sizeDelta = Vector3.Lerp(Noteboxes[_noteIndex].rectTransform.sizeDelta, Vector2.zero, 5 * time - 4);
            }
            if (time > 1f)
            {
                time = 1f;
            }
            Stoptimings[_noteIndex] = time;
            time += Time.deltaTime * Bpm * 0.01f;
            yield return null;
        }
        if (_Notebox != null)
        {
            Stoptimings[_noteIndex] = 0.4f;
            _Notebox.rectTransform.anchoredPosition = NoteBoxEndPoints[_noteIndex];
            HitNote(_noteIndex);
        }
        Stoptimings[_noteIndex] = 0f;
    }

    #endregion

    #region public
    // 노트 판정 함수 (퍼펙 점수 +50, 굿 +10 - 둘다 콤보카운트++, 배드는 점수 +0 - 콤보 0으로 초기화)
    public void HitNote(int _noteIndex)
    {
        if (Stoptimings[_noteIndex] >= 0.4f)
        {
            if (NoteQueues[_noteIndex].Count > 0 && NoteQueues[_noteIndex] != null)
            {
                Image go = null;
                if (NoteQueues[_noteIndex].TryDequeue(out go))
                    Destroy(go.gameObject);
            }

            if (NoteQueues[_noteIndex].Count >= 0)
            {
                if (0.4 <= Stoptimings[_noteIndex] && Stoptimings[_noteIndex] < 0.6)
                {
                    Debug.Log(Stoptimings[_noteIndex] + " Bad");
                    ComboCnt = 0;
                }

                else if (0.6 <= Stoptimings[_noteIndex] && Stoptimings[_noteIndex] < 0.75)
                {
                    ComboCnt++;
                    Score += 10;
                    Debug.Log(Stoptimings[_noteIndex] + " Good Combo: " + ComboCnt + "Score : " + Score);
                }
                else if (0.75 <= Stoptimings[_noteIndex] && Stoptimings[_noteIndex] < 0.85)
                {
                    ComboCnt++;
                    Score += 50;
                    Debug.Log(Stoptimings[_noteIndex] + " Perfect Combo: " + ComboCnt + "Score : " + Score);
                }
                else if (0.85 <= Stoptimings[_noteIndex] && Stoptimings[_noteIndex] < 1)
                {  
                    ComboCnt++;
                    Score += 10;
                    Debug.Log(Stoptimings[_noteIndex] + " Good! Combo: " + ComboCnt + "Score : " + Score);
                }
            }
        }
    }

    // 노트를 생성하는 함수 (Q, W, E, R에 맞게)
    public void PushNote(int _noteIndex)
    {
        CreateMoveNoteToHit(_noteIndex);
    }

    public void PushNotes(int[] _QWER) // QWER[] 형식으로 노트를 동시에 보냄
    {
        if (_QWER[0] == 1)
        {
            PushNote(2);
            Debug.Log("Push Q");
        }

        if (_QWER[1] == 1)
        {
            PushNote(3);
            Debug.Log("Push W");
        }

        if (_QWER[2] == 1)
        {
            PushNote(1);
            Debug.Log("Push E");
        }

        if (_QWER[3] == 1)
        {
            PushNote(0);
            Debug.Log("Push R");
        }
    }
    public void WhenPushNotes(int[] _QWER, float[] _barNBeat)
    {
        StartCoroutine(WhenPushNotesCo(_QWER,_barNBeat));
    }
    public IEnumerator WhenPushNotesCo(int[] _QWER, float[] _barNBeat) // 동시에(또는 단일로) 푸쉬할 버튼을 고르고 몇마디 몇박자에 보낼지 정하는 함수
    {
        float bar = 0f;
        float beat = 0f;
        while (true)
        {
            //Debug.Log(bar + " "+ beat);
            beat += Time.deltaTime * Bpm * 0.01f;
            if (beat > 4)
            {
                beat %= 4;
                bar++;
                Debug.Log(bar + " " + beat);
            }

            if (bar >= _barNBeat[0] && beat >= _barNBeat[1])
            {
                PushNotes(_QWER);
                Debug.Log("노트 옴");
                yield break;
            }
            yield return null;
        }
    }



    //점수 0으로 초기화
    public void Score0()
    {
        Score = 0;
    }
    public void SetBpm(float _BPM)
    {
        Bpm = _BPM;
    }
    #endregion
}
