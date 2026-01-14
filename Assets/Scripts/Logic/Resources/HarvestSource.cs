using System.Collections.Generic;
using UnityEngine;
public class HarvestSource
{
    Dictionary<ItemDefinition, int> resources;

    public HarvestSource(Dictionary<ItemDefinition, int> resources)
    {
        this.resources = resources;
    }

    public bool Depleted => resources.Count == 0;

    public int Get(ItemDefinition item)
        => resources.TryGetValue(item, out int a) ? a : 0;
    public bool Has(ItemDefinition item)
        => Get(item) != 0;

    public int Harvest(ItemDefinition item, int amount = 1)
    {
        if (!resources.TryGetValue(item, out int available)) 
            return amount;

        int taken = Mathf.Min(available, amount);
        available -= taken;

        if (available <= 0)
            resources.Remove(item);
        else
            resources[item] = available;

        return amount - taken; //unmet
    }

    public IEnumerable<ItemSlot> Snapshot()
    {
        foreach(var (item, amount) in resources)
            yield return new ItemSlot(item, amount);
    }
}
