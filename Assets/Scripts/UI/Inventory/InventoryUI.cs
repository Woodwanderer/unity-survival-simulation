using UnityEngine;

//Class used by InventoryUI for: protagonist. Inventory with fixed size
public class InventoryUI : MonoBehaviour
{
    InventoryUISlot[] slotsUI;
    Inventory Inventory;
    void Awake()
    {
        gameObject.SetActive(false);
        slotsUI = GetComponentsInChildren<InventoryUISlot>();
    }
    public void Init(Inventory inventory)
    {
        this.Inventory = inventory;
    }
    private void Update()
    {
        if(!gameObject.activeSelf)
            return;

        Refresh();
    }
    void Refresh()
    {
        if (Inventory == null) 
            return;

        int i = 0;
        foreach (var slot in Inventory.Slots)
        {
            if (slot.IsEmpty)
            {
                slotsUI[i].Clear();
            }
            else
            {
                slotsUI[i].Set(slot.Item, slot.Amount);
            }
            i++;
        }

        for (; i < slotsUI.Length; i++)
            slotsUI[i].Clear();
    }
    public void Show()
    {
        Refresh();
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
