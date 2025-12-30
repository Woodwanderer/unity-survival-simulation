using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text amountText;
    public ItemIcons configIcons;

    public void Set(ItemType type,  int amount)
    {
        icon.sprite = configIcons.GetIcon(type);
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
