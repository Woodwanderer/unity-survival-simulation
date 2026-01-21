using UnityEngine;
using System.Collections.Generic;

public class Pathfinder //BFS -> Deijkstra later ;)
{
    World world;
    public Pathfinder(World world)
    {
        this.world = world;
    }
    static readonly Vector2Int[] Directions =
    {
        new Vector2Int( 1, 0 ),
        new Vector2Int( 0,-1 ),
        new Vector2Int(-1, 0 ),
        new Vector2Int( 0, 1 ),
    };
    public bool IsWithinWorld(Vector2Int pos)
    {
        return ( pos.x >= 0                &&
                 pos.y >= 0                &&
                 pos.x < world.WorldSize.x &&
                 pos.y < world.WorldSize.y
               );
    }
    IEnumerable<Vector2Int> GetNeighbours(Vector2Int pos)
    {
        foreach (Vector2Int dir in Directions)
        {
            Vector2Int next = pos + dir;

            if (!IsWithinWorld(next))
                continue;
            if (!world.GetTileData(next).isWalkable)
                continue;

            yield return next;
        }
    }
    public List<Vector2Int> FloodFill(Vector2Int start, Vector2Int b)
    {
        TileRect rect = new(start, b);
        List<Vector2Int> result = new();
        Queue<Vector2Int> frontier = new();
        HashSet<Vector2Int> visited = new();

        if (!rect.Contains(start)) 
            return result;
        if (!world.GetTileData(start).isWalkable)
            return result;
        if (world.GetTileData(start).HasBuilding) 
            return result;

        frontier.Enqueue(start);
        visited.Add(start);

        while (frontier.Count > 0)
        {
            Vector2Int current = frontier.Dequeue();
            result.Add(current);

            foreach(Vector2Int next in GetNeighbours(current))
            {
                if (visited.Contains(next))
                    continue;

                if (!rect.Contains(next)) 
                    continue;

                if (world.GetTileData(next).HasBuilding) 
                    continue;

                visited.Add(next);
                frontier.Enqueue(next);
            }
        }
        return result;
    }
    public TileEntity FindEntity(Vector2Int start, ItemDefinition item)
    {
        Queue<Vector2Int> frontier = new();
        HashSet<Vector2Int> visited = new();

        frontier.Enqueue(start);
        visited.Add(start);

        TileEntity ent = world.GetTileData(start).Contains(item);

        if (ent != null) 
            return ent;

        while (frontier.Count > 0)
        {
            Vector2Int current = frontier.Dequeue();

            foreach (Vector2Int next in GetNeighbours(current))
            {
                if (visited.Contains(next))
                    continue;

                visited.Add(next);

                TileEntity found = world.GetTileData(next).Contains(item);
                if (found != null)
                {
                    return found;
                }
                frontier.Enqueue(next);
            }
        }
        return null;
    }
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        List<Vector2Int> path = new();

        Queue<Vector2Int> frontier = new();
        frontier.Enqueue(start);

        Dictionary<Vector2Int, Vector2Int> cameFrom = new();
        cameFrom[start] = start;

        while (frontier.Count > 0)
        {
            Vector2Int current = frontier.Dequeue();

            if (current == target)
                break;

            foreach (Vector2Int next in GetNeighbours(current))
            {
                if (cameFrom.ContainsKey(next)) 
                    continue;

                cameFrom[next] = current;
                frontier.Enqueue(next);
            }
        }

        if (!cameFrom.ContainsKey(target)) 
            return null; //no path

        return ReconstructPath(cameFrom, start, target);
    }
    List<Vector2Int> ReconstructPath(Dictionary<Vector2Int,Vector2Int> cameFrom, Vector2Int start, Vector2Int target)
    {
        List<Vector2Int> path = new();

        Vector2Int currentStep = target;
        while(currentStep != start) // List is without starting position
        {
            path.Add(currentStep);
            currentStep = cameFrom[currentStep];
        }
        path.Reverse();
        return path;
    }
    public List<Vector2Int> GetPathSteps(Vector2Int startPos, List<Vector2Int> coords)
    {
        List<Vector2Int> pathSteps = new List<Vector2Int>();

        Vector2Int previous = startPos;
        foreach(Vector2Int next in coords)
        {

            pathSteps.Add(next - previous);
            previous = next;
        }
        return pathSteps;
    }

}
