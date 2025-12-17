using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text amount;

    public void Set(Sprite sprite_in,  int amount_in)
    {
        icon.sprite = sprite_in;
        amount.text = amount_in.ToString();
        amount.enabled = amount_in > 0;
    }
}
