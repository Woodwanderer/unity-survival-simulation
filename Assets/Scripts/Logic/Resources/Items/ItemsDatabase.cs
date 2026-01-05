using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsDatabase", menuName = "Scriptable Objects/ItemsDatabase")]
public class ItemsDatabase : ScriptableObject
{
    public ItemDefinition[] items;

    Dictionary<string, ItemDefinition> lookup;

    private void OnEnable()
    {
        lookup = new Dictionary<string, ItemDefinition>();
        foreach (var item in items)
        {
            lookup[item.id] = item;
        }
    }
    public ItemDefinition Get(string id)
    {
        return lookup[id];
    }
}
