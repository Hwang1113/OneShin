using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FourByFour : MonoBehaviour
{
    [SerializeField]
    private ONEShin_UIManager UIMg = null;

    private void Awake()
    {
        UIMg = GetComponentInChildren<ONEShin_UIManager>();
    }
    private void Update()
    {
        UIMg.MoveNoteToHit();
    }


}
