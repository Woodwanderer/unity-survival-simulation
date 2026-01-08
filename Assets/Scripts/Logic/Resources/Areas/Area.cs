using System.Collections.Generic;
using UnityEngine;

public class Area
{
    public HashSet<Vector2Int> tiles { get; }
    public int count;
    public Area(IEnumerable<Vector2Int> tiles) //result ()
    {
        this.tiles = new HashSet<Vector2Int>(tiles);
        count = this.tiles.Count;
    }
}

