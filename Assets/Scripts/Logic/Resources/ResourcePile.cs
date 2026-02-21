using System;
using UnityEngine;
public class ResourcePile : TileEntity
{
    // ResourcePile has to have Item. While amount is == 0 -> object is to be cleared. Only that determines it's validity.
    public ItemSlot Slot { get; }
    public ResourcePile(Vector2Int tileCoords, ItemSlot slot) : base(tileCoords)
    {
        if (slot.Item == null || slot.Amount <= 0)
            throw new ArgumentException("ResourcePile must be created with item and amount.");
        Slot = slot;
    }
    public bool IsEmpty => Slot.IsEmpty;
    public int Amount => Slot.Amount;
    public ItemDefinition Item => Slot.Item;
    public int Add(int amount = 1)
    {
        return Slot.Add(Slot.Item, amount);
    }
    public int Remove(int amount = 1)
    {
        return Slot.Remove(amount);
    }
}
