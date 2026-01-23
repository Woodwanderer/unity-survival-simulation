using System.Collections.Generic;
using UnityEngine;
public class World
{
    //Variables
    //General World
    int worldSizeX = 60;
    int worldSizeY = 60;
    Vector2Int worldSize;
    public Vector2Int WorldSize => worldSize;
    public Vector2Int halfWorldSize { get; private set; }
    public GameTime gameTime;
    public RenderWorld render;

    LandGenerator landGenerator;
    //Tiles
    BiomeData biomeData;
    private TileData[,] tileData;
    public List<Vector2Int> tilesSelected = new();
    public Vector2Int? tileSelected = null;
    public Area area;    

    //Tile Entities
    public WorldObjData objDatabase;
    public ItemsDatabase itemsDatabase;

    //Buildings
    BuildingsData buildingsData;

    public Pathfinder pathfinder;

    //Protagonist
    public ProtagonistData protagonistData { get; private set; }

    //Tasks 
    public TaskManager taskManager;

    public World(WorldData data, RenderWorld render, GameTime time)
    {
        this.biomeData = data.biomeData;
        this.objDatabase = data.objDatabase;
        this.itemsDatabase = data.itemsDatabase;
        this.buildingsData = data.buildingsData;
        this.render = render;
        gameTime = time;

        pathfinder = new Pathfinder(this);
    }
    public void Tick(float deltaTime)
    {
        taskManager.Tick(deltaTime);
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
        msg += (" There is: " + tileData.biome.ToString() + " and " + tileData.Elevation.ToString() + " here.");
        return msg;
    }

    //INITIALISE
    public void Initialise(RenderWorld render)
    {
        worldSize = new Vector2Int(worldSizeX, worldSizeY);
        halfWorldSize = worldSize / 2;

        landGenerator = new(worldSizeX, worldSizeY, objDatabase, pathfinder, biomeData);

        SetProtagonist(render);
        taskManager = new(pathfinder);
        //GenerateTiles(); // Basic generator
        tileData = landGenerator.GenerateByArea();
    }
    ////LAND GENERATOR DEBUG PREVIEW ANIMATION
    /*
    public IEnumerable<TileData> DebugGenerateWorldSteps()
    {
        tileData = new TileData[worldSizeX, worldSizeY];
        foreach (TileData tile in landGenerator.GenerateByAreaSteps())
        {
            tileData[tile.mapCoords.x, tile.mapCoords.y] = tile;
            yield return tile;
        }   
    }*/
    private void SetProtagonist(RenderWorld render)
    {
        protagonistData = new ProtagonistData(halfWorldSize, gameTime.HourDuration, this, render);
    }

    //TileObjects
    public ResourcePile CreateResourcePile(TileData tile, ItemDefinition item, int amount)
    {
        ResourcePile pile = new(tile.mapCoords, item, amount);
        taskManager.piles.Add(pile);
        tile.AddEntity(pile);
        return pile;
    }
    public TileEntity FindNearest(ItemSlot order, Vector2Int to)
    {
        return pathfinder.FindEntity(to, order.Item);
    }
    //EVENT Functions
    public void ClearTileEntity(TileEntity ent)
    {
        ent.isValid = false;
        TileData tile = GetTileData(ent.TileCoords);
        tile.entities.Remove(ent);
    }

    //Tile SELECTION
    public void SelectTile(Vector2Int coords)
    {
        if (tileSelected != null) 
            render.SelectTile(tileSelected.Value, false);

        tileSelected = coords;
        render.SelectTile(tileSelected.Value, true);
    }
    public void ClearTileSelected()
    {
        if (tileSelected == null)
            return;

        render.SelectTile(tileSelected.Value, false);
        tileSelected = null;
    }
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
        EventBus.Log($"Area of {area.Count} tiles selected. Press ENTER to confirm building here.");
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

        BuildingsData.BuildingDef def = buildingsData.GetDef(BuildingType.stockpile);
        Stockpile stockpile = new(area, def, this);

        taskManager.constructions.Add(stockpile);

        //render.ShowBuilding(stockpile);
        render.UpdateBuildingAppearance(stockpile);

        EventBus.Log("Stockpile construction site established.");

        area = null;
    }
    public void BuildShelter(Vector2Int coords)
    {
        BuildingsData.BuildingDef def = buildingsData.GetDef(BuildingType.shelter);
        Shelter shelter = new(coords, def);

        TileData tile = GetTileData(coords);
        tile.SetBuilding(shelter);
        taskManager.constructions.Add(shelter);

        //render.ShowBuilding(shelter);
        render.UpdateBuildingAppearance(shelter);

        EventBus.Log("Shelter construction site established.");
    }
    public void OnBuildingConstructed(Building building)
    {
        if (building is Stockpile s)
            taskManager.stockpiles.Add(s);
        else
            taskManager.buildings.Add(building);

        taskManager.constructions.Remove(building);
        EventBus.Log($"{building.Type} succesfully constructed on {building.TileCoords}");
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