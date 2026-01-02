using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ActionProgressUI : MonoBehaviour
{
    TMP_Text gathered;
    Image fill;
    float progress;

    void Awake()
    {
        gathered = GetComponent<TMP_Text>();
        fill = GetComponent<Image>();
    }
    void Show()
    {
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }

    void SetProgress(float t)
    {
        fill.fillAmount = t;
    }


}
