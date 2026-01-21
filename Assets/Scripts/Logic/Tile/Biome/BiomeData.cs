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
        public Biome biome;

        [Range(0f, 1f)]
        public float density = 1f;

        public List<SpawnEntry> spawnEntries;
        public float TotalWeight
        {
            get
            {
                float weight = 0;
                if (spawnEntries != null)
                {
                    foreach (var entry in spawnEntries)
                        weight += entry.weight;
                }
                return weight;
            }
        }

        [SerializeField, Tooltip("Sum of all entry weights (read-only)")]
        float totalWeight;
        public void RecalculateWeight()
        {
            float weight = 0;
            if (spawnEntries != null)
            {
                foreach (var entry in spawnEntries)
                    weight += entry.weight;
            }
            totalWeight = weight;
        }
        
    }
    [System.Serializable]
    public class SpawnEntry
    {
        public WorldObjDef worldObjDef;
        public float weight; //0-100 as %
    }
    private void OnValidate()
    {
        if (BiomeDefs == null)
            return;

        foreach (var def in BiomeDefs)
            def?.RecalculateWeight();   
    }
}
public enum Biome
{
    Grasslands,
    Woods,
    Forest,
    Water
}
