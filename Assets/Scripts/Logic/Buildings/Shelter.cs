using UnityEngine;
using System;
using System.Collections.Generic;
public class Shelter : Building
{
    public int capacity = 1;
    public bool providesRest => IsConstructed;

    public Shelter(Vector2Int coords, BuildingsData.BuildingDef def)
        : base(new Area(new[] { coords }), def) { }
    
    
}
