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
        NoteboxStartPoint = Notebox.rectTransform.anchoredPosition; // ��Ʈ�ڽ��� ȭ������ ������ �� ��ġ
        NoteBoxEndPoint = Hitbox.rectTransform.anchoredPosition + new Vector2(-HitboxWidth, 0f); // ��Ʈ�ڽ��� ȭ�� �ۿ� ���� ��ġ

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
