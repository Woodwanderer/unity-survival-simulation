using UnityEngine;

[CreateAssetMenu(fileName = "TileAppearance", menuName = "Scriptable Objects/TileAppearance")]
public class TileAppearance : ScriptableObject
{
    public TerrainEntry[] terrainEntries;
    public ElevationEntry[] elevationEntries;

    public TerrainEntry Get(TerrainType type)
    {
        foreach (TerrainEntry entry in terrainEntries)
        {
            if (entry.type == type)
                return entry;
        }
        return null;
    }
    public ElevationEntry Get(ElevationType type)
    {
        foreach (ElevationEntry entry in elevationEntries)
        {
            if (entry.type == type)
                return entry;
        }
        return null;
    }
}

[System.Serializable]
public class TerrainEntry
{
    public TerrainType type;
    public Sprite[] variants;
    public Sprite GetRandomSprite()
    {
        if (variants == null || variants.Length == 0)
            return null;
        return variants[Random.Range(0, variants.Length)];
    }
}



[System.Serializable]
public class ElevationEntry
{
    public ElevationType type;
    public Sprite[] variants;
    public Sprite GetRandomSprite()
    {
        if (variants == null || variants.Length == 0)
            return null;
        return variants[Random.Range(0, variants.Length)];
    }
}

