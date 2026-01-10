using UnityEngine;
using System.Collections.Generic;

public class Stockpile
{
    World world;
    public string name;
    public Area area;
    public List<TileData> tiles = new();
    public float workTime;
    public float constructionProgress = 0;
    public bool IsConstructed => constructionProgress == 1;

    VirtualResources resources;
    Vector2Int center;
    public Stockpile(Area area, World world)
    {
        this.area = area;
        this.world = world;

        SetTiles();
        workTime = 6 * world.gameTime.HourDuration * area.count; //work units: 6hours/tile
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
