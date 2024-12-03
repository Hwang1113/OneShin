using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ONEShin_UINoteManager1 : MonoBehaviour
{
    private List<Image> Hitboxes = new List<Image>(); // Q, W, E, R에 대한 히트박스를 리스트로 관리
    private List<Image> Noteboxes = new List<Image>(); // Q, W, E, R에 대한 노트박스를 리스트로 관리
    private float[] Stoptimings = new float[4]; // 스탑 타이밍 배열
    private List<Vector2> NoteboxStartPoints = new List<Vector2>(); // 각 노트 타입의 시작 위치를 리스트로 관리
    private List<Vector2> NoteBoxEndPoints = new List<Vector2>(); // 각 노트 타입의 끝 위치를 리스트로 관리
    private List<Queue<Image>> NoteQueues = new List<Queue<Image>>(); // 각 노트 타입의 큐를 리스트로 관리
    private float Bpm = 0f; //60bpm 1분에 60번, 1초 1번
    private float ComboCnt = 0f; //콤보 카운트
    
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
    private void CreateMoveNoteToHit(int noteIndex)
    {
        Image noteGo = Instantiate(Noteboxes[noteIndex], Hitboxes[noteIndex].transform);
        NoteQueues[noteIndex].Enqueue(noteGo);
        StartCoroutine(CreateMoveNoteToHitCoroutine(noteGo, noteIndex));
    }

    // 노트를 히트박스까지 이동시키는 코루틴
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
    // 노트 판정 함수 (퍼펙, 굿 경우 콤보카운트++, 배드는 0으로 초기화)
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

    // 노트를 생성하는 함수 (Q, W, E, R에 맞게)
    public void PushNote(int noteIndex)
    {
        CreateMoveNoteToHit(noteIndex);
    }

    //Bpm을 넣으면 델타타임 대신 쓸 수 있는 BpmDelta를 반환함과 동시에 Bpm 변수를 BpmDelta로 만듬
    public float SetBpmDelta(float _bpm)
    {
        float beatsPerSecond = _bpm / 60f;  // BPM을 초당 비트 수로 변환
        float BpmDelta = Time.deltaTime * beatsPerSecond;  // Time.deltaTime을 BPM에 맞게 변환
        Bpm = BpmDelta;
        return BpmDelta;
    }

    #endregion
}
