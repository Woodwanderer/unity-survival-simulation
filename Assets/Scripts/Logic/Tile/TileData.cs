using UnityEngine;
using System.Collections.Generic;

public class TileData
{
    public Vector2Int mapCoords { get; private set; }
    public TerrainType Terrain {  get; private set; }
    public ElevationType Elevation { get; private set; }
    public List<TileEntity> entities { get; private set; } = new();
    public Stockpile stockpile = null;
    public bool isWalkable => Terrain != TerrainType.Water; // hard restriction on WATER tiles only for now
    public bool HasBuilding => stockpile != null;
    public bool isInit = false;
    public TileData(Vector2Int mapCoords)
    {
        this.mapCoords = mapCoords;
    }
    public void SetLand( TerrainType Terrain, ElevationType Elevation)
    {
        this.Terrain = Terrain;
        this.Elevation = Elevation;
        isInit = true;
    }
    public TileData(Vector2Int MapCoords, TerrainType Terrain, ElevationType Elevation)
    {
        this.mapCoords = MapCoords;
        this.Terrain = Terrain;
        this.Elevation = Elevation;
        isInit = true;
    }
    public void SetBuilding(Stockpile stockpile)
    {
        this.stockpile = stockpile;
    }
    public void AddEntity(TileEntity entity)
    {
        entities.Add(entity);
    }
    public void RemoveObject(TileEntity tileObject)
    {
        entities.Remove(tileObject);
    }
    public TileEntity Contains(ItemDefinition item)
    {
        TileEntity entity = null;
        entity = FindInPiles(item);
        if (entity != null) 
            return entity;

        entity = FindInWorldObj(item);
        return entity;
    }
    public WorldObject FindInWorldObj(ItemDefinition item)
    {
        foreach (TileEntity ent in entities)
        {
            if (ent is WorldObject obj && obj.harvestSource != null && obj.harvestSource.Has(item))
                return obj;
        }
        return null;
    }
    public ResourcePile FindInPiles(ItemDefinition item)
    {
        foreach (TileEntity ent in entities)
        {
            if (ent is ResourcePile pile && pile != null && pile.Item == item)
                return pile;
        }
        return null;
    }
}
