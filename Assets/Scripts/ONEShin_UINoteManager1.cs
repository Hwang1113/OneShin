using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

    //추가
    private int score = 0; // 추가
    private bool isGameOver = false; // 추가2
    private int scoreToAdd; // 추가2 점수 변수를 클래스 필드로 선언
    private float clearTimer; // 추가 58초 후 클리어 UI를 띄우기 위한 타이머
    private bool isClearTimerStarted = false; // 추가 타이머 시작 여부
    private bool isClearTriggered = false; // 추가 클리어 UI 표시 여부
    private bool isCheat=false;
    private float feverTime = 0f;
    #endregion
    #region public 변수
    public int ComboCnt = 0; //콤보 카운트
    public int Score = 0; // 점수 
    public float Bpm = 60; // 음악 템포 또는 BPM 형식
    public bool isGameover = false; // FourbyFour가 오디오 관리중이고 현재 스크립트 부모에게 알릴 bool 변수 선언 12.10
    private bool isFever = false; // FeverMode 상태 Bool 값을 저장할 변수 12.10
    private int NoteCnt = 0; // 노트가 몇개 푸쉬됐는지 알려주는 변수 Push 할때마다 ++ 할 예정
    #endregion

    // 추가
    public JudgeUIManager judgeUIManager; // 추가2
    public ScoreManager scoreManager; // 추가2
    public Stage1UIManager stage1UIManager; // 추가


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

    private void Update()
    {
        // 추가2 LifeManager에서 게임 오버 상태 확인
        if (LifeManager.Instance != null && LifeManager.Instance.IsGameOver)
        {
            if(!isGameOver)
            {
                isGameOver = true;
                isGameover = true; // 현재 스크립트 부모 FourbyFour가 오디오 관리중이고 퍼블릭 변수로 게임 오버 돼었다고 알림 12.10 
                StopAllNotes();
            }
        }
        //추가
        // 타이머가 시작되지 않았다면 시작
        if (!isClearTimerStarted)
        {
            StartClearTimer();
        }
        // 추가 타이머 체크
        if (isClearTimerStarted && !isClearTriggered)
        {
            clearTimer -= Time.deltaTime;
            if (clearTimer <= 0f)
            {
                isFever = true;
                TriggerClearUI();
                StopAllNotes();
            }
        }

        if(isFever && feverTime<5)
        {
            feverTime += Time.deltaTime;
        }
    }


    // 추가2
    private void StopAllNotes()
    {
        // 노트 생성 및 이동 중지
        CancelInvoke();

        // 기존에 생성된 노트 제거
        foreach ( var queue in NoteQueues)
        {
            while (queue.Count > 0)
            {
                Image note = queue.Dequeue(); // note 변수를 선언
                Destroy(note.gameObject);
            }
        }

        foreach (var hitbox in Hitboxes)
        {
            if (hitbox != null)
            {
                hitbox.gameObject.SetActive(false); // 히트박스 비활성화
            }
        }

        // 게임 오버 시 JudgeUIManager의 OnGameOver 호출
        JudgeUIManager.Instance?.OnGameOver();
    }


    // 추가 타이머
    private void StartClearTimer()
    {
        isClearTimerStarted = true;
        clearTimer = 90f; // 58초 타이머 설정
    }

    // 추가 클리어 표시
    private void TriggerClearUI()
    {
        if (LifeManager.Instance != null && LifeManager.Instance.IsGameOver)
        {
            return;
        }

        isClearTriggered = true;

        // Stage1UIManager를 통해 클리어 UI 표시
        if (stage1UIManager != null)
        {
            stage1UIManager.ShowClearUI();

            // 클리어 시 JudgeUIManager의 OnGameClear 호출
            JudgeUIManager.Instance?.OnGameClear();

        }
        else
        {
            Debug.LogError("Stage1UIManager 연결필요");
        }
    }

    #region private
    // 노트를 생성하는 함수 (Q, W, E, R에 맞게)

    // 노트를 생성하고 이동시키는 함수
    private void CreateMoveNoteToHit(int _noteIndex)
    {
        if (isGameOver) return; // 추가2 게임오버 상태에서는 노트 생성 금지

        Image noteGo = Instantiate(Noteboxes[_noteIndex], Hitboxes[_noteIndex].transform);
        NoteQueues[_noteIndex].Enqueue(noteGo);
        StartCoroutine(CreateMoveNoteToHitCoroutine(noteGo, _noteIndex));
    }
    // 노트를 히트박스까지 이동시키는 코루틴
    private IEnumerator CreateMoveNoteToHitCoroutine(Image _Notebox, int _noteIndex)
    {
        _Notebox.gameObject.SetActive(true);

        //Stoptimings[_noteIndex] = 0f; 새로 생성된 노트가의 time은 0이라서 Stoptimings[_noteIndex] 0으로 초기화 되어야 하는게 아니고 (0인 진짜 잠깐의 순간에 hit 하면 작동 안할 것이다.) 12.10
        float time = 0f;
        if (0.4 < Stoptimings[_noteIndex])  // 노트가 생성됐을때 판정 범위안에 노트가 있으면 스탑 타이밍을 초기화 하지 않는게 맞고 0.4미만은 0으로 초기화 하여도 while문을 도는 동안 스탑 타이밍이 최신화 된다 12.10
            Stoptimings[_noteIndex] = 0f;    
        while (time < 1f && _Notebox != null)
        {
            if (_Notebox != null)
            {
                _Notebox.rectTransform.anchoredPosition = Vector3.Lerp(Noteboxes[_noteIndex].rectTransform.anchoredPosition, Hitboxes[_noteIndex].rectTransform.anchoredPosition, time * 10 / 8f);
                _Notebox.rectTransform.sizeDelta = Vector3.Lerp(Noteboxes[_noteIndex].rectTransform.sizeDelta, Vector2.zero, 5 * time - 4);
            }
            if (time > Stoptimings[_noteIndex]) // 12.10 버그 수정 요소가 적용 안돼서 다시 작성함 이 조건문이 없으면 동시에 들어오는 노트 전부가 Stoptimings[_noteIndex] 을 수정해서 HitNote가 제대로 작동 하지 않음 중요!
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
        //Stoptimings[_noteIndex] = 0f; 끝나고 초기화를 할 필요가 없음 12.10
    }

    #endregion

    #region public


    // 노트 판정 제거 함수 (퍼펙 점수 +50, 굿 +10 - 둘다 콤보카운트++, 배드는 점수 +0 - 콤보 0으로 초기화)
    public void HitNote(int _noteIndex)
    {
        // 추가2
        string judgeResult = "Miss"; // 기본값은 Miss 
        int scoreToAdd = 0; // 점수 초기화
        
            // 다른 노트가 눌렸을 때 패널티 적용
            /*
            if (Stoptimings[_noteIndex] < 0.4f)
            {
                // 추가
                LifeManager.Instance.LoseLife(); // 라이프매니저 호출

            }
            */

            if (Stoptimings[_noteIndex] >= 0.4f)
            {
                if (NoteQueues[_noteIndex].Count > 0 && NoteQueues[_noteIndex] != null)
                {
                    Image go = null;
                    if (NoteQueues[_noteIndex].TryDequeue(out go))
                    {    // 노트 파괴 이펙트 호출
                         // FxManager.Instance?.PlayNoteDestroyEffect(go.transform.position);
                        Destroy(go.gameObject);
                        //Debug.Log("파괴"); 
                    }
                }
                if (NoteQueues[_noteIndex].Count >= 0)
                {
                    if (0.3 <= Stoptimings[_noteIndex] && Stoptimings[_noteIndex] < 0.6)
                    {
                        Debug.Log(Stoptimings[_noteIndex] + " Miss");
                        ComboCnt = 0;
                        judgeResult = "Miss"; // 추가, Miss UI 
                    }

                    else if (0.6 <= Stoptimings[_noteIndex] && Stoptimings[_noteIndex] < 0.8)
                    {
                        ComboCnt++;
                        judgeResult = "Good"; // 추가, Good UI

                        // 추가
                        scoreToAdd = 10;
                        UpdateScore(scoreToAdd);

                        Debug.Log(Stoptimings[_noteIndex] + " Good Combo: " + ComboCnt + "Score : " + Score);
                    }
                    else if (0.8 <= Stoptimings[_noteIndex] && Stoptimings[_noteIndex] < 1)
                    {
                        ComboCnt++;
                        judgeResult = "Perfect";  // 추가, Perfect UI

                        // 추가
                        scoreToAdd = 50;
                        UpdateScore(scoreToAdd);

                        Debug.Log(Stoptimings[_noteIndex] + " Perfect Combo: " + ComboCnt + "Score : " + Score);
                    }

                    // 추가2 Miss일때만 라이프감소 호출
                    if (judgeResult == "Miss" && !isCheat)
                    {
                        LifeManager.Instance.LoseLife();
                        //Stoptimings[_noteIndex] = 0; //추가2 Miss 이후 타이밍 초기화 // 스탑타이밍 초기화는 노트가 파괴되고 이후 오는 노트가 없을 시 초기화 해야한다, 이후 오는 노트가 있으면 그 노트가 스탑타이밍을 관리하기 때문 12.10
                    }

                }
                // 추가2 판정 결과 UI 표시
                judgeUIManager.ShowJudgeUI(judgeResult);

                // 추가2 점수 업데이트
                if (scoreManager != null)
                {
                    scoreManager.AddScore(scoreToAdd); // ScoreManager에 점수 전달
                }
            }
        
    }

    public void PushNote(int _noteIndex)
    {
        
        {NoteCnt++; //12.10 노트갯수 카운팅
        if (!isGameOver || !isClearTriggered) //게임오버가 아니거나 클리어가 안됐으면 로그 출력 12.10
            Debug.Log("NoteCnt : " + NoteCnt);} // 정상 작동 노트를 보내는데에서 문제는 없음 12.10
        CreateMoveNoteToHit(_noteIndex);
    }
    public void NotebyBarintlist(int _bar, int[] _boxlist, int[] _halfboxlist)
    {
        StartCoroutine(Bil(_bar, _boxlist, _halfboxlist));
    }
    //노트 보내는 예
    //마디수, {구성};
    //1,{
    //   1,1,1,1,
    //   1,1,1,1,
    //   1,1,1,1,
    //   1,1,1,1
    //  };
    public IEnumerator Bil(int _bar, int[] _boxlist, int[] _halfboxlist) //12.10 _halfboxlist 추가 원래 boxlist 랑 똑같은 형식으로 넣지만 정박 time + 0.5f 타이밍의 노트만 처리
    {
        //Debug.Log(_bar +"Start" + _boxlist);
        float time = 0;
        bool half = false;
        bool push1 = false;
        bool push2 = false;
        bool push3 = false;
        bool push4 = false;        
        bool push1half = false;
        bool push2half = false;
        bool push3half = false;
        bool push4half = false;
        _bar = (_bar * 4) - 4;
        while (true)
        {
            if (isGameOver || isClearTriggered)// 게임오버거나 클리어하면 루틴 탈출 12.10
            {
                Debug.Log("루틴탈출인가1");
                yield break;
                
            }
            time += Time.deltaTime * Bpm * 0.005f; //그냥 여기다 2f 곱해도 되긴하지만 그만큼 마디수가 2배로 늘어서 코루틴이 두배로 돌아야해서 상당히 부담스럽다. 그리고 문제는 이걸로도 2중 hit가 해결이 안된다. 12.10
            if (time >=0.5)
                half = true;
            if (time > 1)
            {

                time %= 1;
                _bar--;
                Debug.Log("Bar" + _bar);
                half = false;
            }
            if (time < 1) //최적화1 검사 횟수 줄임 12.10
            {
                if (_bar == 0 && !push1)
                { if (_boxlist[0] == 1) PushNote(2); if (_boxlist[1] == 1) PushNote(3); if (_boxlist[2] == 1) PushNote(1); if (_boxlist[3] == 1) PushNote(0); push1 = true; //Debug.Log("push1:" + _bar); 
                }
                if (_bar == -1 && !push2)
                { if (_boxlist[4] == 1) PushNote(2); if (_boxlist[5] == 1) PushNote(3); if (_boxlist[6] == 1) PushNote(1); if (_boxlist[7] == 1) PushNote(0); push2 = true; //Debug.Log("push2:" + _bar); 
                }
                if (_bar == -2 && !push3)
                { if (_boxlist[8] == 1) PushNote(2); if (_boxlist[9] == 1) PushNote(3); if (_boxlist[10] == 1) PushNote(1); if (_boxlist[11] == 1) PushNote(0); push3 = true; //Debug.Log("push3:" + _bar); 
                }
                if (_bar == -3 && !push4)
                { if (_boxlist[12] == 1) PushNote(2); if (_boxlist[13] == 1) PushNote(3); if (_boxlist[14] == 1) PushNote(1); if (_boxlist[15] == 1) PushNote(0); push4 = true; //Debug.Log("push4:" + _bar); 
                }

                if (half)  // 이곳이 반박만 처리하는 구간 12.10
                {
                    if (_bar == 0 && !push1half)
                    { if (_halfboxlist[0] == 1) PushNote(2); if (_halfboxlist[1] == 1) PushNote(3); if (_halfboxlist[2] == 1) 
                            PushNote(1); if (_halfboxlist[3] == 1) PushNote(0); push1half = true; //Debug.Log("push1half:" + _bar); 
                    }
                    if (_bar == -1 && !push2half)
                    { if (_halfboxlist[4] == 1) PushNote(2); if (_halfboxlist[5] == 1) PushNote(3); if (_halfboxlist[6] == 1) 
                            PushNote(1); if (_halfboxlist[7] == 1) PushNote(0); push2half = true; //Debug.Log("push2half:" + _bar); 
                    }
                    if (_bar == -2 && !push3half)
                    { if (_halfboxlist[8] == 1) PushNote(2); if (_halfboxlist[9] == 1) PushNote(3); if (_halfboxlist[10] == 1) 
                            PushNote(1); if (_halfboxlist[11] == 1) PushNote(0); push3half = true; //Debug.Log("push3half:" + _bar); 
                    }
                    if (_bar == -3 && !push4half)
                    { if (_halfboxlist[12] == 1) PushNote(2); if (_halfboxlist[13] == 1) PushNote(3); if (_halfboxlist[14] == 1) 
                            PushNote(1); if (_halfboxlist[15] == 1) PushNote(0); push4half = true; //Debug.Log("push4half:" + _bar); 
                    }
                }

                if (_bar < -4)
                {
                    Debug.Log("루틴탈출인가2");
                    yield break; //최적화2 _bar 가 값이 무한히 감소하며 코루틴이 돌아가는 걸 방지 12.10
                }

                yield return null;

            }
        }

    }
    //점수 0으로 초기화
    public void Score0()
    {
        Score = 0;

        // 추가
        Stage1UIManager.Instance.SetScore(Score); // 스테이지 매니저 호출
        //ScoreManager.Instance.SetScoretext(Score); // 스테이지 매니저 호출
    }

    // 추가 함수
    public void UpdateScore(int value)
    {
        Score += value;
        Stage1UIManager.Instance.SetScore(Score); // 업데이트 추가
        //ScoreManager.Instance.SetScoretext(Score); // 업데이트 추가
    }
    public void SetBpm(float _BPM)
    {
        Bpm = _BPM;
    }
    public bool sampleNotesComming(AudioSource _audioSource)
    {
        {

            int[] pattern1box =
            {
            1,0,0,0,
            0,1,0,0,
            0,0,0,1,
            0,0,1,1
            };
            int[] pattern2box =
            {
            0,1,1,0,
            1,0,0,1,
            0,1,1,0,
            0,0,1,0
            };
            int[] pattern3box =
            {
            1,1,1,1,
            1,0,1,0,
            0,1,0,0,
            0,0,1,0
            };
            int[] pattern4box =
            {
            1,1,0,0,
            0,1,1,0,
            0,0,1,1,
            0,1,0,0
            };
            int[] pattern5box =
            {
            0,1,1,0,
            0,0,1,0,
            1,0,0,1,
            0,1,0,0
            };
            int[] pattern6box =
            {
            1,0,0,1,
            0,0,0,1,
            1,0,0,1,
            0,1,1,0
            };
            int[] pattern7box =
            {
            1,0,0,1,
            0,0,1,0,
            0,1,1,0,
            0,1,0,0
            };
            int[] pattern8box =
            {
            1,0,0,1,
            0,1,0,0,
            0,0,1,0,
            0,1,0,1
            };
            int[] pattern9box =
            {
            1,1,1,1,
            1,1,0,0,
            0,0,1,1,
            1,1,0,1
            };
            int[] pattern10box =
            {
            1,1,1,1,
            0,1,1,0,
            0,1,1,0,
            1,1,1,1
            };            
            int[] pattern11box =
            {
            1,0,0,0,
            0,0,1,0,
            0,0,0,0,
            0,0,0,0
            };
            _audioSource.Play();
            for (int i = 0; i < 80; i += 8)
            {
                NotebyBarintlist(i, pattern1box, pattern11box);
                if (i == 16)
                    NotebyBarintlist(i + 1, pattern10box, pattern11box);
                else
                    NotebyBarintlist(i + 1, pattern2box, pattern1box);
                NotebyBarintlist(i + 2, pattern3box, pattern11box);
                NotebyBarintlist(i + 3, pattern4box, pattern11box);
                NotebyBarintlist(i + 4, pattern5box, pattern11box);
                NotebyBarintlist(i + 5, pattern6box, pattern11box);
                if(i == 40)
                    NotebyBarintlist(i + 6, pattern9box, pattern1box);
                else
                    NotebyBarintlist(i + 6, pattern7box, pattern11box);
                if(i == 32 || i == 64)
                    NotebyBarintlist(i + 7, pattern10box, pattern1box);
                else
                    NotebyBarintlist(i + 7, pattern8box, pattern11box);
            }
        }
        //Debug.Log("sampleNotesComming");
        return true;
    }

    public bool CheatLifeNtime() // 라이프 치트키 추가 12.10
    {
        isCheat=!isCheat;
        Debug.Log("Cheat On/Off");
        clearTimer = 10f;
        if (isCheat)
            return true;
        else 
            return false;
    }
    #endregion
}
