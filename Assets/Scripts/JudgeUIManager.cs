
using UnityEngine;
using Unity.UI;
using System.Collections;

public class JudgeUIManager : MonoBehaviour
{
    public GameObject perfectUI;
    public GameObject goodUI;
    public GameObject missUI;
    public GameObject mainDancerUI;

    public GameObject[] trashUIs;
    public GameObject[] subDancerUIs;

    public float displayDuration = 0.7f; // ���� �̹��� ȭ�鿡 ǥ�õǴ� �ð�
    public float trashDisplayDuration = 2f; // ������ �̹��� ȭ�鿡 ǥ�õǴ� �ð�

    private bool isGameOver = false; // ���� ���� ���� üũ
    private GameObject activeTrash; // ���� Ȱ��ȭ�� ������ �̹���

    public FxManager fxManager;

    public static JudgeUIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // ������ �̹������� ��Ȱ��ȭ ���·� ����
        foreach (GameObject trashUI in trashUIs)
        {
            if (trashUI != null)
            {
                trashUI.SetActive(false);
            }
        }

        // ���� �� �̹��� ��� ǥ��
        ShowMainDancerUI();

        /*
        // ���� ���� �� 1�� �� ���� �� �̹��� Fade In
        if (mainDancerUI != null)
        {
            mainDancerUI.SetActive(false);
            Invoke(nameof(ShowMainDancerUI), 1f);
        } */
    }

    private void ShowMainDancerUI()
    {
        if (mainDancerUI != null)
        {
            mainDancerUI.SetActive(true);
            Debug.Log("Main Dancer UI Activated: " + mainDancerUI.name);

            UIFader fader = mainDancerUI.GetComponent<UIFader>();
            if (fader != null)
            {
                fader.FadeInUI();
            }
        }
        else
        {
            Debug.LogError("mainDancerUI is not assigned!");
        }
    }


    public void ShowJudgeUI(string result)
    {
        if (isGameOver) return; // ���� ���� ���¶�� ����UI ǥ������ ����
    
        ResetJudgeUI(); // ���� UI �ʱ�ȭ (������ �̹����� �ʱ�ȭ���� ����)

        switch (result)
        {
            case "Perfect":
                perfectUI.SetActive(true);
                perfectUI.GetComponent<UIFader>().FadeInUI();
                Invoke("HidePerfectUI", displayDuration); // ���� �ð� �� FadeOut ȣ��

                // Perfect ���� �� FX ȣ��
                FxManager.Instance.PlayPerfectEffect(Vector3.zero);

                break;

            case "Good":
                goodUI.SetActive(true);
                goodUI.GetComponent<UIFader>().FadeInUI();
                Invoke("HideGoodUI", displayDuration);

                // Good ���� �� FX ȣ��
                FxManager.Instance.PlayGoodEffect(Vector3.zero);

                break;
            case "Miss":
                missUI.SetActive(true);
                missUI.GetComponent<UIFader>().FadeInUI();
                Invoke("HideMissUI", displayDuration);
                ShowRandomTrashUI(); // ������ �̹��� ǥ��

                break;
        }
    }

    public void ShowSubDancerUI(int score)
    {
        if (score >= 900 && !subDancerUIs[0].activeSelf)
            subDancerUIs[0].SetActive(true);
        if (score >= 1200 && !subDancerUIs[1].activeSelf)
            subDancerUIs[1].SetActive(true);
        if (score >= 1500 && !subDancerUIs[2].activeSelf)
            subDancerUIs[2].SetActive(true);
        if (score >= 2000 && !subDancerUIs[3].activeSelf)
            subDancerUIs[3].SetActive(true);
    }



    private void ShowRandomTrashUI()
    {
        if (trashUIs.Length == 0 || isGameOver) return;

        // �������� �ϳ��� ������ �̹��� ����
        int randomIndex = Random.Range(0, trashUIs.Length);
        GameObject selectedTrash = trashUIs[randomIndex];


        // �迭 �Ҵ� ��� �����
        if (selectedTrash == null)
        {
            Debug.LogError("Selected trashUI is null!");
            return;
        }

        // ���� ���õ� ������ �̹����� activeTrash�� �Ҵ�
        activeTrash = selectedTrash;

        // ȭ�� �� ���� ��ġ ���� (19020x1080 ����)
        RectTransform rectTransform = activeTrash.GetComponent<RectTransform>();

        // ���õ� ������ �̹����� recttransform ����
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform component not found on selectedTrash!");
            return;
        }


        rectTransform.anchoredPosition = new Vector2(
            Random.Range(-960f, 960f), // 1920px/2  
            Random.Range(-540f, 540f) //  1080px/2  
        );

        // ������ UI Ȱ��ȭ
        activeTrash.SetActive(true);

        // 2�� �Ŀ� Fade Out
        StartCoroutine(HideTrashUIWithDelay(activeTrash));
    }

    private IEnumerator HideTrashUIWithDelay(GameObject trash)
    {
        yield return new WaitForSeconds(trashDisplayDuration);

        if (!isGameOver && trash.activeSelf)
        {
            UIFader fader = trash.GetComponent<UIFader>();
            if (fader != null)
            {
                fader.FadeOutUI();
                yield return new WaitForSeconds(fader.fadeDuration);
            }

            trash.SetActive(false);
            Debug.Log("Trash UI Deactivated: " + trash.name);
        }
    }


    private void ResetJudgeUI()
    {
        CancelInvoke(); // ���� ȣ�� �ʱ�ȭ

        perfectUI.SetActive(false);
        goodUI.SetActive(false);
        missUI.SetActive(false);


    }

    private void HidePerfectUI()
    {
        perfectUI.GetComponent<UIFader>().FadeOutUI();
    }

    private void HideGoodUI()
    {
        goodUI.GetComponent<UIFader>().FadeOutUI();
    }

    private void HideMissUI()
    {
        missUI.GetComponent<UIFader>().FadeOutUI();
    }

    public void OnGameClear()
    {
        isGameOver = true;
        DisableAllTrashUIs();
        DisableAllDancerUIs();
    }


    public void OnGameOver()
    {
        isGameOver = true;
        //Debug.Log("Game Over Triggered");

        DisableAllTrashUIs();
        DisableAllDancerUIs();
    }


    private void DisableAllTrashUIs()
    {
        foreach (GameObject trashUI in trashUIs)
        {
            if (trashUI.activeSelf)
            {
                UIFader fader = trashUI.GetComponent<UIFader>();
                if (fader != null)
                {
                    fader.FadeOutUI();
                }
                else
                {
                    trashUI.SetActive(false);
                }
            }
        }
        activeTrash = null;
    }

    private void DisableAllDancerUIs()
    {
        // ���� �� ��Ȱ��ȭ
        if (mainDancerUI != null && mainDancerUI.activeSelf)
        {
            Debug.Log("Disabling Main Dancer UI: " + mainDancerUI.name);
            mainDancerUI.SetActive(false);
        }

        // ���� �� ��Ȱ��ȭ
        foreach (GameObject dancer in subDancerUIs)
        {
            if (dancer != null && dancer.activeSelf)
            {
                Debug.Log("Disabling Sub Dancer UI: " + dancer.name);
                dancer.SetActive(false);
            }
        }
    }

}