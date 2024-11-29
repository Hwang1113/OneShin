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
        Debug.Log("����time" + time);
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
