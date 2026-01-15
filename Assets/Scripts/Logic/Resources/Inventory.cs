using System.Collections.Generic;
public class Inventory : IItemContainer
{
    List<ItemSlot> slots;
    public IEnumerable<ItemSlot> Slots => slots;
    public Inventory(int slotCount)
    {
        slots = new();
        for(int i = 0; i < slotCount; i++)
        {
            slots.Add(new ItemSlot());
        }
    }

    public int Add(ItemDefinition item, int amount)
    {
        int remaining = amount;

        foreach (ItemSlot slot in slots) 
        {
            if (!slot.IsEmpty && slot.Item == item && !slot.IsFull)
            {
                remaining = slot.Add(item, remaining);
                if (remaining <= 0)
                    return 0;
            }
        }
        foreach (ItemSlot slot in slots) 
        {
            if (slot.IsEmpty)
            {
                remaining = slot.Add(item, remaining);
                if (remaining <= 0)
                    return 0;
            }
        }
        return remaining; // overflow
    }
    public int Remove(ItemDefinition item, int amount)
    {
        int remaining = amount;
        foreach (ItemSlot slot in slots) 
        {
            if (slot.Item != item || slot.IsEmpty)
                continue;

            remaining = slot.Remove(remaining);

            if (remaining <= 0)
                return 0;
        }

        return remaining;
    }
    public VirtualResources Snapshot() => new VirtualResources(slots);
    public float CalculateWeight(ItemDefinition additionalItem = null)
    {
        float weight = 0;
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
                continue;

            weight += slot.Item.weight * slot.Amount;
        }

        if (additionalItem != null)
            weight += additionalItem.weight;

        return weight;
    }
    public bool IsEmpty
    {
        get
        {
            foreach (var slot in slots)
                if (!slot.IsEmpty) 
                    return false;
            return true;
        }
    }

}
