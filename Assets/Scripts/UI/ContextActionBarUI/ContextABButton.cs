using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;
using System; // for Action

public class ContextABButton : MonoBehaviour
{
    Image image;
    Button button;

    private void Awake()
    {
        image = transform.Find("BtnSprite").GetComponent<Image>();
        button = GetComponent<Button>();
    }

    public void SetIcon(Sprite icon)
    {
        image.sprite = icon;
        
        
    }
    public void SetAction(Action action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => action());
    }

}
