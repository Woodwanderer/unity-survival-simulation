using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
public class BuildingBarButton : MonoBehaviour
{
    Image image;
    Button button;
    TMP_Text bottomText;
    TMP_Text topText;
    private void Awake()
    {
        button = GetComponent<Button>();
        image = transform.Find("BtnSprite").GetComponent<Image>();
        bottomText = transform.Find("BottomText").GetComponent<TMP_Text>();
        topText = transform.Find("TopText").GetComponent<TMP_Text>();
    }
    public void Set(Sprite icon, string bottomText, Action action)
    {
        SetIcon(icon);
        SetBottomText(bottomText);
        SetAction(action);
    }
    public void SetIcon(Sprite icon)
    {
        image.sprite = icon;
        image.enabled = true;
    }
    public void SetTopText(string text)
    {
        this.topText.text = text;
        this.topText.enabled = true;
    }
    public void SetBottomText(string bottomText)
    {
        this.bottomText.text = bottomText;
        this.bottomText.enabled = true;
    }
    public void SetAction(Action action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => action());
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    public void Clear()
    {
        image.sprite = null;
        image.enabled = false;
        bottomText.text = "";
        bottomText.enabled = false;
        button.onClick.RemoveAllListeners();
    }
    public void Show(bool active)
    {
        gameObject.SetActive(active);
    }

}
