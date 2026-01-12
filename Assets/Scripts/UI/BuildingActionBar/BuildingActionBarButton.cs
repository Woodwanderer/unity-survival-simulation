using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BuildingActionBarButton : MonoBehaviour
{
    Image image;
    Button button;
    TMP_Text lowerText;
    private void Awake()
    {
        button = GetComponent<Button>();
        image = transform.Find("BtnSprite").GetComponent<Image>();
        lowerText = transform.Find("LowerText").GetComponent<TMP_Text>();
    }
    public void Set(Sprite icon, string lowerText, Action action)
    {
        SetIcon(icon);
        SetLowerText(lowerText);
        SetAction(action);
    }
    public void SetIcon(Sprite icon)
    {
        image.sprite = icon;
        image.enabled = true;
    }
    public void SetLowerText(string lowerText)
    {
        this.lowerText.text = lowerText;
        this.lowerText.enabled = true;
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
        lowerText.text = "";
        lowerText.enabled = false;
        button.onClick.RemoveAllListeners();
    }
    public void Show(bool active)
    {
        gameObject.SetActive(active);
    }

}
