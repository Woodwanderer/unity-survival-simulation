using NUnit.Framework;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
    private ProtagonsitMovement protMovement;
    
    private Vector2 mapToCenter;


    //INITIALISE
    public void Initialise(World world)
    {        
        this.world = world;
        mapToCenter = world.halfWorldSize; // used by MapToWorld()
        TilePrefabs = new TilePrefab[world.worldSizeX, world.worldSizeY];
        Render();
    }
    public void Render()
    {
        // Spawn Tile Grid
        for (int x = 0; x < world.worldSizeX; x++)
        {
            for (int y = 0; y < world.worldSizeY; y++)
            {
                SpawnTile(world.GetTileData(x, y));
            }
        }

        SpawnProtagonist(world.GetProtagonistData());
        //SpawnDeer();
    }
    public Vector3 MapToWorld(Vector2Int mapPos)
    {
        Vector3 mapToWorld = new((mapPos.x - mapToCenter.x) * tileSize, (mapPos.y - mapToCenter.y) * tileSize, 0);
        return mapToWorld;
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
        if (tileD.objects[0] == null) return null;

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


    //PATH
    public void DrawPath(List<Vector2Int> pathCoords, bool visible)
    {
        foreach (Vector2Int coords in pathCoords)
        {
            TilePath(coords, visible);
        }
    }
    public void TilePath(Vector2Int coords, bool visible)
    {
        TilePrefabs[coords.x, coords.y].ShowPath(visible);
    }
    

    // PROTAGONIST
    private void SpawnProtagonist(ProtagonistData protagonistData)
    {

        Vector2Int startCoords = world.GetProtagonistCoords();
        Vector3 startLoc = MapToWorld(startCoords) + creatureTileOffset;

        GameObject protagonistInstance = Instantiate(protagonistPrefab, startLoc, Quaternion.identity);
        protMovement = protagonistInstance.GetComponent<ProtagonsitMovement>();
        protMovement.Initialise(protagonistData, this);
    }
    public void MoveProt()
    {   
        StartCoroutine(protMovement.MoveAlong());
    }

    // NPC - Creatures
    void SpawnDeer()
    {
        Vector2Int startCoords = new Vector2Int(Random.Range(0, world.worldSizeX), Random.Range(0, world.worldSizeY));

        CreatureData data = new CreatureData(startCoords);

        GameObject deer = Instantiate(DeerPrefab, MapToWorld(startCoords) + creatureTileOffset, Quaternion.identity);

        CreatureMovement movement = deer.GetComponent<CreatureMovement>();
        movement.Initialise(data, this);
    }
    
    //EVENTS
    private void OnEnable()
    {
        EventBus.OnTileHighlight += TileHighlight;
    }
    private void OnDisable()
    {
        EventBus.OnTileHighlight -= TileHighlight;
    }
 

    

}

