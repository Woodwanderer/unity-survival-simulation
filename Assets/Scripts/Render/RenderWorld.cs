using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;


public class RenderWorld : MonoBehaviour
{
    //VARIABLES
    public World world;

    //Tiles
    public GameObject tilePrefab;
    private TilePrefab[,] TilePrefabs;
    public TileAppearance config;
    public TileObjectAppearance objectAppearance;
    public readonly float tileSize = 1.0f; //basicaly it's the scale 

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
    private Sprite GetTileObjectSprite(TileData tileD)
    {
        if (tileD.objects.Count == 0 ) return null;

        TileObjectEntry entry = objectAppearance.Get(tileD.objects[0].type);
        if (entry == null) return null;

        return entry.GetRandomSprite();
    }
    private void SpawnTile(TileData tileData)
    {
        GameObject tileObjCopy = Instantiate(tilePrefab, MapToWorld(tileData.mapCoords), Quaternion.identity);
        TilePrefab tileP = tileObjCopy.GetComponent<TilePrefab>();

        tileP.GetTileDataRef(tileData);
        tileP.SetTerrain(GetTerrainSprite(tileData.Terrain));
        tileP.SetElevation(GetElevationSprite(tileData.Elevation));

        if (tileData.objects != null && tileData.objects.Count > 0) 
            tileP.SetObjects(GetTileObjectSprite(tileData), tileSize);



        TilePrefabs[tileData.mapCoords.x, tileData.mapCoords.y] = tileP;
    }
    
    private void TileHighlight(TileData previousTile, TileData currentTile)
    {
        if (previousTile != null)
        {
            TilePrefabs[previousTile.mapCoords.x, previousTile.mapCoords.y].highlight.enabled = false;
        }
        if (currentTile != null)
            TilePrefabs[currentTile.mapCoords.x, currentTile.mapCoords.y].highlight.enabled = true;
        else return;
    }
    public void RemoveObjectSprite(Vector2Int tileCoords)
    {
        TilePrefab tileP = TilePrefabs[tileCoords.x, tileCoords.y];
        tileP.HideObjectSprite();
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
    
    //EVENTS
    private void OnEnable()
    {
        EventBus.OnTileHighlight += TileHighlight;
        EventBus.OnObjectDepleted += RemoveObjectSprite;
    }
    private void OnDisable()
    {
        EventBus.OnTileHighlight -= TileHighlight;
        EventBus.OnObjectDepleted -= RemoveObjectSprite;
    }

}

