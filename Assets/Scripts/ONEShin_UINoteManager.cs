using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class ONEShin_UINoteManager : MonoBehaviour
{
    private Image Hitbox = null;
    private Image Notebox = null;
    private float Stoptiming = 0f;
    private Vector2 NoteboxStartPoint = Vector2.zero;
    private float HitboxWidth = 0f;
    private Vector2 NoteBoxEndPoint = Vector2.zero;
    private Queue<Image> NoteQueue = null; 
    private void Awake()
    {
        Hitbox = GetComponentInChildren<Image>();
        HitboxWidth = Hitbox.rectTransform.sizeDelta.x; 

        Notebox = Hitbox.GetComponentsInChildren<Image>()[1];
        NoteboxStartPoint = Notebox.rectTransform.anchoredPosition; // 노트박스가 화면으로 나오기 전 위치
        NoteBoxEndPoint = Hitbox.rectTransform.anchoredPosition + new Vector2(-HitboxWidth, 0f); // 노트박스가 화면 밖에 나올 위치
        NoteQueue = new Queue<Image>();

    }
    #region private
    private void MoveNoteToHit()
    {

        Image noteGo = Instantiate(Notebox, Hitbox.transform);
        NoteQueue.Enqueue(noteGo);
        StartCoroutine(MoveNoteToHitCoroutine(noteGo));
    }
    
    private IEnumerator MoveNoteToHitCoroutine(Image _Notebox)
    {
        //Notebox.rectTransform.anchoredPosition = Hitbox.rectTransform.anchoredPosition;
        Notebox.gameObject.SetActive(true);
        float time = 0f;
        while (time < 1f)
        {
            _Notebox.rectTransform.anchoredPosition = Vector3.Lerp(NoteboxStartPoint, NoteBoxEndPoint, time);
            if (time > 1f)
            {
                time = 1f;
            }
            Stoptiming = time;
            time += Time.deltaTime;
            yield return null;
        }
        _Notebox.rectTransform.anchoredPosition = NoteBoxEndPoint;
        HitNote();
    }
    #endregion
    #region public
    public void HitNote()
    {
        //Notebox.gameObject.SetActive(false);
        if (NoteQueue.Count > 0)
        {
            Image go = null;
            if (NoteQueue.TryDequeue(out go))
                Destroy(go.gameObject);
        }
    }

    public void PushNote()
    {
        MoveNoteToHit();
    }
    #endregion
}
