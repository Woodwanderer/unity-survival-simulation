using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileObject
{
    public TileObjectsType type;

    Dictionary<ItemType, int> items = new Dictionary<ItemType, int>();
    public Dictionary<ItemType, int> Items => items;

    public TileObject(TileObjectsType type_in, Dictionary<ItemType, int> startResources)
    {
        type = type_in;
        items = startResources;
    }
    public Dictionary<ItemType, int> HarvestByType(ItemType type)
    {
        Dictionary<ItemType, int> result = new();
        if (!items.ContainsKey(type)) 
            return result; //gonna be null

        result[type] = items[type];
        items.Remove(type);
        return result;
    }
    public Dictionary<ItemType,int> Harvest()
    {
        Dictionary<ItemType, int> result = new();
        if(items.Count == 0) 
            return result; //gonna be null

        ItemType firstKey = items.Keys.First();
        result[firstKey] = items[firstKey];
        items.Remove(firstKey);
        return result;
    }
    public Dictionary<ItemType, int> HarvestFull() //to write - get all items at once
    {
        //Disctioanry to typ referencjyjny. Zwykłe przypisanie, nie tworzy kopi result = items, tylko wskznik na items, wiec niejako result to items i sie czysci przy items.Clear()
        Dictionary<ItemType, int> result = new(items);

        items.Clear();
        return result;
    }
}



