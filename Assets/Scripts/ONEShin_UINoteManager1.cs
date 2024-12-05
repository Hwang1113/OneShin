using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ONEShin_UINoteManager1 : MonoBehaviour
{
    #region private ����
    private List<Image> Hitboxes = new List<Image>(); // Q, W, E, R�� ���� ��Ʈ�ڽ��� ����Ʈ�� ����
    private List<Image> Noteboxes = new List<Image>(); // Q, W, E, R�� ���� ��Ʈ�ڽ��� ����Ʈ�� ����
    public List<Image> Accuracy = new List<Image>(); // Accuracy�� ����Ʈ�� ���� (Perfect,Good,Bad)
    private List<Vector2> NoteboxStartPoints = new List<Vector2>(); // �� ��Ʈ Ÿ���� ���� ��ġ�� ����Ʈ�� ����
    private List<Vector2> NoteBoxEndPoints = new List<Vector2>(); // �� ��Ʈ Ÿ���� �� ��ġ�� ����Ʈ�� ����
    private List<Queue<Image>> NoteQueues = new List<Queue<Image>>(); // �� ��Ʈ Ÿ���� ť�� ����Ʈ�� ����
    private float[] Stoptimings = new float[4]; // ��ž Ÿ�̹� �迭
    #endregion
    #region public ����
    public int ComboCnt = 0; //�޺� ī��Ʈ
    public int Score = 0; // ���� 
    public int lifeCnt = 5;
    public bool isLifeZero = false; // ������ 0�� ������ true
    public int totalNoteCnt = 0;
    public float Bpm = 60; // ���� ���� �Ǵ� BPM ����
    public bool isFever = false;
    public const int EnterFeverCnt = 100; 

    // ������ �Ŵ����� ���⼭ �����ؼ� �������� ������ ������ �Ŵ��� ��ũ��Ʈ�� ������ �� �ְ� ������ �ʿ��� 12.05

    #endregion
    #region private
    private void Awake()
    {
        // Q, W, E, R�� ���� Hitbox�� Notebox �ʱ�ȭ
        for (int i = 0; i < 4; i++)
        {
            Hitboxes.Add(transform.GetChild(i).GetComponent<Image>());
            Noteboxes.Add(Hitboxes[i].GetComponentsInChildren<Image>()[1]);
            NoteQueues.Add(new Queue<Image>());
        }
        // �� ��Ʈ�� ���� ��ġ�� �� ��ġ ����
        for (int i = 0; i < 4; i++)
        {
            NoteboxStartPoints.Add(Noteboxes[i].rectTransform.anchoredPosition);
            NoteBoxEndPoints.Add(Hitboxes[i].rectTransform.anchoredPosition + new Vector2(-Hitboxes[i].rectTransform.sizeDelta.x, Hitboxes[i].rectTransform.sizeDelta.y));
        }

        Accuracy.AddRange(transform.GetChild(4).GetComponentsInChildren<Image>(false));

        foreach (Image img in Accuracy)
        {
            img.gameObject.SetActive(false);  // �ش� GameObject ��Ȱ��ȭ
        }
        // ������ �Ŵ����� ���⼭ �����ؼ� �������� ������ ������ �Ŵ��� ��ũ��Ʈ�� ������ �� �ְ� [SerializeField]�� �������� ���⼭ ��Ե� �����;��� 12.05

    }
    private IEnumerator CreateMoveNoteToHitCoroutine(Image _Notebox, int _noteIndex)
    {
        _Notebox.gameObject.SetActive(true);
        //Stoptimings[_noteIndex] = 0f;// ���⼭ ��ġ�� ������ �����? ������ ���� 2���̻� �������� ���������� �����Ǵ°� �ƴѵ���
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
    #region ��Ʈ ���� �� ����
    // ��Ʈ ���� ���� �Լ� (���� ���� +50, �� +10 - �Ѵ� ComboCnt++;, ���� �޺� 0���� �ʱ�ȭ lifeCnt--;)
    public void HitNote(int _noteIndex)
    {
        Debug.Log(Score);
        if (isFever)
        { 
            feverTimeHit();

        }

        StartCoroutine(HitNoteCo(_noteIndex));
    }

    public IEnumerator HitNoteCo(int _noteIndex)
    {
        // �ٸ� ��Ʈ�� ������ �� �г�Ƽ ����
        if (Stoptimings[_noteIndex] < 0.4f)
        {
            if (!isFever) // �ǹ�Ÿ���� �ƴϸ� �г�Ƽ �޺� ī��Ʈ 0���� �ʱ�ȭ
                ComboCnt = 0;   // �ٸ� ��Ʈ�� �������Ƿ�, �޺� �ʱ�ȭ
            //�ǹ� Ÿ���̸� �г�Ƽ ������  
        }

        if (Stoptimings[_noteIndex] >= 0.4f)
        {
            if (NoteQueues[_noteIndex].Count > 0 && NoteQueues[_noteIndex] != null) //��Ʈ �ı�
            {
                Image go = null;
                if (NoteQueues[_noteIndex].TryDequeue(out go))
                    Destroy(go.gameObject);
            }
            if (NoteQueues[_noteIndex].Count >= 0 && !isFever) // ���� ���� ���, �ǹ�Ÿ���̸� ���� ��� ����
            {
                if (0.4 <= Stoptimings[_noteIndex] && Stoptimings[_noteIndex] < 0.6) //bad
                {
                    ComboCnt = 0;
                    lifeCnt--;
                    //�̰��� �ٳ��� �� ���� ȣ���ϸ� �� 12.05���� 
                    if (lifeCnt == 0)
                        isLifeZero = true;
                    StartCoroutine(ShowAccuracyUI(2));
                }

                else if (0.6 <= Stoptimings[_noteIndex] && Stoptimings[_noteIndex] < 0.75) //good
                {
                    ComboCnt++;
                    Score += 10;
                    if (ComboCnt != 0 && 0 == ComboCnt % EnterFeverCnt)
                        StartCoroutine(StartFeverTime());
                    StartCoroutine(ShowAccuracyUI(1));

                }
                else if (0.75 <= Stoptimings[_noteIndex] && Stoptimings[_noteIndex] < 0.85) //perfect
                {
                    ComboCnt++;
                    Score += 50;
                    if (ComboCnt != 0 && 0 == ComboCnt % EnterFeverCnt)
                        StartCoroutine(StartFeverTime());
                    StartCoroutine(ShowAccuracyUI(0));
                }
                else if (0.85 <= Stoptimings[_noteIndex] && Stoptimings[_noteIndex] < 1)//good
                {
                    ComboCnt++;
                    Score += 10;
                    if (ComboCnt != 0 && 0 == ComboCnt % EnterFeverCnt)
                        StartCoroutine(StartFeverTime());
                    StartCoroutine(ShowAccuracyUI(1));
                }
            }

        }
        yield return null;
    }
    //3�ʵ��� ��Ȯ���� �˷� ���ش� 0����,1��,2���
    public IEnumerator ShowAccuracyUI(int _Accuracy) 
    {
        float time = 0f;
        foreach (Image img in Accuracy)
            img.gameObject.SetActive(false);
        Accuracy[_Accuracy].gameObject.SetActive(true);
        while (time < 2)
        {
            time += Time.deltaTime;
            yield return null;
        }
        Accuracy[_Accuracy].gameObject.SetActive(false);
    }
    #endregion
    #region ��Ʈ����
    public void PushNote(int _noteIndex)
    {
        //CreateMoveNoteToHit(_noteIndex);
        totalNoteCnt++;
        Image noteGo = Instantiate(Noteboxes[_noteIndex], Hitboxes[_noteIndex].transform);
        NoteQueues[_noteIndex].Enqueue(noteGo);
        StartCoroutine(CreateMoveNoteToHitCoroutine(noteGo, _noteIndex));
    } // ��Ʈ(q2,w3,e0,r1) �� �ϳ� ������

    public void NotebyBarintlist(int _bar, int[] _boxlist)
    {
        StartCoroutine(NoteBil(_bar, _boxlist));
    }
    //��Ʈ ������ ��
    //�����, {����};
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
        //Debug.Log(_bar +"Start" + _boxlist);
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

    }

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
        NotebyBarintlist(onebar, pattern1box);
        NotebyBarintlist(twobar, pattern2box);
        NotebyBarintlist(3, pattern3box);
        NotebyBarintlist(4, pattern4box);
        NotebyBarintlist(5, pattern5box);
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

        //Debug.Log("sampleNotesComming");
        return true;
    } //�׽�Ʈ �� ��Ʈ ������ ������
    #endregion
    #region �ۺ� ���� ����
    public void Score0()
    {
        Score = 0;
    } // ���ھ� 0���� �ʱ�ȭ
    public void SetBpm(float _BPM)
    {
        Bpm = _BPM;
    } // BPM ����
    public void SetLife5()
    {
        lifeCnt = 5;
    } // ������ ī��Ʈ 5�� �ʱ�ȭ

    public string Rank() // ������Ʈ ���� �� ���� ������ ���� ��ũ ���� 
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
            return ("���� ��ũ���� �ȉ�");
    }

    public void AddCombo99() // �޺��� 99 ���ϱ�
    {
        ComboCnt += 99;
    }

    #endregion
    #region �ǹ�Ÿ��
    public IEnumerator StartFeverTime()
    {
        float feverTimeCnt = 0;
        isFever = true;
        while (feverTimeCnt < 3)
        {
            feverTimeCnt += Time.deltaTime;
            //Debug.Log("�ǹ�Ÿ��!");
            yield return null;
        }
        if (feverTimeCnt >= 3)
        {
            Debug.Log("�ǹ�Ÿ�ӳ�~");
            isFever = false;
        }
    }

    public void feverTimeHit() // �ǹ�Ÿ����Ʈ �Լ� �����ϸ� 1�� �߰�
    {
            Score += 1;
        Debug.Log(Score);
    }
    #endregion
    #region �ۺ� ���� Ȯ��
    public bool IsFeverTime
    {
        get { return isFever; }  // ���� ��ȯ
    }

    public int ChecklifeCnt
    {
        get { return lifeCnt; }  // ���� ��ȯ
    }
    #endregion
    #endregion
}
