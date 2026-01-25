using UnityEngine;

public class Shelter : Building
{
    readonly int capacity = 2; // data
    public int Capacity;
    public float comfort = 1f;

    public Shelter(Vector2Int coords, BuildingsData.BuildingDef def)
        : base(new Area(new[] { coords }), def)
    {
        Capacity = capacity;
    }
    
    
}
