using System.Collections.Generic;

public class VirtualResources
{
    Dictionary<ItemType , int> resources = new();

    public int Get(ItemType type)
        => resources.TryGetValue(type, out int amount) ? amount : 0;
    
    public bool Has(ItemType type, int amount = 1)
        => Get(type) >= amount; 
    
    public void Add(ItemType type, int amount)
    {
        resources[type] = Get(type) + amount;
    }
    public void Add(ResourceChange entryItem)
    {
        Add(entryItem.item, entryItem.delta);
    }
    public bool Remove(ItemType type, int amount)
    {
        if (!Has(type, amount))
            return false;

        resources[type] -= amount;
        if (resources[type] <= 0)
            resources.Remove(type);

        return true;
    }
    public IEnumerable<KeyValuePair<ItemType, int>> All()
        => resources;

}
