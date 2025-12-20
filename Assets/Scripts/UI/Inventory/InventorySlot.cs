using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text amountText;
    int amount = 0;
    public ItemType type { get; private set; }
    public ItemIcons configIcons;

    public void Set(ItemType type_in,  int amount_in)
    {
        this.type = type_in;
        icon.sprite = configIcons.GetIcon(type_in);
        IncreaseAmount(amount_in);
        icon.enabled = true;
    }
    public void IncreaseAmount(int  amount_in)
    {
        amount += amount_in;
        Refresh();
    }
    private void Refresh()
    {
        amountText.text = amount.ToString();
        if (amount > 0)
        {
            amountText.enabled = true;
            icon.enabled = true;
        }
    }
}
