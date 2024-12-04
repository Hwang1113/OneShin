using System;
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
    //public float[] barNBeat = { 0, 0 }; // 몇마디 몇박에 나올지 형식 {bar,beat}
    //public float maxBeat = 4f;  //4분의 4박자면 4를 입력 , 4분의 3박자면 3을 입력
    //public float maxBar = 4f; // 최대마디
    //public int[] QWER = { 0, 0, 0, 0 }; // 어떤 노트가 동시에 나올지 형식

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
    // 노트를 생성하는 함수 (Q, W, E, R에 맞게)

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
            time += Time.deltaTime * Bpm * 0.005f;
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


    // 노트 판정 제거 함수 (퍼펙 점수 +50, 굿 +10 - 둘다 콤보카운트++, 배드는 점수 +0 - 콤보 0으로 초기화)
    public void HitNote(int _noteIndex)
    {
        // 다른 노트가 눌렸을 때 패널티 적용
        if (Stoptimings[_noteIndex] < 0.4f)
        {
            // 다른 노트가 눌렸으므로, 콤보 초기화 와 점수 차감
            ComboCnt = 0;
            Score -= 10; 
            Debug.Log("다른 노트 누름");
        }

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

    public void PushNote(int _noteIndex)
    {
        CreateMoveNoteToHit(_noteIndex);
    }

    //public void PushNotes(int[] _QWER) // QWER[] 형식으로 노트를 동시에 보냄
    //{
    //    if (_QWER[0] == 1)
    //    {
    //        PushNote(2);
    //        Debug.Log("Push Q");
    //    }

    //    if (_QWER[1] == 1)
    //    {
    //        PushNote(3);
    //        Debug.Log("Push W");
    //    }

    //    if (_QWER[2] == 1)
    //    {
    //        PushNote(1);
    //        Debug.Log("Push E");
    //    }

    //    if (_QWER[3] == 1)
    //    {
    //        PushNote(0);
    //        Debug.Log("Push R");
    //    }
    //}
    //public void WhenPushNotes(int[] _QWER, float[] _phraseNBarNBeat)
    //{
    //    StartCoroutine(WhenPushNotesCo(_QWER,_phraseNBarNBeat));
    //}
    //public IEnumerator WhenPushNotesCo(int[] _QWER, float[] _phraseNBarNBeat) // 동시에(또는 단일로) 푸쉬할 버튼을 고르고 몇마디 몇박자에 보낼지 정하는 함수
    //{
    //    float phrase = 0f;
    //    float bar = 0f;
    //    float beat = 0f;
    //    while (true)
    //    {
    //        //Debug.Log(bar + " "+ beat);
    //        beat += Time.deltaTime * Bpm * 0.01f;
    //        if (beat > maxBeat)
    //        {
    //            beat %= maxBeat;
    //            bar++;
    //            if (bar > maxBar)
    //                bar %= maxBar;
    //                phrase++;
    //            //Debug.Log(bar + " " + beat);
    //        }


    //        if (phrase >= _phraseNBarNBeat[0] && bar >= _phraseNBarNBeat[1] && beat >= _phraseNBarNBeat[2])
    //        {
    //            PushNotes(_QWER);
    //            //Debug.Log("노트 옴");
    //            yield break;
    //        }
    //        yield return null;
    //    }
    //}


    public void NotebyBarintlist(int _bar, int[] _boxlist)
    {
        StartCoroutine(Bil(_bar, _boxlist));
    }
    //노트 보내는 예
    //마디수, {구성};
    //1,{
    //   1,1,1,1,
    //   1,1,1,1,
    //   1,1,1,1,
    //   1,1,1,1
    //  };

    //2,{
    //   1,1,1,1,
    //   1,1,1,1,
    //   1,1,1,1,
    //   1,1,1,1
    //  };
    public IEnumerator Bil(int _bar, int[] _boxlist)
    {
        Debug.Log(_bar +"Start" + _boxlist);
        float time = 0;
        bool push1 = false;
        bool push2 = false;
        bool push3 = false;
        bool push4 = false;
        _bar = (_bar * 4) - 4;
        while (true)
        {
            time += Time.deltaTime * Bpm * 0.01f;
            if (time > 1)
            {
                time %= 1;
                _bar--;
            }
            if (_bar == 0 && !push1)
                { if (_boxlist[0] == 1) PushNote(2); if (_boxlist[1] == 1) PushNote(3); if (_boxlist[2] == 1) PushNote(1); if (_boxlist[3] == 1) PushNote(0); push1 = true; Debug.Log("push1:" + _bar); }            
            if (_bar == -1 && !push2)
                { if (_boxlist[4] == 1) PushNote(2); if (_boxlist[5] == 1) PushNote(3); if (_boxlist[6] == 1) PushNote(1); if (_boxlist[7] == 1) PushNote(0); push2 = true; Debug.Log("push2:" + _bar); }          
            if (_bar == -2 && !push3)
                { if (_boxlist[8] == 1) PushNote(2); if (_boxlist[9] == 1) PushNote(3); if (_boxlist[10] == 1) PushNote(1); if (_boxlist[11] == 1) PushNote(0); push3 = true; Debug.Log("push3:" + _bar); }          
            if (_bar == -3 && !push4)
                { if (_boxlist[12] == 1) PushNote(2); if (_boxlist[13] == 1) PushNote(3); if (_boxlist[14] == 1) PushNote(1); if (_boxlist[15] == 1) PushNote(0); push4 = true; Debug.Log("push4:" + _bar); }
            yield return null;
        }

    }

    public void PushLongNote(int _noteIndex)
    {
        Image noteGo = Instantiate(Noteboxes[_noteIndex], Hitboxes[_noteIndex].transform);
        NoteQueues[_noteIndex].Enqueue(noteGo);
        StartCoroutine(CreateLongNoteCo(noteGo, _noteIndex));
    }
    public IEnumerator CreateLongNoteCo(Image _Notebox, int _noteIndex)
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
            time += Time.deltaTime * Bpm * 0.005f;
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


    //점수 0으로 초기화
    public void Score0()
    {
        Score = 0;
    }
    public void SetBpm(float _BPM)
    {
        Bpm = _BPM;
    }
    //public void sampleNotesComming()
    //{
    //    int[] sampleQWERAll = { 1, 1, 1, 1 };
    //    int[] sampleQWER1 = { 1, 0, 0, 0 };
    //    int[] sampleQWER2 = { 0, 1, 0, 0 };
    //    int[] sampleQWER3 = { 0, 0, 1, 0 };
    //    int[] sampleQWER4 = { 0, 0, 0, 1 };
    //    float[] samplePBB = { 1f, 1f, 0f };
    //    float[] samplePBB1 = { 0f, 0f, 0f };
    //    float[] samplePBB6 = { 0f, 1f, 1f };
    //    float[] samplePBB7 = { 0f, 1f, 2f };
    //    float[] samplePBB8 = { 0f, 1f, 3f };
    //    float[] samplePBB9 = { 0f, 2f, 0f };

    //    WhenPushNotes(sampleQWERAll, samplePBB1);
    //    WhenPushNotes(sampleQWER4, samplePBB6);
    //    WhenPushNotes(sampleQWER3, samplePBB7);
    //    WhenPushNotes(sampleQWER2, samplePBB8);
    //    WhenPushNotes(sampleQWER1, samplePBB9);


    //    // 현재 버그 : 코루틴으로 만든 노트 중 동일한 버튼으로 히트 할 수 있는 노트가 2개 이상 있으면 히트를 못하는 버그가 생김
    //    // 
    //    Debug.Log("sampleNotesComming");
    //}

    public bool sampleNotesComming()
    {
        int onebar = 1;
        int twobar = 2;
        int[] onebox =
        {
            1,0,0,0,
            0,1,0,0,
            0,0,1,0,
            0,0,0,1
        };        
        int[] twobox =
        {
            0,0,0,1,
            0,0,1,0,
            0,1,0,0,
            1,0,0,0
        };
        int[] pattern1box =
        {
            0,0,0,1,
            0,0,1,1,
            1,0,0,0,
            1,1,0,0
        };        
        int[] pattern2box =
        {
            1,0,0,1,
            0,1,1,0,
            1,0,0,1,
            1,0,0,1
        };    
        int[] pattern3box =
        {
            1,0,0,1,
            0,0,0,0,
            0,1,1,0,
            1,0,0,1
        };    
        int[] pattern4box =
        {
            1,0,0,1,
            0,0,0,0,
            1,1,1,1,
            1,0,0,1
        };
        NotebyBarintlist(onebar, onebox);
        NotebyBarintlist(twobar, twobox);
        NotebyBarintlist(3, pattern1box);
        NotebyBarintlist(4, pattern2box);
        NotebyBarintlist(5, pattern3box);
        NotebyBarintlist(6, pattern2box);
        NotebyBarintlist(7, pattern3box);
        NotebyBarintlist(8, pattern4box);

        Debug.Log("sampleNotesComming");
        return true;
    }

    #endregion
}
