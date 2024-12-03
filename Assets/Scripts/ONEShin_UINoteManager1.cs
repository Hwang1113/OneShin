using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ONEShin_UINoteManager1 : MonoBehaviour
{
    private List<Image> Hitboxes = new List<Image>(); // Q, W, E, R�� ���� ��Ʈ�ڽ��� ����Ʈ�� ����
    private List<Image> Noteboxes = new List<Image>(); // Q, W, E, R�� ���� ��Ʈ�ڽ��� ����Ʈ�� ����
    private float[] Stoptimings = new float[4]; // ��ž Ÿ�̹� �迭
    private List<Vector2> NoteboxStartPoints = new List<Vector2>(); // �� ��Ʈ Ÿ���� ���� ��ġ�� ����Ʈ�� ����
    private List<Vector2> NoteBoxEndPoints = new List<Vector2>(); // �� ��Ʈ Ÿ���� �� ��ġ�� ����Ʈ�� ����
    private List<Queue<Image>> NoteQueues = new List<Queue<Image>>(); // �� ��Ʈ Ÿ���� ť�� ����Ʈ�� ����
    private float Bpm = 0f; //60bpm 1�п� 60��, 1�� 1��
    private float ComboCnt = 0f; //�޺� ī��Ʈ
    
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
    private void CreateMoveNoteToHit(int noteIndex)
    {
        Image noteGo = Instantiate(Noteboxes[noteIndex], Hitboxes[noteIndex].transform);
        NoteQueues[noteIndex].Enqueue(noteGo);
        StartCoroutine(CreateMoveNoteToHitCoroutine(noteGo, noteIndex));
    }

    // ��Ʈ�� ��Ʈ�ڽ����� �̵���Ű�� �ڷ�ƾ
    private IEnumerator CreateMoveNoteToHitCoroutine(Image _Notebox, int noteIndex)
    {
        _Notebox.gameObject.SetActive(true);
        Stoptimings[noteIndex] = 0f;
        float time = 0f;
        while (time < 1f && _Notebox != null)
        {
            if (_Notebox != null)
            {
                _Notebox.rectTransform.anchoredPosition = Vector3.Lerp(Noteboxes[noteIndex].rectTransform.anchoredPosition, Hitboxes[noteIndex].rectTransform.anchoredPosition, time * 10 / 8f);
                _Notebox.rectTransform.sizeDelta = Vector3.Lerp(Noteboxes[noteIndex].rectTransform.sizeDelta, Vector2.zero, 5 * time - 4);
            }
            if (time > 1f)
            {
                time = 1f;
            }
            Stoptimings[noteIndex] = time;
            time += Time.deltaTime;
            yield return null;
        }
        if (_Notebox != null)
        {
            Stoptimings[noteIndex] = 0.5f;
            _Notebox.rectTransform.anchoredPosition = NoteBoxEndPoints[noteIndex];
            HitNote(noteIndex);
        }
        Stoptimings[noteIndex] = 0f;
    }

    #endregion

    #region public
    // ��Ʈ ���� �Լ� (����, �� ��� �޺�ī��Ʈ++, ���� 0���� �ʱ�ȭ)
    public void HitNote(int noteIndex)
    {
        if (Stoptimings[noteIndex] >= 0.4f)
        {
            if (NoteQueues[noteIndex].Count > 0 && NoteQueues[noteIndex] != null)
            {
                Image go = null;
                if (NoteQueues[noteIndex].TryDequeue(out go))
                    Destroy(go.gameObject);
            }

            if (NoteQueues[noteIndex].Count >= 0)
            {
                if (0.4 <= Stoptimings[noteIndex] && Stoptimings[noteIndex] < 0.6)
                {
                    Debug.Log(Stoptimings[noteIndex] + " Bad");
                    ComboCnt = 0;
                }

                else if (0.6 <= Stoptimings[noteIndex] && Stoptimings[noteIndex] < 0.75)
                {
                    ComboCnt++;
                    Debug.Log(Stoptimings[noteIndex] + " Good Combo: " + ComboCnt);
                }
                else if (0.75 <= Stoptimings[noteIndex] && Stoptimings[noteIndex] < 0.85)
                {
                    ComboCnt++;
                    Debug.Log(Stoptimings[noteIndex] + " Perfect Combo: " + ComboCnt);
                }
                else if (0.85 <= Stoptimings[noteIndex] && Stoptimings[noteIndex] < 1)
                {  
                    ComboCnt++;
                    Debug.Log(Stoptimings[noteIndex] + " Good! Combo: " + ComboCnt);
                }
            }
        }
    }

    // ��Ʈ�� �����ϴ� �Լ� (Q, W, E, R�� �°�)
    public void PushNote(int noteIndex)
    {
        CreateMoveNoteToHit(noteIndex);
    }

    //Bpm�� ������ ��ŸŸ�� ��� �� �� �ִ� BpmDelta�� ��ȯ�԰� ���ÿ� Bpm ������ BpmDelta�� ����
    public float SetBpmDelta(float _bpm)
    {
        float beatsPerSecond = _bpm / 60f;  // BPM�� �ʴ� ��Ʈ ���� ��ȯ
        float BpmDelta = Time.deltaTime * beatsPerSecond;  // Time.deltaTime�� BPM�� �°� ��ȯ
        Bpm = BpmDelta;
        return BpmDelta;
    }

    #endregion
}
