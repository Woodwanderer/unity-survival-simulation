using System.Collections.Generic;
public class VirtualResources
{
    Dictionary<ItemDefinition, int> resources = new();
    public VirtualResources(IEnumerable<ItemSlot> slots)
    {
        foreach(var slot in slots)
        {
            if (slot == null || slot.IsEmpty) 
                continue;

            resources[slot.Item] = Get(slot.Item) + slot.Amount;
        }
    }
    public bool Depleted => resources.Count == 0;
    public int Get(ItemDefinition item)
        => resources.TryGetValue(item, out int amount) ? amount : 0;
    
    public bool Has(ItemDefinition item, int amount = 1)
        => Get(item) >= amount; 
         
    public IEnumerable<KeyValuePair<ItemDefinition, int>> All()
        => resources;
   
}
