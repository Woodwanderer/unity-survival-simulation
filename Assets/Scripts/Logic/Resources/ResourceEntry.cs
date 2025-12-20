using UnityEngine;

public struct ResourceEntry
{
    public readonly ItemType item;
    public readonly int amount;
    public ResourceEntry(ItemType item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}
