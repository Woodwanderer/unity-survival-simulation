using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    InventorySlot[] slots;

    private void Awake()
    {
        gameObject.SetActive(false);
        slots = GetComponentsInChildren<InventorySlot>();
    }

    public void AddToInv(ResourceEntry entry)
    {
        AddToInv(entry.item, entry.amount);
    }
    public void AddToInv(ItemType type, int amount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.type == type)
            {
                slot.IncreaseAmount(amount);
                return;
            }
            if (slot.type == ItemType.None)
            {
                slot.Set(type, amount);
                return;
            }
        }
    }
    public void RemoveEntry(ResourceEntry entry)
    {
        foreach (InventorySlot slot in slots)
        {
            if(slot.type == entry.item)
            {
                slot.DecreaseAmount(entry.amount);
                return;
            }
        }
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
