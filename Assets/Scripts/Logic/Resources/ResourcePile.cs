using System;
using System.Collections.Generic;
using UnityEngine;
public class ResourcePile : TileEntity, IItemContainer
{
    ItemSlot slot;
    public ResourcePile(Vector2Int tileCoords, ItemDefinition item, int amount) : base(tileCoords)
    {
        if (item == null || amount <= 0)
            throw new ArgumentException("ResourcePile must be created with item and amount.");
        slot = new(item, amount);
    }
    public IEnumerable<ItemSlot> Slots
    {
        get
        {
            yield return slot;
        }
    }
    public bool IsEmpty => slot.IsEmpty;
    public int Amount => slot.Amount;
    public ItemDefinition Item => slot.Item;
    public int Add(ItemDefinition item, int amount)
    {
        return slot.Add(item, amount);
    }
    public int Remove(ItemDefinition item, int amount)
    {
        return slot.Remove(amount);
    }
    public VirtualResources Snapshot() => new VirtualResources(Slots);

}
