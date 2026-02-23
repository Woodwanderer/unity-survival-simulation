using System;
using UnityEngine;
public class ResourcePile : TileEntity
{
    // ResourcePile has to have Item. While amount is == 0 -> object is to be cleared. Only that determines it's validity.
    public ItemSlot Slot { get; }

    public bool IsEmpty => Slot.IsEmpty;
    public event Action<ResourcePile> OnDepleted;
    public int Amount => Slot.Amount;
    public ItemDefinition Item => Slot.Item;
    public ResourcePile(Vector2Int tileCoords, ItemSlot slot) : base(tileCoords)
    {
        if (slot.Item == null || slot.Amount <= 0)
            throw new ArgumentException("ResourcePile must be created with item and amount.");
        Slot = slot;
    }
    
    public int Add(int amount = 1)
    {
        return Slot.Add(Slot.Item, amount);
    }
    public int Remove(int amount = 1)
    {
        int remaining = Slot.Remove(amount);

        if (IsEmpty)
            OnDepleted?.Invoke(this);

        return remaining;
    }
}
