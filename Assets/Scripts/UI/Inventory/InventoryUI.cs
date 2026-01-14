using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    InventorySlot[] slots;
    Inventory Inventory;
    void Awake()
    {
        gameObject.SetActive(false);
        slots = GetComponentsInChildren<InventorySlot>();
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
            slots[i].Set(slot.Item, slot.Amount);
            i++;
        }

        for (; i < slots.Length; i++)
            slots[i].Clear();
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
