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
    public float[] barNBeat = { 0, 0 }; // ��� ��ڿ� ������ ���� {bar,beat}
    public float maxBeat = 4f;  //4���� 4���ڸ� 4�� �Է� , 4���� 3���ڸ� 3�� �Է�
    public int[] QWER = { 0, 0, 0, 0 }; // � ��Ʈ�� ���ÿ� ������ ����


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
    // ��Ʈ ���� �Լ� (���� ���� +50, �� +10 - �Ѵ� �޺�ī��Ʈ++, ���� ���� +0 - �޺� 0���� �ʱ�ȭ)
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

    // ��Ʈ�� �����ϴ� �Լ� (Q, W, E, R�� �°�)
    public void PushNote(int _noteIndex)
    {
        CreateMoveNoteToHit(_noteIndex);
    }

    public void PushNotes(int[] _QWER) // QWER[] �������� ��Ʈ�� ���ÿ� ����
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
    public IEnumerator WhenPushNotesCo(int[] _QWER, float[] _barNBeat) // ���ÿ�(�Ǵ� ���Ϸ�) Ǫ���� ��ư�� ���� ��� ����ڿ� ������ ���ϴ� �Լ�
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
                Debug.Log("��Ʈ ��");
                yield break;
            }
            yield return null;
        }
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
    #endregion
}
