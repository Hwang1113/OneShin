using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ONEShin_UINoteManager1 : MonoBehaviour
{
    #region private ����
    private List<Image> Hitboxes = new List<Image>(); // Q, W, E, R�� ���� ��Ʈ�ڽ��� ����Ʈ�� ����
    private List<Image> Noteboxes = new List<Image>(); // Q, W, E, R�� ���� ��Ʈ�ڽ��� ����Ʈ�� ����
    private float[] Stoptimings = new float[4]; // ��ž Ÿ�̹� �迭
    private List<Vector2> NoteboxStartPoints = new List<Vector2>(); // �� ��Ʈ Ÿ���� ���� ��ġ�� ����Ʈ�� ����
    private List<Vector2> NoteBoxEndPoints = new List<Vector2>(); // �� ��Ʈ Ÿ���� �� ��ġ�� ����Ʈ�� ����
    private List<Queue<Image>> NoteQueues = new List<Queue<Image>>(); // �� ��Ʈ Ÿ���� ť�� ����Ʈ�� ����
    #endregion
    #region public ����
    public int ComboCnt = 0; //�޺� ī��Ʈ
    public int Score = 0; // ���� 
    public float Bpm = 60; // ���� ���� �Ǵ� BPM ����
    //public float[] barNBeat = { 0, 0 }; // ��� ��ڿ� ������ ���� {bar,beat}
    //public float maxBeat = 4f;  //4���� 4���ڸ� 4�� �Է� , 4���� 3���ڸ� 3�� �Է�
    //public float maxBar = 4f; // �ִ븶��
    //public int[] QWER = { 0, 0, 0, 0 }; // � ��Ʈ�� ���ÿ� ������ ����

    #endregion
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
    }


    #region private
    // ��Ʈ�� �����ϴ� �Լ� (Q, W, E, R�� �°�)

    // ��Ʈ�� �����ϰ� �̵���Ű�� �Լ�
    private void CreateMoveNoteToHit(int _noteIndex)
    {
        Image noteGo = Instantiate(Noteboxes[_noteIndex], Hitboxes[_noteIndex].transform);
        NoteQueues[_noteIndex].Enqueue(noteGo);
        StartCoroutine(CreateMoveNoteToHitCoroutine(noteGo, _noteIndex));
    }
    // ��Ʈ�� ��Ʈ�ڽ����� �̵���Ű�� �ڷ�ƾ
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


    // ��Ʈ ���� ���� �Լ� (���� ���� +50, �� +10 - �Ѵ� �޺�ī��Ʈ++, ���� ���� +0 - �޺� 0���� �ʱ�ȭ)
    public void HitNote(int _noteIndex)
    {
        // �ٸ� ��Ʈ�� ������ �� �г�Ƽ ����
        if (Stoptimings[_noteIndex] < 0.4f)
        {
            // �ٸ� ��Ʈ�� �������Ƿ�, �޺� �ʱ�ȭ �� ���� ����
            ComboCnt = 0;
            Score -= 10; 
            Debug.Log("�ٸ� ��Ʈ ����");
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

    //public void PushNotes(int[] _QWER) // QWER[] �������� ��Ʈ�� ���ÿ� ����
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
    //public IEnumerator WhenPushNotesCo(int[] _QWER, float[] _phraseNBarNBeat) // ���ÿ�(�Ǵ� ���Ϸ�) Ǫ���� ��ư�� ���� ��� ����ڿ� ������ ���ϴ� �Լ�
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
    //            //Debug.Log("��Ʈ ��");
    //            yield break;
    //        }
    //        yield return null;
    //    }
    //}


    public void NotebyBarintlist(int _bar, int[] _boxlist)
    {
        StartCoroutine(Bil(_bar, _boxlist));
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


    //���� 0���� �ʱ�ȭ
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


    //    // ���� ���� : �ڷ�ƾ���� ���� ��Ʈ �� ������ ��ư���� ��Ʈ �� �� �ִ� ��Ʈ�� 2�� �̻� ������ ��Ʈ�� ���ϴ� ���װ� ����
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
