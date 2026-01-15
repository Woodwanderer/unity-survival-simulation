using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class RenderWorld : MonoBehaviour
{
    //VARIABLES
    public World world;

    //Tiles
    public GameObject tilePrefab;
    private TilePrefab[,] TilePrefabs;

    public TileAppearance config;
    public WorldObjectAppearance objectAppearance;

    public BuildingAppearance buildingAppearance;

    public readonly float tileSize = 1.0f; //basicaly it's the scale 
    Coroutine zoneAnim;

    //Creatures
    public GameObject DeerPrefab;
    public readonly Vector3 creatureTileOffset = new Vector3(0, -0.4f, 0);

    //Protagonist
    public GameObject protagonistPrefab; //Link do prefaba Protagonisty - podpiąć w Unity
    public GameObject protagonist; //Instance
    public AnimateActions animator;

    private Vector2 mapToCenter;

    //INITIALISE
    public void Initialise(World world)
    {
        this.world = world;
        mapToCenter = world.halfWorldSize; // used by MapToWorld()
        TilePrefabs = new TilePrefab[world.WorldSize.x, world.WorldSize.y];
        Render();
    }
    public void Render()
    {
        // Spawn Tile Grid
        for (int x = 0; x < world.WorldSize.x; x++)
        {
            for (int y = 0; y < world.WorldSize.y; y++)
            {
                SpawnTile(world.GetTileData(x, y));
            }
        }
        SpawnProtagonist(world.GetProtagonistData());
        SpawnDeer();
    }
    public Vector3 MapToWorld(Vector2Int coords)
    {
        Vector3 mapToWorld = new((coords.x - mapToCenter.x) * tileSize, (coords.y - mapToCenter.y) * tileSize, 0);
        return mapToWorld;
    }
    public Vector2Int WorldToMap(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / tileSize + mapToCenter.x);
        int y = Mathf.RoundToInt(worldPos.y / tileSize + mapToCenter.y);
        return new Vector2Int(x, y);
    }
    public void Tick(float deltaTime)
    {

    }

    //TILES
    private Sprite GetTerrainSprite(TerrainType type)
    {
        TerrainEntry entry = config.Get(type);
        return entry.GetRandomSprite();
    }
    private Sprite GetElevationSprite(ElevationType type)
    {
        ElevationEntry entry = config.Get(type);
        return entry.GetRandomSprite();
    }
    private void SpawnTile(TileData tileData)
    {
        GameObject tileObjCopy = Instantiate(tilePrefab, MapToWorld(tileData.mapCoords), Quaternion.identity);
        TilePrefab tileP = tileObjCopy.GetComponent<TilePrefab>();

        tileP.SetTileDataRef(tileData);
        tileP.SetTerrain(GetTerrainSprite(tileData.Terrain));
        tileP.SetElevation(GetElevationSprite(tileData.Elevation));

        foreach (TileEntity ent in tileData.entities)
        {
            if (ent is WorldObject wo)
            {
                tileP.SetEntity(wo, GetWorldObjectSprite(tileData, wo), tileSize);
            }
        }

        TilePrefabs[tileData.mapCoords.x, tileData.mapCoords.y] = tileP;
    }
    TilePrefab GetTileP(Vector2Int coords)
    {
        return TilePrefabs[coords.x, coords.y];
    }
    public void SelectTiles(IEnumerable<Vector2Int> tiles, bool active)
    {
        foreach (Vector2Int tileCoord in tiles)
        {
            TilePrefab current = GetTileP(tileCoord);
            current.SetSelected(active);
        }
    }
    public void SelectAreaBuilding(Stockpile stockpile, bool active)
    {
        SelectTiles(stockpile.area.tiles, active);
    }
    public void AnimateZoneSelection(List<Vector2Int> tiles, float delay = 0.015f)
    {
        if (zoneAnim != null)
            StopCoroutine(zoneAnim);

        zoneAnim = StartCoroutine(AnimateZone(tiles, delay));
    }
    IEnumerator AnimateZone(List<Vector2Int> tiles, float delay)
    {
        foreach(Vector2Int tileCoords in tiles)
        {
            TilePrefab tile = GetTileP(tileCoords);
            tile.SetSelected(true);
            yield return new WaitForSeconds(delay);
        }
    }

    //Building
    public void ShowStockpile(Stockpile stockpile)
    {
        if (stockpile == null)
        {
            EventBus.Log("No proper design.");
            return;
        }
        foreach (var tile in stockpile.area.tiles)
        {
            SetBuildingAppearance(stockpile);
        }
    }
    void SetBuildingAppearance(Stockpile stockpile)
    {
        foreach (var tile in stockpile.area.tiles)
        {
            TilePrefab tileP = GetTileP(tile);
            Sprite build;
            if (stockpile.IsConstructed)
            {
                build = buildingAppearance.Get(BuildingType.stockpile).building;
                bool setColour = true;
                tileP.ShowBuilding(true, build, setColour);
            }
            else
            {
                build = buildingAppearance.Get(BuildingType.stockpile).construction;
                tileP.ShowBuilding(true, build);
            }
            
        }
    }

    //Objects
    public void SpawnResourcePile(ResourcePile pile)
    {
        TilePrefab tileP = GetTileP(pile.TileCoords);
        Sprite icon = pile.Item.icon;
        
        tileP.SetEntity(pile, icon, tileSize);
    }
    private Sprite GetWorldObjectSprite(TileData tileD, WorldObject obj)
    {
        if (tileD.entities.Count == 0) return null;

        WorldObjectEntry entry = objectAppearance.Get(obj.Definition.objType);
        if (entry == null) return null;

        return entry.GetRandomSprite();
    }
    public void RemoveObjectSprite(TileEntity ent)
    {
        TilePrefab tileP = TilePrefabs[ent.TileCoords.x, ent.TileCoords.y];
        tileP.HideEntitySprite(ent);
    }

    //PATH
    public void DrawPath(List<Vector2Int> pathCoords, bool visible)
    {
        if (pathCoords.Count == 0)
            return;

        foreach (Vector2Int coords in pathCoords)
        {
            ShowTilePath(coords, visible);
        }
    }
    public void ShowTilePath(Vector2Int coords, bool visible)
    {
        TilePrefabs[coords.x, coords.y].ShowPath(visible);
    }
    

    // PROTAGONIST
    private void SpawnProtagonist(ProtagonistData protagonistData)
    {
        Vector2Int startCoords = world.GetProtagonistCoords();
        Vector3 startLoc = MapToWorld(startCoords) + creatureTileOffset;

        protagonist = Instantiate(protagonistPrefab, startLoc, Quaternion.identity);
        animator = protagonist.GetComponent<AnimateActions>();
        animator.Init(world.protagonistData.actions);
    }
    public Vector3 GetProtagonistLocation() // to jest jednorazwoe pobranie wartości
    {
        return protagonist.transform.position;
    }
    public Transform GetProtagonistTransform() //to jest zwrócenie referencji - będzei aktualizowane, co frame
    {
        return protagonist.transform;
    }

    // NPC - Creatures
    void SpawnDeer()
    {
        Vector2Int startCoords = new Vector2Int(Random.Range(0, world.WorldSize.x), Random.Range(0, world.WorldSize.y));

        CreatureData data = new CreatureData(startCoords);

        GameObject deer = Instantiate(DeerPrefab, MapToWorld(startCoords) + creatureTileOffset, Quaternion.identity);

        CreatureMovement movement = deer.GetComponent<CreatureMovement>();
        movement.Initialise(data, this);
    }

}

