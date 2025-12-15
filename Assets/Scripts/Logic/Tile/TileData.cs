using UnityEngine;
using System.Collections.Generic;

public class TileData
{
    public Vector2Int mapCoords { get; private set; }
    public TerrainType Terrain {  get; private set; }
    public ElevationType Elevation { get; private set; }
    public List<TileObject> objects { get; private set; } = new();
    //private static int objCapacity = 3;

    public TileData(Vector2Int MapCoords, TerrainType Terrain, ElevationType Elevation)
    {
        this.mapCoords = MapCoords;
        this.Terrain = Terrain;
        this.Elevation = Elevation;
    }
    public void SetObjects(List<TileObject> objectsIn)
    {
        objects = objectsIn;
    }
    public void AddObject(TileObject tileObject)
    {
        objects.Add(tileObject);
    }

}
