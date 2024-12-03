using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ONEShin_UINoteManager : MonoBehaviour
{
    private Image Hitbox = null; // 최초 R 히트박스를 담을 이미지
    private Image Notebox = null; // 최초 R 노트박스를 담을 이미지 
    private float Stoptiming = 0f; // R 노트를 히트 했을때 판정을 담을 배열 총 4개
    private Vector2 NoteboxStartPoint = Vector2.zero; //각각 R 노트 시작지점을 
    private float HitboxWidth = 0f;
    private float HitboxHeight = 0f;
    private Vector2 NoteBoxEndPoint = Vector2.zero; // R 노트 끝나는지점
    private Queue<Image> NoteQueue = new Queue<Image>(); // R노트 넣을 큐리스트 이걸로 히트 처리함

    private void Awake()
    { 
        Hitbox = GetComponentInChildren<Image>(); // (this)캔버스 자식으로 있음

        HitboxWidth = Hitbox.rectTransform.sizeDelta.x; 
        HitboxHeight = Hitbox.rectTransform.sizeDelta.y; 


        Notebox = Hitbox.GetComponentsInChildren<Image>()[1]; // 히트박스 자식으로 있음 GetChildren하면 본인이 불러와지기에 이와 같이 호출
        NoteboxStartPoint = Notebox.rectTransform.anchoredPosition; // 노트박스가 화면으로 나오기 전 위치 (시작지점)
        NoteBoxEndPoint = Hitbox.rectTransform.anchoredPosition + new Vector2(-HitboxWidth , HitboxHeight); // 노트박스가 화면 밖에 나올 위치 (엔딩지점)
         //노트 담을 큐 리스트

    }
    #region private
    private void CreateMoveNoteRToHit()  //////// 노트를 생성하고, 시작 지점(NoteboxStartPoint[0]) 부터 히트박스 뒤(NoteBoxEndPoint) 까지 움직인다.
    {

        Image noteGo = Instantiate(Notebox, Hitbox.transform);
        NoteQueue.Enqueue(noteGo);
        StartCoroutine(CreateMoveNoteRToHitCoroutine(noteGo));
    }
    
    private IEnumerator CreateMoveNoteRToHitCoroutine(Image _Notebox)
    {
        //Notebox.rectTransform.anchoredPosition = Hitbox.rectTransform.anchoredPosition;
        _Notebox.gameObject.SetActive(true);
        Stoptiming = 0f;
        float time = 0f;
        while (time < 1f && _Notebox != null)
        {
            if (_Notebox != null)
            {
                _Notebox.rectTransform.anchoredPosition = Vector3.Lerp(Notebox.rectTransform.anchoredPosition, Hitbox.rectTransform.anchoredPosition, time * 10 / 8f);

                _Notebox.rectTransform.sizeDelta = Vector3.Lerp(Notebox.rectTransform.sizeDelta, Vector2.zero, 5*time - 4);
            }
            if (time > 1f)
            {
                time = 1f;
            }
            Stoptiming = time;
            time += Time.deltaTime;
            yield return null;
        }
        if (_Notebox != null)
        {
            Stoptiming = 0.4f;
            _Notebox.rectTransform.anchoredPosition = NoteBoxEndPoint;
            HitNote();

        }
        Stoptiming = 0f;
    }
    #endregion
    #region public
    public void HitNote()
    {
        //Notebox.gameObject.SetActive(false);
        if (Stoptiming >= 0.4f)
        {
            if (NoteQueue.Count > 0 && NoteQueue != null) // 노트갯수가 0초과임과 동시에 null 이 아니면 노트 삭제 
            {
                Image go = null;
                if (NoteQueue.TryDequeue(out go))
                    Destroy(go.gameObject);
            }
            if (NoteQueue.Count >= 0)
                if(0.4 <= Stoptiming &&   Stoptiming < 0.6)
                    Debug.Log(Stoptiming + "Bad");
                else if (0.6 <= Stoptiming && Stoptiming < 0.75)
                    Debug.Log(Stoptiming + "Good");                
                else if (0.75 <= Stoptiming && Stoptiming < 0.85 )
                    Debug.Log(Stoptiming + "Perfect");
                else if (0.85 <= Stoptiming && Stoptiming < 1 )
                    Debug.Log(Stoptiming + "Good");

            //이상 ~ 미만 기준임 
            // 0.4미만은 무판정 , Bad 0.4 ~ 0.6, Good 0.6 ~ 0.75, Perfect 0.75 ~ 0.85, Good 0.85 ~ 1   1이상이 되면 0.4로 자동 치환 그래서 Bad 판정후 노트 사라짐

        }

        //Stoptiming 은 점수로 표기하면 되지 않을까?
    }

    public void PushNoteR() //노트 생성
    {
        CreateMoveNoteRToHit();
    }

    #endregion
}
