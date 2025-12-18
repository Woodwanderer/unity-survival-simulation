using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileObjectDefinition", menuName = "Scriptable Objects/TileObjectDefinition")]
public class TileObjectDefinition : ScriptableObject
{
    public TileObjectsType objType;
    public int maxAge;

    public ResourceRules[] resources;

    [System.Serializable]
    public class ResourceRules
    {
        public ItemType item;
        public int maxAmount = 0;
    }
    public Dictionary<ItemType, int> GenerateResources() //give age from TileObject later on
    {
        int amount = 0;
        int age = Random.Range(1, maxAge + 1);
        Dictionary<ItemType, int> result = new Dictionary<ItemType, int>();
        foreach(ResourceRules resource in resources)
        {
            amount = resource.maxAmount * age / maxAge;
            result[resource.item] = amount;
        }
        return result;
    }

}
