using System.Collections.Generic;

public class VirtualResources
{
    Dictionary<ItemDefinition, int> resources = new();
    public VirtualResources() { } //create empty
    public VirtualResources(IEnumerable<KeyValuePair<ItemDefinition, int>> res) // gotta copy Dictionary, else it works on original //..also kvpair -> gives possibility to add any datatype: list, kv, dictionary etc.
    {
        if (res == null)
            return;

        foreach (var kv in res) 
        {
            resources[kv.Key] = kv.Value;
        }
    }
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
    public void Add(ResourcePile transfer)
        => Add(transfer.item, transfer.amount);

    public bool Remove(ItemDefinition type, int amount)
    {
        if (!Has(type, amount))
            return false;

        resources[type] -= amount;
        if (resources[type] <= 0)
            resources.Remove(type);

        return true;
    }
    public bool Remove(ResourcePile transfer)
        => Remove(transfer.item, transfer.amount);
            
    public IEnumerable<KeyValuePair<ItemDefinition, int>> All()
        => resources;
    public IEnumerable<ResourcePile> DrainAll()
    {
        foreach(var kv  in resources)
            yield return new ResourcePile(kv.Key, kv.Value);

        resources.Clear();
    }
    public float CalculateWeight(ItemDefinition additionalItem = null)
    {
        float weight = 0;
        foreach (var (item, amount) in resources) 
        {
            weight += item.weight * amount;
        }

        if (additionalItem != null)
            weight += additionalItem.weight;

        return weight;
    }

}
