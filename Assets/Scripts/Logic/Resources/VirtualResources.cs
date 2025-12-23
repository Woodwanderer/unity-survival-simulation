using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class VirtualResources
{
    Dictionary<ItemType , int> resources = new();
    public ResourceEntry wasAdded = default;
    public bool added = false;
    public ResourceEntry wasRemoved = default;
    public bool removed = false;

    public void AddItem(ItemType type, int amount)
    {
        if (resources.ContainsKey(type)) 
            resources[type] += amount;
        else
            resources[type] = amount;

        wasAdded = new(type, amount);
        added = true;
    }
    public bool RemoveItem(ItemType type, int amount)
    {
        if (resources.ContainsKey(type))
        {
            if (resources[type] >= amount)
            {
                resources[type] -= amount;
                wasRemoved = new(type, amount);
                return removed = true;
            }
            EventBus.Log("Not enough resources.");
            return removed = false;
        }
        else
        {
            EventBus.Log("Not enough resources.");
            return removed = false;
        }
    }
    public void ClearEntry()
    {
        wasAdded = default;
        wasRemoved= default;
        added = false;
        removed = false;
    }
}
