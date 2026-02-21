using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldObjDef", menuName = "Scriptable Objects/WorldObjDef")]
public class WorldObjDef : ScriptableObject
{
    public WorldObjType objType;
    public int maxAge;
    public bool spawnOnWorldGen;

    public ResourceRules[] resources;

    [System.Serializable]
    public class ResourceRules
    {
        public ItemDefinition item;
        public int maxAmount = 0;
    }
    public Dictionary<ItemDefinition, int> GenerateResources(int age) //give age from TileObject later on
    {
        Dictionary<ItemDefinition, int> result = new Dictionary<ItemDefinition, int>();

        if (!spawnOnWorldGen) //gives empty tab
            return result;

        foreach(ResourceRules resource in resources)
        {
            int amount = resource.maxAmount * age / maxAge;
            result[resource.item] = amount;
        }
        return result;
    }
}
public enum WorldObjType
{
    Tree,
    FruitTree,
    Bush,
    Berries,
    Rock
}

