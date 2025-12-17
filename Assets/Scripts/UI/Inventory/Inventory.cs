using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    InventorySlot[] slots;
    
    private void Awake()
    {
        gameObject.SetActive(false);
        slots = GetComponentsInChildren<InventorySlot>();
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
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        EventBus.OnItemHarvest += AddToInv;
    }
    private void OnDisable()
    {
        EventBus.OnItemHarvest -= AddToInv;
    }
}
