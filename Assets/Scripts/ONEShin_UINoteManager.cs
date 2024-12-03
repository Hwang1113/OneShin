using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ONEShin_UINoteManager : MonoBehaviour
{
    private Image Hitbox = null; // 최초 R 히트박스를 담을 이미지
    private Image[] Hitboxes = new Image[3]; // 히트박스를 담을 이미지 총 4개
    private Image Notebox = null; // 최초 R 노트박스를 담을 이미지 
    private Image[] Noteboxes = null; // 노트박스를 담을 이미지 총 4개
    private float[] Stoptiming = new float[3]; //노트를 히트 했을때 판정을 담을 배열 총 4개
    private Vector2[] NoteboxStartPoint = new Vector2[3]; //각각 노트 시작지점을 담을 배열 총 4개
    private float HitboxWidth = 0f;
    private float HitboxHeight = 0f;
    private Vector2[] NoteBoxEndPoint = new Vector2[3]; //각각 노트 끝나는지점을 담을 배열 총 4개
    private Queue<Image> NoteQueue = new Queue<Image>();
    private Queue<Image>[] NoteQueues = new Queue<Image>[3]; // 큐 노트리스트를 담을 배열 총 4개
    private GameObject[] HitandNoteBoxes = new GameObject[3]; // 히트박스랑 노트박스를 담을 (빈)게임 오브젝트 4개

    private void Awake()
    {
        Hitbox = GetComponentInChildren<Image>();
        Hitboxes[0] = Hitbox;
        HitboxWidth = Hitboxes[0].rectTransform.sizeDelta.x; 
        HitboxHeight = Hitboxes[0].rectTransform.sizeDelta.y; 


        Notebox = Hitbox.GetComponentsInChildren<Image>()[1];
        //Noteboxes[0] = Notebox;
        NoteboxStartPoint[0] = Notebox.rectTransform.anchoredPosition; // 노트박스가 화면으로 나오기 전 위치 (시작지점)
        NoteBoxEndPoint[0] = Hitboxes[0].rectTransform.anchoredPosition + new Vector2(-HitboxWidth , HitboxHeight); // 노트박스가 화면 밖에 나올 위치 (엔딩지점)
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
        Stoptiming[0] = 0f;
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
            Stoptiming[0] = time;
            time += Time.deltaTime;
            yield return null;
        }
        if (_Notebox != null)
        {
            Stoptiming[0] = 0.5f;
            _Notebox.rectTransform.anchoredPosition = NoteBoxEndPoint[0];
            HitNoteR();
        }
        Stoptiming[0] = 0f;
    }
    #endregion
    #region public
    public void HitNoteR()
    {
        //Notebox.gameObject.SetActive(false);
        if (Stoptiming[0] >= 0.5f)
        {
            if (NoteQueue.Count > 0 && NoteQueue != null) // 노트갯수가 0초과임과 동시에 null 이 아니면 노트 삭제 
            {
                Image go = null;
                if (NoteQueue.TryDequeue(out go))
                    Destroy(go.gameObject);
            }
            if (NoteQueue.Count >= 0)
                if(0.5 <= Stoptiming[0] &&   Stoptiming[0] < 0.6)
                    Debug.Log(Stoptiming[0] + "Bad");
                else if (0.6 <= Stoptiming[0] && Stoptiming[0] < 0.75)
                    Debug.Log(Stoptiming[0] + "Good");                
                else if (0.75 <= Stoptiming[0] && Stoptiming[0] < 0.85 )
                    Debug.Log(Stoptiming[0] + "Perfect");
                else if (0.85 <= Stoptiming[0] && Stoptiming[0] < 1 )
                    Debug.Log(Stoptiming[0] + "Good");

            //이상 ~ 미만 기준임 
            // 0.5이하는 무판정 , Bad 0.5 ~ 0.6, Good 0.6 ~ 0.75, Perfect 0.75 ~ 0.85, Good 0.85 ~ 1   1이상이 되면 0.5로 자동 치환 그래서 Bad 판정후 노트 사라짐

        }

        //Stoptiming[0] 은 점수로 표기하면 되지 않을까?
    }

    public void PushNoteR() //노트 생성
    {
        CreateMoveNoteRToHit();
    }

    #endregion
}
