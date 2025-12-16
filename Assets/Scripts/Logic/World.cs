using NUnit.Framework;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;
using Unity.Collections;

public class World
{
    //Variables
    public int worldSizeX { get; private set; } = 20;
    public int worldSizeY { get; private set; } = 20;
    public Vector2Int worldSize { get; private set; }
    public Vector2Int halfWorldSize { get; private set; }

    private TileData[,] tileData;
    private TileData lastTileSelected;
    public ProtagonistData protagonistData { get; private set; }
    TerrainType terrain;
    ElevationType elevation;

    //GETTERS
    //Tiles
    public TileData GetTileData(int x, int y) => tileData[x,y];
    private TileData GetTileData(Vector2Int coords)
    {
        return tileData[coords.x, coords.y];
    }
    public Vector2Int GetTileCoords(TileData tileData)
    {
        return tileData.mapCoords;
    }
    public Vector2Int GetLastTileSelectedCoords()
    {
        return GetTileCoords(lastTileSelected);
    }
    //Protagonist
    public ProtagonistData GetProtagonistData() => protagonistData;
    public TileData GetProtagonistTileData()
    {
        return GetTileData(protagonistData.mapCoords.x, protagonistData.mapCoords.y);
    }
    public Vector2Int GetProtagonistCoords()
    {
        return protagonistData.mapCoords;
    }

    public string ProtTileDataToString(TileData tileData)
    {
        string msg = ("You are on tile: " + protagonistData.mapCoords.ToString());
        msg += (" There is: " + tileData.Terrain.ToString() + " and " + tileData.Elevation.ToString() + " here.");
        return msg;
    }

    //INITIALISE
    public void Initialise()
    {
        worldSize = new Vector2Int(worldSizeX, worldSizeY);
        halfWorldSize = worldSize / 2;

        GenerateTiles();
        SetProtagonist();
    }
    public void GenerateTiles()
    {

        tileData = new TileData[worldSizeX, worldSizeY];

        for (int x = 0; x < worldSizeX; x++)
        {
            for (int y = 0; y < worldSizeY; y++)
            {
                Vector2Int tilePos = new(x, y);

                int count = System.Enum.GetValues(typeof(TerrainType)).Length;
                terrain = (TerrainType)UnityEngine.Random.Range(0, count);

                elevation = (ElevationType)UnityEngine.Random.Range(0, 3);

                tileData[x, y] = new TileData(tilePos, terrain, elevation);
                PopulateTileObjects(tileData[x, y]);
            }
        }
    }
    private void PopulateTileObjects(TileData tile)
    {
        int count = System.Enum.GetValues(typeof(TileObjectsType)).Length;
        TileObjectsType type = (TileObjectsType)(UnityEngine.Random.Range(0, count)); // with None
        TileObject obj = new(type, true);
        obj.quantity = UnityEngine.Random.Range(1, 5);

        tile.AddObject(obj);
    }

    private void SetProtagonist()
    {
        protagonistData = new ProtagonistData(halfWorldSize);
        string msg = ProtTileDataToString(GetProtagonistTileData());
        EventBus.Log(msg);
    }
    
    //Tile SELECTION
    public bool TrySelectTile(TileData tileData)
    {
        if (tileData == lastTileSelected)
        {
            EventBus.Log("The tile is already selected Bro. :) ");
            return false;
        }
        else
        {
            EventBus.TileHighlight(lastTileSelected, tileData);
            lastTileSelected = tileData;
            EventBus.Log("You selected tile: " + lastTileSelected.mapCoords.ToString());
            return true;
        }

    }
    public bool CancelSelection()
    {
        if (lastTileSelected != null)
        { 
            EventBus.TileHighlight(lastTileSelected, null);
        }
        lastTileSelected = null;
        return true;
    }

    //ROUTE
    public void CancelRoute()
    {
        protagonistData.route.Clear();
    }
    public bool EstablishRoute()
    {
        if (protagonistData.mapCoords == lastTileSelected.mapCoords)
            return false;

        protagonistData.SetRouteTo(lastTileSelected.mapCoords);

        EventBus.Log("Route Established. ");
        return true;
    }
    public bool Harvest()
    {
        TileData currentTile = GetProtagonistTileData();
        if(currentTile.objects.Count == 0)
        {
            EventBus.Log("Nothing to gather here.");
            return false;
        }

        TileObjectsType type = currentTile.objects[0].type;
        if (type == TileObjectsType.Rock)
            EventBus.Log("You gathered some stone.");
        else
            EventBus.Log("You gathered some wood.");
        currentTile.objects[0].quantity -= 1;
        if(currentTile.objects[0].quantity <= 0)
        {
            EventBus.ObjectDepleted(currentTile.mapCoords);
            currentTile.objects.Clear();
        }
        return true;
    }
  
}
