using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ONEShin_UINoteManager : MonoBehaviour
{
    private Image Hitbox = null; // ���� R ��Ʈ�ڽ��� ���� �̹���
    private Image[] Hitboxes = new Image[3]; // ��Ʈ�ڽ��� ���� �̹��� �� 4��
    private Image Notebox = null; // ���� R ��Ʈ�ڽ��� ���� �̹��� 
    private Image[] Noteboxes = null; // ��Ʈ�ڽ��� ���� �̹��� �� 4��
    private float[] Stoptiming = new float[3]; //��Ʈ�� ��Ʈ ������ ������ ���� �迭 �� 4��
    private Vector2[] NoteboxStartPoint = new Vector2[3]; //���� ��Ʈ ���������� ���� �迭 �� 4��
    private float HitboxWidth = 0f;
    private float HitboxHeight = 0f;
    private Vector2[] NoteBoxEndPoint = new Vector2[3]; //���� ��Ʈ ������������ ���� �迭 �� 4��
    private Queue<Image> NoteQueue = new Queue<Image>();
    private Queue<Image>[] NoteQueues = new Queue<Image>[3]; // ť ��Ʈ����Ʈ�� ���� �迭 �� 4��
    private GameObject[] HitandNoteBoxes = new GameObject[3]; // ��Ʈ�ڽ��� ��Ʈ�ڽ��� ���� (��)���� ������Ʈ 4��

    private void Awake()
    {
        Hitbox = GetComponentInChildren<Image>();
        Hitboxes[0] = Hitbox;
        HitboxWidth = Hitboxes[0].rectTransform.sizeDelta.x; 
        HitboxHeight = Hitboxes[0].rectTransform.sizeDelta.y; 


        Notebox = Hitbox.GetComponentsInChildren<Image>()[1];
        //Noteboxes[0] = Notebox;
        NoteboxStartPoint[0] = Notebox.rectTransform.anchoredPosition; // ��Ʈ�ڽ��� ȭ������ ������ �� ��ġ (��������)
        NoteBoxEndPoint[0] = Hitboxes[0].rectTransform.anchoredPosition + new Vector2(-HitboxWidth , HitboxHeight); // ��Ʈ�ڽ��� ȭ�� �ۿ� ���� ��ġ (��������)
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
            if (NoteQueue.Count > 0 && NoteQueue != null) // ��Ʈ������ 0�ʰ��Ӱ� ���ÿ� null �� �ƴϸ� ��Ʈ ���� 
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

            //�̻� ~ �̸� ������ 
            // 0.5���ϴ� ������ , Bad 0.5 ~ 0.6, Good 0.6 ~ 0.75, Perfect 0.75 ~ 0.85, Good 0.85 ~ 1   1�̻��� �Ǹ� 0.5�� �ڵ� ġȯ �׷��� Bad ������ ��Ʈ �����

        }

        //Stoptiming[0] �� ������ ǥ���ϸ� ���� ������?
    }

    public void PushNoteR() //��Ʈ ����
    {
        CreateMoveNoteRToHit();
    }

    #endregion
}
