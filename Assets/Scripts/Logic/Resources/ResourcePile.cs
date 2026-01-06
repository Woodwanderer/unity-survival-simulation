using UnityEngine;

public class ResourcePile
{
    public ItemDefinition item;
    public int amount;

    bool IsFull => amount >= item.maxStockpileSize;
    public ResourcePile(ItemDefinition item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
    public int Add(int delta)
    {
        int space = item.maxStockpileSize - amount;
        int added = Mathf.Min(space, delta);
        amount += added;
        return delta - added; //overflow
    }

}
