using UnityEngine;
using System.Collections.Generic;

public class Stockpile
{
    World world;
    public string name;
    public Area area;
    public List<TileData> tiles = new();

    VirtualResources resources;
    Vector2Int center;
    public Stockpile(Area area, World world)
    {
        this.area = area;
        this.world = world;
        SetTiles();
    }
    void SetTiles()
    {
        foreach (var tile in area.tiles)
        {
            TileData current = world.GetTileData(tile);
            tiles.Add(current);
            current.SetBuilding(this);
        }
    }

}
