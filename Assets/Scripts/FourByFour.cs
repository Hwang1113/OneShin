using UnityEngine;


public class FourByFour : MonoBehaviour
{
    [SerializeField]
    private ONEShin_UINoteManager UINMg = null;

    private float Bpm = 0f; //60bpm 1분에 60번, 1초 1번

    private void Awake()
    {
        UINMg = GetComponentInChildren<ONEShin_UINoteManager>();
    }
    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
            UINMg.HitNoteR();
        if (Input.GetKeyDown(KeyCode.P))
            UINMg.PushNoteR();
    }
}
