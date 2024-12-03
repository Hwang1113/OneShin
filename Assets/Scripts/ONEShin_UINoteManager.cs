using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ONEShin_UINoteManager : MonoBehaviour
{
    private Image Hitbox = null; // ���� R ��Ʈ�ڽ��� ���� �̹���
    private Image Notebox = null; // ���� R ��Ʈ�ڽ��� ���� �̹��� 
    private float Stoptiming = 0f; // R ��Ʈ�� ��Ʈ ������ ������ ���� �迭 �� 4��
    private Vector2 NoteboxStartPoint = Vector2.zero; //���� R ��Ʈ ���������� 
    private float HitboxWidth = 0f;
    private float HitboxHeight = 0f;
    private Vector2 NoteBoxEndPoint = Vector2.zero; // R ��Ʈ ����������
    private Queue<Image> NoteQueue = new Queue<Image>(); // R��Ʈ ���� ť����Ʈ �̰ɷ� ��Ʈ ó����

    private void Awake()
    { 
        Hitbox = GetComponentInChildren<Image>(); // (this)ĵ���� �ڽ����� ����

        HitboxWidth = Hitbox.rectTransform.sizeDelta.x; 
        HitboxHeight = Hitbox.rectTransform.sizeDelta.y; 


        Notebox = Hitbox.GetComponentsInChildren<Image>()[1]; // ��Ʈ�ڽ� �ڽ����� ���� GetChildren�ϸ� ������ �ҷ������⿡ �̿� ���� ȣ��
        NoteboxStartPoint = Notebox.rectTransform.anchoredPosition; // ��Ʈ�ڽ��� ȭ������ ������ �� ��ġ (��������)
        NoteBoxEndPoint = Hitbox.rectTransform.anchoredPosition + new Vector2(-HitboxWidth , HitboxHeight); // ��Ʈ�ڽ��� ȭ�� �ۿ� ���� ��ġ (��������)
         //��Ʈ ���� ť ����Ʈ

    }
    #region private
    private void CreateMoveNoteRToHit()  //////// ��Ʈ�� �����ϰ�, ���� ����(NoteboxStartPoint[0]) ���� ��Ʈ�ڽ� ��(NoteBoxEndPoint) ���� �����δ�.
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
            if (NoteQueue.Count > 0 && NoteQueue != null) // ��Ʈ������ 0�ʰ��Ӱ� ���ÿ� null �� �ƴϸ� ��Ʈ ���� 
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

            //�̻� ~ �̸� ������ 
            // 0.4�̸��� ������ , Bad 0.4 ~ 0.6, Good 0.6 ~ 0.75, Perfect 0.75 ~ 0.85, Good 0.85 ~ 1   1�̻��� �Ǹ� 0.4�� �ڵ� ġȯ �׷��� Bad ������ ��Ʈ �����

        }

        //Stoptiming �� ������ ǥ���ϸ� ���� ������?
    }

    public void PushNoteR() //��Ʈ ����
    {
        CreateMoveNoteRToHit();
    }

    #endregion
}
