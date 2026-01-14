using System.Collections.Generic;
using UnityEngine;

public class Area
{
    public HashSet<Vector2Int> tiles { get; }
    public int Count => tiles.Count;
    
    public Vector2Int center;
    public Area(IEnumerable<Vector2Int> tiles)
    {
        this.tiles = new HashSet<Vector2Int>(tiles);
        CalculateCenter();
    }
    public bool Contains(Vector2Int coords)
    {
        return tiles.Contains(coords); 
    }
    public void CalculateCenter()
    {
        Vector2 sum = new();
        foreach (var tile in this.tiles)
            sum += tile;

        center = Vector2Int.RoundToInt(sum / Count);
    }
}

