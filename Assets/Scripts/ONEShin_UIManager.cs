using UnityEngine;
using UnityEngine.UI;

public class ONEShin_UIManager : MonoBehaviour
{
    private Image Hitbox = null;
    private Image Notebox = null;
    private float time = 0f; 
    private void Awake()
    {
        Hitbox = GetComponentInChildren<Image>();
        Notebox = Hitbox.GetComponentInChildren<Image>();
        if (Hitbox != null && Notebox != null)
            Debug.Log("Hit Note ºÒ·¯¿È");
    }
}
