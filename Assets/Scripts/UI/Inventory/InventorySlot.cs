using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text amountText;

    public void Set(ItemDefinition item,  int amount)
    {
        icon.sprite = item.icon;
        icon.enabled = true;
        amountText.text = amount.ToString();
        amountText.enabled = true;
    }
    public void Clear()
    {
        icon.enabled = false;
        amountText.enabled = false; icon.sprite = null;
    }
}
