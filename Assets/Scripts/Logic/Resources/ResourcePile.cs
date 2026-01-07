using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public int Remove(int demand)
    {
        if (demand <= amount)
        {
            amount -= demand;
            return 0;
        }
        else
        {
            amount = 0; // Can I destroy ResourcePile?
            item = null;
            return demand - amount;
        }
    }
    public IEnumerable<KeyValuePair<ItemDefinition, int>> Get()
    {
        if(item == null || amount <= 0)
            yield break; // Returns Enumerable.Empty<KeyValuePair..>
        yield return new KeyValuePair<ItemDefinition, int>(item, amount);
    }
}
