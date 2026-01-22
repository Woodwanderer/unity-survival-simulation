using UnityEngine;
using System;
using System.Collections.Generic;
public class Shelter : Building
{
    public BuildingType type;
    public int capacity = 1;
    public bool providesRest => IsConstructed;
    public override IEnumerable<Vector2Int> OccupiedTiles
    {
        get { yield return TileCoords; }
    }

    public Shelter(Vector2Int tileCoords, BuildingsData.BuildingDef def) : base(tileCoords, def) { }
    
    
}
