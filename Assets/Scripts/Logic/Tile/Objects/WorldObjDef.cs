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
    public Dictionary<ItemDefinition, int> GenerateResources() //give age from TileObject later on
    {
        Dictionary<ItemDefinition, int> result = new Dictionary<ItemDefinition, int>();

        if (!spawnOnWorldGen) //gives empty tab
            return result;

        int amount = 0;
        int age = UnityEngine.Random.Range(1, maxAge);

        foreach(ResourceRules resource in resources)
        {
            amount = resource.maxAmount * age / maxAge;
            result[resource.item] = amount;
        }
        return result;
    }
    #if UNITY_EDITOR
    private void OnValidate()
    {
        if(!Application.isPlaying)
            return;

        Debug.LogError($"[WorldObjDef] {name} was modified during Play Mode!", this);
    }
#endif
}
public enum WorldObjType
{
    None,
    Tree,
    FruitTree,
    Bush,
    Berries,
    Rock
}

