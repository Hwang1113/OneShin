using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ONEShin_UINoteManager : MonoBehaviour
{
    private Image Hitbox = null;
    private Image Notebox = null;
    private float time = 0f;
    private Vector2 NoteboxStartPoint = Vector2.zero;
    private float HitboxWidth = 0f;
    private Vector2 NoteBoxEndPoint = Vector2.zero;

    private void Awake()
    {
        Hitbox = GetComponentInChildren<Image>();
        HitboxWidth = Hitbox.rectTransform.sizeDelta.x; 

        Notebox = Hitbox.GetComponentsInChildren<Image>()[1];
        NoteboxStartPoint = Notebox.rectTransform.anchoredPosition; // 노트박스가 화면으로 나오기 전 위치
        NoteBoxEndPoint = Hitbox.rectTransform.anchoredPosition + new Vector2(-HitboxWidth, 0f); // 노트박스가 화면 밖에 나올 위치
    }
    #region private
    private void MoveNoteToHit()
    {
        StartCoroutine(MoveNoteToHitCoroutine());
    }
    
    private IEnumerator MoveNoteToHitCoroutine()
    {
        //Notebox.rectTransform.anchoredPosition = Hitbox.rectTransform.anchoredPosition;
        time = 0f;
        while (true)
        {
            Notebox.rectTransform.anchoredPosition = Vector3.Lerp(NoteboxStartPoint, NoteBoxEndPoint, time);
            time += Time.deltaTime;
            Debug.Log(NoteboxStartPoint + "" + NoteBoxEndPoint + "" + time);
            if (time >= 1f) 
                break;
            yield return null;
        }

    }
    #endregion
    #region public
    public void HitNote()
    {
        Notebox.gameObject.SetActive(false);
        StopCoroutine(MoveNoteToHitCoroutine());
        Debug.Log("멈춘time" + time);
    }

    public void PushNote()
    {
        Notebox.rectTransform.anchoredPosition = NoteboxStartPoint;
        time = 0f;
        Notebox.gameObject.SetActive(true);
        MoveNoteToHit();
    }
    #endregion

}
