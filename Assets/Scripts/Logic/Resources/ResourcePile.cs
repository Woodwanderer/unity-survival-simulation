using System;
using UnityEngine;
public class ResourcePile : TileEntity
{
    public ItemSlot Slot { get; }
    public ResourcePile(Vector2Int tileCoords, ItemDefinition item, int amount) : base(tileCoords)
    {
        if (item == null || amount <= 0)
            throw new ArgumentException("ResourcePile must be created with item and amount.");
        Slot = new(item, amount);
    }
    public bool IsEmpty => Slot.IsEmpty;
    public int Amount => Slot.Amount;
    public ItemDefinition Item => Slot.Item;
    public int Add(ItemDefinition item, int amount)
    {
        return Slot.Add(item, amount);
    }
    public int Remove(ItemDefinition item, int amount)
    {
        return Slot.Remove(amount);
    }
}
