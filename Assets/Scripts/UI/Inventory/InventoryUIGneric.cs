using UnityEngine;
using System.Collections.Generic;
//Not a fixed size inventory: stockpiles
public class InventoryUIGneric : MonoBehaviour 
{
    List<InventoryUISlot> slotsUI = new();
    public GameObject inventorySlot;

    IItemContainer invData;
    public bool IsInit => invData != null;
    
    void Awake()
    {
        gameObject.SetActive(false);
    }
    public void Init(IItemContainer inv)
    {
        this.invData = inv;

        //slots
        for (int i = 0; i < invData.Capacity; i++) 
        {
            GameObject slotObj = Instantiate(inventorySlot, transform);
            InventoryUISlot slotUI = slotObj.GetComponent<InventoryUISlot>();
            slotsUI.Add(slotUI);
        }
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
    public void Show()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            Refresh();
        }
        else
            gameObject.SetActive(false);
    }
}
