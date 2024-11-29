using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FourByFour : MonoBehaviour
{
    [SerializeField]
    private ONEShin_UINoteManager UIMg = null;

    private float Bpm = 0f; //60bpm 1분에 60번, 1초 1번

    
    private void Awake()
    {
        UIMg = GetComponentInChildren<ONEShin_UINoteManager>();
    }
    private void Start()
    {

    }

    private void Update()
    {
        UIMg.MoveNoteToHit();
    }
}
