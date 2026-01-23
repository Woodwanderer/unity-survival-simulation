using System.Collections.Generic;
using UnityEngine;

public class Area
{
    public HashSet<Vector2Int> tiles { get; } = new();

    HashSet<Vector2Int> boundary;
    HashSet<Vector2Int> outerRing;
    public IReadOnlyCollection<Vector2Int> Boundary
    {
        get
        {
            if (dirty)
                Recalculate();
            return boundary;
        }
    }
    public IReadOnlyCollection<Vector2Int> OuterRing
    {
        get
        {
            if (dirty)
                Recalculate();
            return outerRing;
        }
    }
    bool dirty = true;
    public int Count => tiles.Count;
    
    public Vector2Int center;
    
    public Area(IEnumerable<Vector2Int> tiles)
    {
        this.tiles = new HashSet<Vector2Int>(tiles);
        CalculateCenter();
    }
    public void AddTile(Vector2Int mapCoords)
    {
        tiles.Add(mapCoords);
        dirty = true;
    }
    public void RemoveTile(Vector2Int mapCoords)
    {
        tiles.Remove(mapCoords);
        dirty = true;
    }
    public bool Contains(Vector2Int coords)
    {
        return tiles.Contains(coords); 
    }
    public void CalculateCenter()
    {
        Vector2 sum = Vector2.zero;
        foreach (var tile in tiles)
            sum += tile;

        center = Vector2Int.RoundToInt(sum / Count);
    }
    void Recalculate()
    {
        boundary = new();
        outerRing = new();

        Vector2 sum = Vector2.zero;

        foreach (var tile in tiles)
        {
            sum += tile;
            foreach (var dir in GridDirections.Cardinal)
            {
                Vector2Int n = tile + dir;
                if (!tiles.Contains(n))
                {
                    boundary.Add(tile);
                    outerRing.Add(n);
                }
            }
        }
        center = Vector2Int.RoundToInt(sum / Count);
        dirty = false;
    }
    public bool IsInVicinity(Vector2Int coords)
    {
        if (dirty)
            Recalculate();

        return outerRing.Contains(coords);
    }
    public bool IsInArea(Vector2Int coords)
    {
        return Contains(coords);
    }
    public bool IsInRange(Vector2Int coords)
    {
        return IsInVicinity(coords) || IsInArea(coords);
    }
    

}

