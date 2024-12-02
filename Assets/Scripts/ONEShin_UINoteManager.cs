using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class ONEShin_UINoteManager : MonoBehaviour
{
    private Image Hitbox = null;
    private Image Hitboxes = null;
    private Image Notebox = null;
    private float Stoptiming = 0f;
    private Vector2 NoteboxStartPoint = Vector2.zero;
    private float HitboxWidth = 0f;
    private float HitboxHeight = 0f;
    private Vector2 NoteBoxEndPoint = Vector2.zero;
    private Queue<Image> NoteQueue = null; 

    private void Awake()
    {
        Hitbox = GetComponentInChildren<Image>(); 
        HitboxWidth = Hitbox.rectTransform.sizeDelta.x; 
        HitboxHeight = Hitbox.rectTransform.sizeDelta.y; 


        Notebox = Hitbox.GetComponentsInChildren<Image>()[1];
        NoteboxStartPoint = Notebox.rectTransform.anchoredPosition; // ��Ʈ�ڽ��� ȭ������ ������ �� ��ġ (��������)
        NoteBoxEndPoint = Hitbox.rectTransform.anchoredPosition + new Vector2(-HitboxWidth , HitboxHeight); // ��Ʈ�ڽ��� ȭ�� �ۿ� ���� ��ġ (��������)

        NoteQueue = new Queue<Image>(); //��Ʈ ���� ť ����Ʈ

    }
    #region private
    private void CreateMoveQNoteToHit()  //////// ��Ʈ�� �����ϰ�, ���� ����(NoteboxStartPoint) ���� ��Ʈ�ڽ� ��(NoteBoxEndPoint) ���� �����δ�.
    {

        Image noteGo = Instantiate(Notebox, Hitbox.transform);
        NoteQueue.Enqueue(noteGo);
        StartCoroutine(CreateMoveQNoteToHitCoroutine(noteGo));
    }
    
    private IEnumerator CreateMoveQNoteToHitCoroutine(Image _Notebox)
    {
        //Notebox.rectTransform.anchoredPosition = Hitbox.rectTransform.anchoredPosition;
        _Notebox.gameObject.SetActive(true);
        Stoptiming = 0f;
        float time = 0f;
        while (time < 1f && _Notebox != null)
        {
            if (_Notebox != null)
            {
                _Notebox.rectTransform.anchoredPosition = Vector3.Lerp(NoteboxStartPoint, Hitbox.rectTransform.anchoredPosition, time * 10 / 8f);

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
            Stoptiming = 0.5f;
            _Notebox.rectTransform.anchoredPosition = NoteBoxEndPoint;
            HitQNote();
        }
        Stoptiming = 0f;
    }
    #endregion
    #region public
    public void HitQNote()
    {
        //Notebox.gameObject.SetActive(false);
        if (Stoptiming >= 0.5f)
        {
            if (NoteQueue.Count > 0 && NoteQueue != null) // ��Ʈ������ 0�ʰ��Ӱ� ���ÿ� null �� �ƴϸ� ��Ʈ ���� 
            {
                Image go = null;
                if (NoteQueue.TryDequeue(out go))
                    Destroy(go.gameObject);
            }
            if (NoteQueue.Count >= 0)
                if(0.5 <= Stoptiming &&   Stoptiming < 0.6)
                    Debug.Log(Stoptiming + "Bad");
                else if (0.6 <= Stoptiming && Stoptiming < 0.75)
                    Debug.Log(Stoptiming + "Good");                
                else if (0.75 <= Stoptiming && Stoptiming < 0.85 )
                    Debug.Log(Stoptiming + "Perfect");
                else if (0.85 <= Stoptiming && Stoptiming < 1 )
                    Debug.Log(Stoptiming + "Good");

            //�̻� ~ �̸� ������ 
            // 0.5���ϴ� ������ , Bad 0.5 ~ 0.6, Good 0.6 ~ 0.75, Perfect 0.75 ~ 0.85, Good 0.85 ~ 1   1�̻��� �Ǹ� 0.5�� �ڵ� ġȯ �׷��� Bad ������ ��Ʈ �����

        }

        //Stoptiming �� ������ ǥ���ϸ� ���� ������?
    }

    public void PushNote() //��Ʈ ����
    {
        CreateMoveQNoteToHit();
    }

    #endregion
}
