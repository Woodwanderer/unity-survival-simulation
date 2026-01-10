using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
    RenderWorld render;

    //Tiles
    private TileData[,] tileData;
    public List<Vector2Int> tilesSelected = new();
    public Area area;
    public List<Stockpile> stockpiles = new();

    //Tile Objects
    public TileObjectsDatabase objDatabase;
    public ItemsDatabase itemsDatabase;
    public Pathfinder pathfinder;

    //Protagonist
    public ProtagonistData protagonistData { get; private set; }

    public World(TileObjectsDatabase data_in, ItemsDatabase itemsData, RenderWorld render, GameTime time)
    {
        this.objDatabase = data_in;
        this.itemsDatabase = itemsData;
        this.render = render;
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
    public void Initialise(RenderWorld render)
    {
        worldSize = new Vector2Int(worldSizeX, worldSizeY);
        halfWorldSize = worldSize / 2;

        GenerateTiles();
        SetProtagonist(render);
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
            }
        }
    }
    private void SetProtagonist(RenderWorld render)
    {
        protagonistData = new ProtagonistData(halfWorldSize, gameTime.HourDuration, this, render);
    }

    //TileObjects
    private void PopulateTileObjects(TileData tile)
    {
        // is spawned at world generation?
        var spawnableDefs = objDatabase.definitions.Where(def => def.spawnOnWorldGen).ToArray();

        TileObjectDefinition def = spawnableDefs[Random.Range(0, spawnableDefs.Length)];

        TileObject obj = new(def.objType, tile.mapCoords);

        obj.resources = new(def.GenerateResources());

        tile.AddObject(obj);
    }
    public TileObject CreateResourcePile(TileData tile, ItemDefinition item, int amount)
    {
        TileObjectDefinition pileDef = objDatabase.Get(TileObjectsType.ResourcePile);

        TileObject pile = new(pileDef.objType, tile.mapCoords);
        pile.pile = new(item, amount);

        tile.AddObject(pile);
        return pile;
    }
    
    //EVENT Functions
    public void ClearTileObject(TileObject obj)
    {
        TileData tile = GetTileData(obj.tileCoords);
        tile.objects.Remove(obj);
    }

    //Tile SELECTION
    public List<Vector2Int> GetTileCoordsInRect(Vector2Int a,  Vector2Int b)
    {
        TileRect rect = new(a, b);
        List<Vector2Int> tileCoords = new List<Vector2Int>();

        for(int x = rect.Min.x; x <= rect.Max.x; x++)
        {
            for(int y = rect.Min.y; y <= rect.Max.y; y++)
            {   
                tileCoords.Add(new Vector2Int(x, y));
            }
        }
        return tileCoords;
    }
    public void SelectZone(Vector2Int a, Vector2Int b)
    {
        List<Vector2Int> selection = GetTileCoordsInRect(a, b);

        render.SelectTiles(tilesSelected, false);
        tilesSelected = selection;
        render.SelectTiles(tilesSelected, true);
    }
    public void SelectConnectedZone(Vector2Int a, Vector2Int b)
    {
        List<Vector2Int> selection = pathfinder.FloodFill(a, b);
        if (selection.Count == 0)
        {
            EventBus.Log("Can't build here.");
            return;
        }

        render.SelectTiles(tilesSelected, false);
        tilesSelected = selection;
        render.AnimateZoneSelection(selection);
        area = new(selection);
        EventBus.Log($"Area of {area.count} tiles selected. Press ENTER to confirm building here.");
    }
    public void ClearZoneSelection()
    {
        render.SelectTiles(tilesSelected, false);
        tilesSelected.Clear();
        area = null;
    }
    //Buidling
    public void BuildStockpile()
    {
        if (area == null) 
            return;

        Stockpile stockpile = new(area, this);
        stockpiles.Add(stockpile);
        render.SpawnStockpile(stockpile);
        EventBus.Log("Stockpile established.");
        area = null;
    }
}

public struct TileRect
{
    public Vector2Int Min;
    public Vector2Int Max;

    public TileRect(Vector2Int a, Vector2Int b)
    {
        Min = new(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y));
        Max = new(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
    }
    public bool Contains(Vector2Int pos)
    {
        return ( pos.x >= Min.x &&
                 pos.y >= Min.y &&
                 pos.x <= Max.x &&
                 pos.y <= Max.y
               );
    }
}