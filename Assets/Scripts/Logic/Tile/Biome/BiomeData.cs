using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeData", menuName = "Scriptable Objects/BiomeData")]
public class BiomeData : ScriptableObject
{
    public BiomeDef[] BiomeDefs;


    public BiomeDef GetBiomeDef(Biome type)
    {
        foreach (BiomeDef def in BiomeDefs)
            if (def.biome == type)
                return def;
        return null;
    }

    [System.Serializable]
    public class BiomeDef
    {
        public float Weight
        {
            get
            {
                if (spawnEntries == null)
                    return 0;
                if(spawnEntries.Count == 0)
                    return 0;
                float weight = 0;
                foreach (var entry in spawnEntries)
                {
                    weight += entry.weight;
                }
                return weight;
            }
        }
        public Biome biome;
        public List<SpawnEntry> spawnEntries;
    }
    [System.Serializable]
    public class SpawnEntry
    {
        public WorldObjDef worldObjDef;
        public float weight; //0-100 as %
    }
}
public enum Biome
{
    Grasslands,
    Woods,
    Forest,
    Water
}
