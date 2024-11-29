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
    public void MoveNoteToHit()
    {
        //Notebox.rectTransform.anchoredPosition = Hitbox.rectTransform.anchoredPosition;
        Notebox.rectTransform.anchoredPosition = Vector3.Lerp(NoteboxStartPoint, NoteBoxEndPoint, time);
        time += Time.deltaTime;
        if (time > 1)
            return; 
        Debug.Log(NoteboxStartPoint + "" + Hitbox.rectTransform.anchoredPosition);
    }
    public void HitNote()
    {
        Notebox.gameObject.SetActive(false);
    }
    public void PushNote()
    {
        Notebox.rectTransform.anchoredPosition = NoteboxStartPoint;
        time = 0f;
        Notebox.gameObject.SetActive(true);
        MoveNoteToHit();
    }

}
