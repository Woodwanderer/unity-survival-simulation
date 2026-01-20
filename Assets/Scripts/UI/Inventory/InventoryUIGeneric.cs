using UnityEngine;
using System.Collections.Generic;
//Not a fixed size inventory: stockpiles
public class InventoryUIGeneric : MonoBehaviour 
{
    List<InventoryUISlot> slotsUI = new();
    public GameObject inventorySlot;

    IItemContainer invData;
    int currentCapacity = -1;
    
    void Awake()
    {
        gameObject.SetActive(false);
    }
    public void Show(IItemContainer inv)
    {
        if (inv == null) 
            return;

        if (inv.Capacity != currentCapacity)
            Rebuild(inv.Capacity);

        this.invData = inv;
        gameObject.SetActive(true);
        Refresh();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Toggle(IItemContainer inv)
    {
        if (gameObject.activeSelf)
            Hide();
        else
            Show(inv);
    }
    public void Rebuild(int newCapacity)
    {
        foreach(InventoryUISlot slot in slotsUI)
            Destroy(slot.gameObject);

        slotsUI.Clear();

        for (int i = 0; i < newCapacity; i++)
        {
            GameObject slotObj = Instantiate(inventorySlot, transform);
            InventoryUISlot slotUI = slotObj.GetComponent<InventoryUISlot>();
            slotsUI.Add(slotUI);
        }
        currentCapacity = newCapacity;
    }
    private void Update()
    {
        if (!gameObject.activeSelf)
            return;

        Refresh();
    }
    void Refresh()
    {
        if (invData == null)
            return;

        int i = 0;
        foreach (var slot in invData.Slots)
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

        for (; i < invData.Capacity; i++)
            slotsUI[i].Clear();
    }
}
