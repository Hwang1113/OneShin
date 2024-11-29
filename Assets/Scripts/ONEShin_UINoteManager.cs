using System.Collections;
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

    private void Awake()
    {
        Hitbox = GetComponentInChildren<Image>();
        HitboxWidth = Hitbox.rectTransform.sizeDelta.x; 

        Notebox = Hitbox.GetComponentsInChildren<Image>()[1];
        NoteboxStartPoint = Notebox.rectTransform.anchoredPosition; // ��Ʈ�ڽ��� ȭ������ ������ �� ��ġ
        NoteBoxEndPoint = Hitbox.rectTransform.anchoredPosition + new Vector2(-HitboxWidth, 0f); // ��Ʈ�ڽ��� ȭ�� �ۿ� ���� ��ġ
    }
    #region private
    private void MoveNoteToHit()
    {
        StartCoroutine(MoveNoteToHitCoroutine());
    }
    
    private IEnumerator MoveNoteToHitCoroutine()
    {
        //Notebox.rectTransform.anchoredPosition = Hitbox.rectTransform.anchoredPosition;
        Notebox.gameObject.SetActive(true);
        float time = 0f;
        while (time < 1f)
        {
            Notebox.rectTransform.anchoredPosition = Vector3.Lerp(NoteboxStartPoint, NoteBoxEndPoint, time);
            if (time > 1f)
            {
                time = 1f;
            }
            Stoptiming = time;
            time += Time.deltaTime;
            yield return null;
        }
        Notebox.rectTransform.anchoredPosition = NoteBoxEndPoint;
    }
    #endregion
    #region public
    public void HitNote()
    {
        Notebox.gameObject.SetActive(false);

        Debug.Log("����time" + Stoptiming);
    }

    public void PushNote()
    {

        MoveNoteToHit();
    }
    #endregion
}
