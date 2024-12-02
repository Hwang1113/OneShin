using UnityEngine;


public class FourByFour : MonoBehaviour
{
    [SerializeField]
    private ONEShin_UINoteManager UINMg = null;

    private float Bpm = 0f; //60bpm 1�п� 60��, 1�� 1��

    private void Awake()
    {
        UINMg = GetComponentInChildren<ONEShin_UINoteManager>();
    }
    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // 
            UINMg.HitQNote();
        if (Input.GetKeyDown(KeyCode.W)) // 
            UINMg.HitQNote();
        if (Input.GetKeyDown(KeyCode.E)) // 
            UINMg.HitQNote();
        if (Input.GetKeyDown(KeyCode.R)) // 
            UINMg.HitQNote();
        if (Input.GetKeyDown(KeyCode.P))
            UINMg.PushNote();
    }
}
