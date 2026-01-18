using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class LandGenerator
{
    int gridX;
    int gridY;
    TileData[,] grid;
    TileObjectsDatabase objData;
    Pathfinder pathfinder;
    public LandGenerator(int gridX, int gridY, TileObjectsDatabase objData, Pathfinder pathfinder)
    {
        this.gridX = gridX;
        this.gridY = gridY;
        this.objData = objData;
        this.pathfinder = pathfinder;
    }
    public TileData GetTileData(int x, int y) => grid[x, y];
    public TileData GetTileData(Vector2Int mapCoords)
    {
        return GetTileData(mapCoords.x, mapCoords.y);
    }
    void SetTileLand(TileData tile)
    {
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

        tile.SetLand(terrain, elevation);


        if (terrain != TerrainType.Water)
            PopulateWorldObjects(grid[tile.mapCoords.x, tile.mapCoords.y]);
    }
    static readonly Vector2Int[] Directions =
{
        new Vector2Int( 1, 0 ),
        new Vector2Int( 0,-1 ),
        new Vector2Int(-1, 0 ),
        new Vector2Int( 0, 1 ),
    };
    bool IsWithinWorld(Vector2Int pos)
    {
        return (pos.x >= 0 &&
                 pos.y >= 0 &&
                 pos.x < gridX &&
                 pos.y < gridY
               );
    }
    IEnumerable<Vector2Int> GetNeighbours(Vector2Int pos)
    {
        foreach (Vector2Int dir in Directions)
        {
            Vector2Int next = pos + dir;

            if (!IsWithinWorld(next))
                continue;

            yield return next;
        }
    }
    public TileData[,] GenerateByArea() // 0.0 Genrator -> equal random values for all types
    {
        grid = new TileData[gridX, gridY];
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                Vector2Int tilePos = new(x, y);
                grid[x, y] = new TileData(tilePos);
            }
        }

        List<TileData> notSet = new();
        foreach (TileData tile in grid)
        {
            notSet.Add(tile);
        }


        while (notSet.Count > 0)
        {
            TileData start = notSet[UnityEngine.Random.Range(0, notSet.Count)];
            SetTileLand(start);
            notSet.Remove(start);

            Queue<TileData> area = new();
            area.Enqueue(start);

            List<TileData> alreadyRolled = new();
            while (area.Any())
            {
                float chance = Random.Range(0.45f, 0.55f);
                float chanceIncrement = 0.05f;
                TileData current = area.Dequeue();

                foreach (Vector2Int neighbour in GetNeighbours(current.mapCoords))
                {
                    TileData next = GetTileData(neighbour);
                    if (next.isInit)
                    {
                        if (next.Terrain != current.Terrain)
                        {
                            chance -= chanceIncrement;
                            chance = Mathf.Clamp(chance, 0.2f, 0.8f);
                        }
                        else
                        {
                            chance += chanceIncrement;
                            chance = Mathf.Clamp(chance, 0.2f, 0.8f);
                        }
                    }
                }
                foreach (Vector2Int neighbour in GetNeighbours(current.mapCoords))
                {
                    TileData next = GetTileData(neighbour);
                    if (next.isInit)
                        continue;
                    if (alreadyRolled.Contains(next)) 
                        continue;

                    bool roll = Random.value <= chance;
                    if (roll)
                    {
                        next.SetLand(current.Terrain, current.Elevation);
                        area.Enqueue(next);
                        notSet.Remove(next);
                    }
                    else
                    {
                        alreadyRolled.Add(next);
                    }
                }
            }
        }
        return grid;
    }
    public IEnumerable<TileData> GenerateByAreaSteps() // 0.0 Genrator -> equal random values for all types
    {
        grid = new TileData[gridX, gridY];
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                Vector2Int tilePos = new(x, y);
                grid[x, y] = new TileData(tilePos);
            }
        }
        List<TileData> notSet = new();
        foreach (TileData tile in grid)
        {
            notSet.Add(tile);
        }

        while (notSet.Count > 0)
        {
            TileData start = notSet[UnityEngine.Random.Range(0, notSet.Count)];
            SetTileLand(start);
            notSet.Remove(start);

            yield return start;

            Queue<TileData> area = new();
            area.Enqueue(start);

            List<TileData> alreadyRolled = new();
            while (area.Any())
            {
                float chance = Random.Range(0.45f, 0.55f);
                float chanceIncrement = 0.05f;
                TileData current = area.Dequeue();

                foreach (Vector2Int neighbour in GetNeighbours(current.mapCoords))
                {
                    TileData next = GetTileData(neighbour);
                    if (next.isInit)
                    {
                        if (next.Terrain != current.Terrain)
                        {
                            chance -= chanceIncrement;
                            chance = Mathf.Clamp(chance, 0.2f, 0.8f);
                        }
                        else
                        {
                            chance += chanceIncrement;
                            chance = Mathf.Clamp(chance, 0.2f, 0.8f);
                        }
                    }
                }
                foreach (Vector2Int neighbour in GetNeighbours(current.mapCoords))
                {
                    TileData next = GetTileData(neighbour);
                    if (next.isInit)
                        continue;
                    if (alreadyRolled.Contains(next))
                        continue;

                    bool roll = Random.value <= chance;
                    if (roll)
                    {
                        next.SetLand(current.Terrain, current.Elevation);
                        area.Enqueue(next);
                        notSet.Remove(next);

                        yield return next;
                    }
                    else
                    {
                        alreadyRolled.Add(next);
                    }
                }
            }
        }
    }
    public void GenerateTiles() // 0.0 Genrator -> equal random values for all types
    {
        grid = new TileData[gridX, gridY];

        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
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

                grid[x, y] = new TileData(tilePos, terrain, elevation);

                if (terrain != TerrainType.Water)
                    PopulateWorldObjects(grid[x, y]);
            }
        }
    }
    private void PopulateWorldObjects(TileData tile)
    {
        var spawnableDefs = objData.definitions.Where(def => def.spawnOnWorldGen).ToArray();

        TileObjectDefinition def = spawnableDefs[Random.Range(0, spawnableDefs.Length)];

        WorldObject obj = new(def, tile.mapCoords);

        obj.harvestSource = new HarvestSource(def.GenerateResources());

        tile.AddEntity(obj);
    }
}
