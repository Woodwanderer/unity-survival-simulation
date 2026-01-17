using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUISlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text amountText;
    bool IsInit => icon.sprite != null && amountText.enabled;
    public void Set(ItemDefinition item,  int amount)
    {
        icon.sprite = item.icon;
        icon.enabled = true;
        amountText.text = amount.ToString();
        amountText.enabled = true;
    }
    public void Clear()
    {
        if (!IsInit)
            return;

        icon.enabled = false;
        amountText.enabled = false;
        icon.sprite = null;
    }
}
