using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    InventorySlot[] slots;
    VirtualResources resources;
    void Awake()
    {
        gameObject.SetActive(false);
        slots = GetComponentsInChildren<InventorySlot>();
    }
    public void Init(VirtualResources resources)
    {
        this.resources = resources;
    }
    private void Update()
    {
        if(!gameObject.activeSelf)
            return;
        Refresh();
    }
    void Refresh()
    {
        if (resources == null) 
            return;

        int i = 0;
        foreach (var entry in resources.All())
        {
            slots[i].Set(entry.Key, entry.Value);
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
