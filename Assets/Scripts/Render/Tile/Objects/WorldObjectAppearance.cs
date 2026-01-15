using UnityEngine;

[CreateAssetMenu(fileName = "TileObjectAppearance", menuName = "Scriptable Objects/TileObjectAppearance")]
public class WorldObjectAppearance : ScriptableObject
{
    public WorldObjectEntry[] entries;
    public WorldObjectEntry Get(TileObjectsType type)
    {
        foreach (var entry in entries)
        {
            if (entry.type == type)
                return entry;
        }
        return null;
    }
}

[System.Serializable]
public class WorldObjectEntry
{
    public TileObjectsType type;
    public Sprite[] variants;

    //Random Variant Choice
    public Sprite GetRandomSprite()
    {
        if (variants == null || variants.Length == 0)
            return null;
        return variants[Random.Range(0, variants.Length)];
    }
}
