using UnityEngine;

public readonly struct ResourceChange
{
    public readonly ItemDefinition item;
    public readonly int delta; // + = add, - = remove
    public ResourceChange(ItemDefinition item, int delta)
    {
        this.item = item;
        this.delta = delta;
    }
}
