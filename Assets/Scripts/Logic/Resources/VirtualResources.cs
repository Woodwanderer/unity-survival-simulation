using System.Collections.Generic;

public class VirtualResources
{
    Dictionary<ItemDefinition, int> resources = new();
    bool isDepleted =>
        resources.Count == 0;
    public bool Depleted => isDepleted;

    public int Get(ItemDefinition type)
        => resources.TryGetValue(type, out int amount) ? amount : 0;
    
    public bool Has(ItemDefinition type, int amount = 1)
        => Get(type) >= amount; 
    
    public void Add(ItemDefinition type, int amount)
    {
        resources[type] = Get(type) + amount;
    }
    public void Add(ResourceChange entryItem)
        => Add(entryItem.item, entryItem.delta);

    public bool Remove(ItemDefinition type, int amount)
    {
        if (!Has(type, amount))
            return false;

        resources[type] -= amount;
        if (resources[type] <= 0)
            resources.Remove(type);

        return true;
    }
    public bool Remove(ResourceChange transfer)
        => Remove(transfer.item, transfer.delta);
            
    public IEnumerable<KeyValuePair<ItemDefinition, int>> All()
        => resources;
    public IEnumerable<ResourceChange> DrainAll()
    {
        foreach(var kv  in resources)
            yield return new ResourceChange(kv.Key, kv.Value);

        resources.Clear();
    }

}
