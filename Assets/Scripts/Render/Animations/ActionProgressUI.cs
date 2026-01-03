using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ActionProgressUI : MonoBehaviour
{
    [SerializeField] TMP_Text gathered;
    [SerializeField] Image fill;

    void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetProgress(float t)
    {
        Show();
        fill.fillAmount = t;
        gathered.text = t.ToString();
    }


}
