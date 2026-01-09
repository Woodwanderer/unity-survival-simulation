using UnityEngine;
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
    public void Set(Sprite icon, string amount, Action action)
    {
        SetIcon(icon);
        SetAmount(amount);
        SetAction(action);
    }
    public void SetIcon(Sprite icon)
    {
        image.sprite = icon;
        image.enabled = true;
    }
    public void SetAmount(string amount)
    {
        this.amount.text = amount;
        this.amount.enabled = true;
    }
    public void SetAction(Action action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => action());
    }
    public void Clear()
    {
        image.sprite = null;
        image.enabled = false;
        amount.text = "";
        amount.enabled = false;
        button.onClick.RemoveAllListeners();
    }

}
