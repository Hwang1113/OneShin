
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

    public float displayDuration = 0.7f; // 판정 이미지 화면에 표시되는 시간
    public float trashDisplayDuration = 2f; // 쓰레기 이미지 화면에 표시되는 시간

    private bool isGameOver = false; // 게임 오버 상태 체크
    private GameObject activeTrash; // 현재 활성화된 쓰레기 이미지

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
        // 쓰레기 이미지들을 비활성화 상태로 시작
        foreach (GameObject trashUI in trashUIs)
        {
            if (trashUI != null)
            {
                trashUI.SetActive(false);
            }
        }

        // 메인 댄서 이미지 즉시 표시
        ShowMainDancerUI();

        /*
        // 게임 시작 후 1초 뒤 메인 댄서 이미지 Fade In
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
        if (isGameOver) return; // 게임 오버 상태라면 판정UI 표시하지 않음
    
        ResetJudgeUI(); // 기존 UI 초기화 (쓰레기 이미지는 초기화하지 않음)

        switch (result)
        {
            case "Perfect":
                perfectUI.SetActive(true);
                perfectUI.GetComponent<UIFader>().FadeInUI();
                Invoke("HidePerfectUI", displayDuration); // 일정 시간 후 FadeOut 호출

                // Perfect 판정 시 FX 호출
                FxManager.Instance.PlayPerfectEffect(Vector3.zero);

                break;

            case "Good":
                goodUI.SetActive(true);
                goodUI.GetComponent<UIFader>().FadeInUI();
                Invoke("HideGoodUI", displayDuration);

                // Good 판정 시 FX 호출
                FxManager.Instance.PlayGoodEffect(Vector3.zero);

                break;
            case "Miss":
                missUI.SetActive(true);
                missUI.GetComponent<UIFader>().FadeInUI();
                Invoke("HideMissUI", displayDuration);
                ShowRandomTrashUI(); // 쓰레기 이미지 표시

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

        // 랜덤으로 하나의 쓰레기 이미지 선택
        int randomIndex = Random.Range(0, trashUIs.Length);
        GameObject selectedTrash = trashUIs[randomIndex];


        // 배열 할당 요소 비었음
        if (selectedTrash == null)
        {
            Debug.LogError("Selected trashUI is null!");
            return;
        }

        // 새로 선택된 쓰레기 이미지를 activeTrash에 할당
        activeTrash = selectedTrash;

        // 화면 내 랜덤 위치 설정 (19020x1080 기준)
        RectTransform rectTransform = activeTrash.GetComponent<RectTransform>();

        // 선택된 쓰레기 이미지에 recttransform 없음
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform component not found on selectedTrash!");
            return;
        }


        rectTransform.anchoredPosition = new Vector2(
            Random.Range(-960f, 960f), // 1920px/2  
            Random.Range(-540f, 540f) //  1080px/2  
        );

        // 쓰레기 UI 활성화
        activeTrash.SetActive(true);

        // 2초 후에 Fade Out
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
        CancelInvoke(); // 이전 호출 초기화

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
        // 메인 댄서 비활성화
        if (mainDancerUI != null && mainDancerUI.activeSelf)
        {
            Debug.Log("Disabling Main Dancer UI: " + mainDancerUI.name);
            mainDancerUI.SetActive(false);
        }

        // 서브 댄서 비활성화
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