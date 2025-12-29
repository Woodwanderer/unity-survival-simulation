using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;
using System; // for Action
using TMPro;

public class ContextABButton : MonoBehaviour
{
    Image image;
    Button button;
    TMP_Text amount;


    private void Awake()
    {
        image = transform.Find("BtnSprite").GetComponent<Image>();
        button = GetComponent<Button>();
        amount = transform.Find("Amount").GetComponent<TMP_Text>();
    }

    public void SetIcon(Sprite icon)
    {
        image.sprite = icon;
    }
    public void SetAmount(string amount)
    {
        this.amount.text = amount;
    }
    public void SetAction(Action action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => action());
    }

}
