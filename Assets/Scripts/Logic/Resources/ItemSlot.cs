using UnityEngine;
using System;
public class ItemSlot
{
    public ItemDefinition Item { get; private set; }
    public int Amount { get; private set; }
    public bool IsEmpty => Amount <= 0;
    public bool IsFull => Amount >= Item.maxStockpileSize;
    public ItemSlot(ItemDefinition item = null, int amount = 0)
    {
        Item = item;
        Amount = amount;
    }
    public int FreeSpaceFor(ItemDefinition item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (IsEmpty)
            return item.maxStockpileSize;

        if (Item != item)
            return 0;

        return Item.maxStockpileSize - Amount;
    }
    public int Add(ItemDefinition item, int amount)
    {
        if (amount <= 0)
            return 0;
        if (IsEmpty) 
            Item = item;
        if (Item != item) 
            return amount;
        
        int added = Mathf.Min(FreeSpaceFor(item), amount);
        Amount += added;
        return amount - added; //overflow
    }
    public int Remove(int amount = 1)
    {
        if (amount <= 0 || IsEmpty)
            return amount;

        int removed = Mathf.Min(Amount, amount);
        Amount -= removed;

        return amount - removed; //unmet demand   
    }
}
