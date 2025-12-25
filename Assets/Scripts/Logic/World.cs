using NUnit.Framework;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using System.Linq;

public class World
{
    //Variables
    //General World
    int worldSizeX = 60;
    int worldSizeY = 40;
    Vector2Int worldSize;
    public Vector2Int WorldSize => worldSize;
    public Vector2Int halfWorldSize { get; private set; }
    public GameTime gameTime;

    //Tiles
    private TileData[,] tileData;
    private TileData lastTileSelected;
    //Tile Objects
    public TileObjectsDatabase database;
    Pathfinder pathfinder;

    //Protagonist
    public ProtagonistData protagonistData { get; private set; }

    //Resources
    public VirtualResources resources = new();
    
    public World(TileObjectsDatabase data_in, GameTime time)
    {
        this.database = data_in;
        gameTime = time;
        pathfinder = new Pathfinder(this);
    }
    public void Tick(float deltaTime)
    {
        protagonistData.Tick(deltaTime);
    }

    //GETTERS
    //Tiles
    public TileData GetTileData(int x, int y) => tileData[x,y];
    public TileData GetTileData(Vector2Int mapCoords)
    {
        return GetTileData(mapCoords.x, mapCoords.y);
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
                TerrainType terrain;
                ElevationType elevation;
                
                //Set terrain
                int countTerrainType = System.Enum.GetValues(typeof(TerrainType)).Length;
                terrain = (TerrainType)UnityEngine.Random.Range(0, countTerrainType);

                //Set elevation
                if (!(terrain == TerrainType.Water)) 
                {
                    int countElevationType = System.Enum.GetValues(typeof(ElevationType)).Length;
                    elevation = (ElevationType)UnityEngine.Random.Range(1, countElevationType);
                }
                else
                {
                    elevation = ElevationType.Water;
                }


                    tileData[x, y] = new TileData(tilePos, terrain, elevation);

                if (terrain != TerrainType.Water) 
                    PopulateTileObjects(tileData[x, y]);
                else
                    tileData[x, y].isWalkable = false;
            }
        }
    }
 
    private void PopulateTileObjects(TileData tile)
    {
        //Get Random TileObjectType
        int count = System.Enum.GetValues(typeof(TileObjectsType)).Length;
        TileObjectsType type = (TileObjectsType)(UnityEngine.Random.Range(0, count)); //no more None
        
        //                                                                                                              TO DO!!!!!!!!!!!!!!!!!!!!!!!!! -.all tile are filled.Make saome conditional spawn
        TileObjectDefinition definition = database.Get(type);
        TileObject obj = new(definition.objType, definition.GenerateResources());
        tile.AddObject(obj);
        
    }
    private void SetProtagonist()
    {
        protagonistData = new ProtagonistData(halfWorldSize, gameTime.HourDuration, resources);
        string msg = ProtTileDataToString(GetProtagonistTileData());
        EventBus.Log(msg);
    }
    
    //Tile SELECTION
    public bool TrySelectTile(TileData tileData)
    {
        if (tileData == lastTileSelected)
            return false;
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
        protagonistData.pathCoords.Clear();
    }
    public bool EstablishRoute()
    {
        if (protagonistData.mapCoords == lastTileSelected.mapCoords || lastTileSelected.isWalkable == false) 
            return false;


        protagonistData.pathCoords.Clear();
        protagonistData.pathCoords = pathfinder.FindPath(protagonistData.mapCoords, lastTileSelected.mapCoords);
        protagonistData.pathSteps.Clear();
        protagonistData.pathSteps = pathfinder.GetPathSteps(protagonistData.mapCoords, protagonistData.pathCoords);

        EventBus.Log("Route Established. ");
        return true;
    }
    public bool Harvest()
    {
        TileData currentTile = GetProtagonistTileData();
        if (currentTile.objects.Count == 0)
        {
            EventBus.Log("Nothing to gather here.");
            return false;
        }

        TileObjectsType type = currentTile.objects[0].type;
        TileObject obj = currentTile.objects[0];

        Dictionary<ItemType, int> loot = obj.Harvest();
        KeyValuePair<ItemType, int> res00 = loot.First(); //touple - para
        EventBus.Log("You gathered " + res00.Value + " " + res00.Key);

        resources.AddItem(res00.Key, res00.Value);

        //CLEAR - object fully depleted
        if (obj.Items.Count == 0)
        {
            EventBus.ObjectDepleted(currentTile.mapCoords);
            currentTile.objects.Clear();
        }

        return true;
    }

}
