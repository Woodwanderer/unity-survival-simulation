using UnityEngine;
using TMPro;

//Class used by InventoryUI for: protagonist. Inventory with fixed size
public class InventoryUI : MonoBehaviour
{
    InventoryUISlot[] slotsUI;
    TMP_Text weight;
    TMP_Text weightMax;

    Inventory inventory;
    CharacterSheet stats;
    void Awake()
    {
        gameObject.SetActive(false);
        slotsUI = GetComponentsInChildren<InventoryUISlot>();
        weight = transform.Find("CarryWeightValue").GetComponent<TMP_Text>();
        weightMax = transform.Find("MaxCarryWeightValue").GetComponent<TMP_Text>();
    }
    public void Init(Inventory inventory, CharacterSheet stats)
    {
        this.inventory = inventory;
        this.stats = stats;

    }
    private void Update()
    {
        if(!gameObject.activeSelf)
            return;

        Refresh();
    }
    void Refresh()
    {
        if (inventory == null) 
            return;

        SetWeightText();

        int i = 0;
        foreach (var slot in inventory.Slots)
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
    void SetWeightText()
    {
        weight.text = inventory.Weight.ToString();
        weightMax.text = stats.carryWeight.ToString();
    }
}
