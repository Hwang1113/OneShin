using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ONEShin_UINoteManager1 : MonoBehaviour
{
    //버튼 델리게이트 선언 
    public delegate void BtnDelegate();
    public BtnDelegate onClickBtn = null;

    #region private 변수
    private List<Image> Hitboxes = new List<Image>(); // Q, W, E, R에 대한 히트박스를 리스트로 관리
    private List<Image> Noteboxes = new List<Image>(); // Q, W, E, R에 대한 노트박스를 리스트로 관리
    private float[] Stoptimings = new float[4]; // 스탑 타이밍 배열
    private List<Vector2> NoteboxStartPoints = new List<Vector2>(); // 각 노트 타입의 시작 위치를 리스트로 관리
    private List<Vector2> NoteBoxEndPoints = new List<Vector2>(); // 각 노트 타입의 끝 위치를 리스트로 관리
    private List<Queue<Image>> NoteQueues = new List<Queue<Image>>(); // 각 노트 타입의 큐를 리스트로 관리
    [SerializeField]
    private Image Good = null; // Good
    #endregion
    #region public 변수
    public int ComboCnt = 0; //콤보 카운트
    public int Score = 0; // 점수 
    public float Bpm = 60; // 음악 템포 또는 BPM 형식
    public int lifeCnt = 5; 
    public int totalNoteCnt = 0;
    public bool isFever = false;
    public const int EnterFeverCnt = 100;
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
    private IEnumerator CreateMoveNoteToHitCoroutine(Image _Notebox, int _noteIndex)
    {
        _Notebox.gameObject.SetActive(true);
        //Stoptimings[_noteIndex] = 0f;// 여기서 겹치는 문제가 생긴듯? 원인이 이전 2개이상 생겼을때 독립적으로 생성되는게 아닌듯함
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
            if (time > Stoptimings[_noteIndex])
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
    }

    #endregion

    #region public

    #region 노트 판정 후 제거
    // 노트 판정 제거 함수 (퍼펙 점수 +50, 굿 +10 - 둘다 ComboCnt++;, 배드는 콤보 0으로 초기화 lifeCnt--;)
    public void HitNote(int _noteIndex)
    {
        if (isFever)
        { 
            feverTimeHit();
        }

        StartCoroutine(HitNoteCo(_noteIndex));
    }

    public IEnumerator HitNoteCo(int _noteIndex)
    {
        // 다른 노트가 눌렸을 때 패널티 적용
        if (Stoptimings[_noteIndex] < 0.4f)
        {
            if (!isFever) // 피버타임이 아니면 패널티 콤보 카운트 0으로 초기화
                ComboCnt = 0;   // 다른 노트가 눌렸으므로, 콤보 초기화
            //피버 타임이면 패널티 미적용  
        }

        if (Stoptimings[_noteIndex] >= 0.4f)
        {
            if (NoteQueues[_noteIndex].Count > 0 && NoteQueues[_noteIndex] != null) //노트 파괴
            {
                Image go = null;
                if (NoteQueues[_noteIndex].TryDequeue(out go))
                    Destroy(go.gameObject);
            }
            if (NoteQueues[_noteIndex].Count >= 0 && !isFever) // 판정 점수 계산, 피버타임이면 점수 계산 안함
            {
                if (0.4 <= Stoptimings[_noteIndex] && Stoptimings[_noteIndex] < 0.6) //bad
                {
                    ComboCnt = 0;
                    lifeCnt--;
                }

                else if (0.6 <= Stoptimings[_noteIndex] && Stoptimings[_noteIndex] < 0.75) //good
                {
                    ComboCnt++;
                    Score += 10;
                    if (ComboCnt != 0 && 0 == ComboCnt % EnterFeverCnt)
                        StartCoroutine(StartFeverTime());

                }
                else if (0.75 <= Stoptimings[_noteIndex] && Stoptimings[_noteIndex] < 0.85) //perfect
                {
                    ComboCnt++;
                    Score += 50;
                    if (ComboCnt != 0 && 0 == ComboCnt % EnterFeverCnt)
                        StartCoroutine(StartFeverTime());
                }
                else if (0.85 <= Stoptimings[_noteIndex] && Stoptimings[_noteIndex] < 1)//good
                {
                    ComboCnt++;
                    Score += 10;
                    if (ComboCnt != 0 && 0 == ComboCnt % EnterFeverCnt)
                        StartCoroutine(StartFeverTime());
                }
            }

        }
        yield return null;
    }
    #endregion
    #region 노트생성
    public void PushNote(int _noteIndex)
    {
        //CreateMoveNoteToHit(_noteIndex);
        totalNoteCnt++;
        Image noteGo = Instantiate(Noteboxes[_noteIndex], Hitboxes[_noteIndex].transform);
        NoteQueues[_noteIndex].Enqueue(noteGo);
        StartCoroutine(CreateMoveNoteToHitCoroutine(noteGo, _noteIndex));
    } // 노트(q2,w3,e0,r1) 중 하나 보내기

    public void NotebyBarintlist(int _bar, int[] _boxlist)
    {
        StartCoroutine(NoteBil(_bar, _boxlist));
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
    public IEnumerator NoteBil(int _bar, int[] _boxlist)
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
                { if (_boxlist[0] == 1) PushNote(2); if (_boxlist[1] == 1) PushNote(3); if (_boxlist[2] == 1) PushNote(1); if (_boxlist[3] == 1) PushNote(0); push1 = true; }            
            if (_bar == -1 && !push2)
                { if (_boxlist[4] == 1) PushNote(2); if (_boxlist[5] == 1) PushNote(3); if (_boxlist[6] == 1) PushNote(1); if (_boxlist[7] == 1) PushNote(0); push2 = true; }          
            if (_bar == -2 && !push3)
                { if (_boxlist[8] == 1) PushNote(2); if (_boxlist[9] == 1) PushNote(3); if (_boxlist[10] == 1) PushNote(1); if (_boxlist[11] == 1) PushNote(0); push3 = true; }          
            if (_bar == -3 && !push4)
                { if (_boxlist[12] == 1) PushNote(2); if (_boxlist[13] == 1) PushNote(3); if (_boxlist[14] == 1) PushNote(1); if (_boxlist[15] == 1) PushNote(0); push4 = true; }
            yield return null;
        }

    } // 노트 여러개 보내기
    #endregion
    #region 퍼블릭 변수 제어
    public void Score0()
    {
        Score = 0;
    } // 스코어 0으로 초기화
    public void SetBpm(float _BPM)
    {
        Bpm = _BPM;
    } // BPM 설정
    public void SetLife5()
    {
        lifeCnt = 5;
    } // 라이프 카운트 5로 초기화

    public string Rank() // 보낸노트 종합 후 점수 비율에 따라 랭크 산정 
    {
        if(50 * totalNoteCnt == Score)
            return ("SSS Rank");        
        if(50 * totalNoteCnt > Score && 30 * Score >= totalNoteCnt)
            return ("S Rank");         
        if(30 * totalNoteCnt > Score && 15 * Score >= totalNoteCnt)
            return ("A Rank");         
        if(15 * totalNoteCnt > Score && 10 * Score >= totalNoteCnt)
            return ("B Rank");               
        if(totalNoteCnt >= 10 * Score)
            return ("F Rank");        


        // SSS, S ,A ,B
        else
            return ("오류 랭크정산 안됌");
    }

    public void AddCombo99() // 콤보에 99 더하기
    {
        ComboCnt += 99;
    }

    #endregion
    #region 피버타임
    public IEnumerator StartFeverTime()
    {
        float feverTimeCnt = 0;
        isFever = true;
        while (feverTimeCnt < 3)
        {
            feverTimeCnt += Time.deltaTime;
            Debug.Log("피버타임!");
            yield return null;
        }
        if (feverTimeCnt >= 3)
        {
            Debug.Log("피버타임끝~");
            isFever = false;
        }
    }

    public void feverTimeHit() // 피버타임히트 함수 실행하면 1점 추가
    {
            Score += 1;
        Debug.Log(Score);
    }
    #endregion

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
            0,0,0,0,
            1,1,0,0
        };        
        int[] pattern2box =
        {
            1,0,0,1,
            0,1,1,0,
            0,0,0,0,
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
        int[] pattern5box =
        {
            1,0,0,1,
            0,0,0,0,
            1,0,1,0,
            0,0,0,0
        };
        int[] pattern6box =
        {
            1,0,0,0,
            1,0,0,0,
            1,0,0,0,
            1,0,0,0
        };
        NotebyBarintlist(onebar, pattern6box);
        NotebyBarintlist(twobar, pattern6box);
        NotebyBarintlist(3, pattern6box);
        NotebyBarintlist(4, pattern6box);
        NotebyBarintlist(5, pattern6box);
        NotebyBarintlist(6, pattern6box);
        NotebyBarintlist(7, pattern6box);
        NotebyBarintlist(8, pattern6box);
        NotebyBarintlist(9, pattern6box);
        NotebyBarintlist(10, pattern5box);
        NotebyBarintlist(11, pattern5box);
        NotebyBarintlist(12, pattern5box);
        NotebyBarintlist(13, pattern5box);
        NotebyBarintlist(14, pattern2box);
        NotebyBarintlist(15, pattern4box);
        NotebyBarintlist(16, pattern2box);
        NotebyBarintlist(17, pattern3box);

        Debug.Log("sampleNotesComming");
        return true;
    } //테스트 용 노트 여러개 보내기

    #region 퍼블릭 변수 확인
    public bool IsFeverTime
    {
        get { return isFever; }  // 값을 반환
    }
    #endregion
    #endregion
}
