using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;

public class ContextABButton : MonoBehaviour
{
    Image image;

    private void Awake()
    {
        image = transform.Find("BtnSprite").GetComponent<Image>();
    }

    public void SetIcon(Sprite icon)
    {
        image.sprite = icon;
        
        
    }
    public void harvest()
    {
        Button button = GetComponent<Button>();
    }

}
