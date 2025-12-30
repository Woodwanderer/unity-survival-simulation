using UnityEngine;

public readonly struct ResourceChange
{
    public readonly ItemType item;
    public readonly int delta; // + = add, - = remove
    public ResourceChange(ItemType item, int delta)
    {
        this.item = item;
        this.delta = delta;
    }
}
