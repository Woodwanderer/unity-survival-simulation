using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class VirtualResources
{
    Dictionary<ItemType , int> resources = new();
    public ResourceEntry wasAdded = default;

    public void AddItem(ItemType type, int amount)
    {
        if (resources.ContainsKey(type)) 
            resources[type] += amount;
        else
            resources[type] = amount;

        wasAdded = new(type, amount);
    }
    public void ClearEntry()
    {
        wasAdded = default;
    }
}
