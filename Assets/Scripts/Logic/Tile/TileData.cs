using UnityEngine;
using System.Collections.Generic;
using System;

public class TileData
{
    public Vector2Int mapCoords { get; private set; }
    public Biome biome {  get; private set; }
    public ElevationType Elevation { get; private set; }
    public List<TileEntity> entities { get; private set; } = new();
    public Building building = null;
    public bool HasBuilding => building != null;
    public bool isWalkable => biome != Biome.Water; // hard restriction on WATER tiles only for now
    
    public bool isInit = false;
    public TileData(Vector2Int mapCoords)
    {
        this.mapCoords = mapCoords;
    }
    public void SetLand( Biome Terrain, ElevationType Elevation)
    {
        this.biome = Terrain;
        this.Elevation = Elevation;
        isInit = true;
    }
    public TileData(Vector2Int MapCoords, Biome Terrain, ElevationType Elevation)
    {
        this.mapCoords = MapCoords;
        this.biome = Terrain;
        this.Elevation = Elevation;
        isInit = true;
    }
    public void SetBuilding(Building building)
    {
        if (HasBuilding)
            throw new InvalidOperationException($"Tile {mapCoords} already has a building.");

        this.building = building;
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
