using UnityEngine;
using System.Collections.Generic;

public class TileData
{
    public Vector2Int mapCoords { get; private set; }
    public TerrainType Terrain {  get; private set; }
    public ElevationType Elevation { get; private set; }
    public List<TileObject> objects { get; private set; } = new();
    Stockpile stockpile = null;
    public bool isWalkable => Terrain != TerrainType.Water; // hard restriction on WATER tiles only for now
    public bool HasBuilding => stockpile != null;

    public TileData(Vector2Int MapCoords, TerrainType Terrain, ElevationType Elevation)
    {
        this.mapCoords = MapCoords;
        this.Terrain = Terrain;
        this.Elevation = Elevation;
    }
    public void SetBuilding(Stockpile stockpile)
    {
        this.stockpile = stockpile;
    }
    public void AddObject(TileObject tileObject)
    {
        objects.Add(tileObject);
    }
    public void RemoveObject(TileObject tileObject)
    {
        objects.Remove(tileObject);
    }
}
