using NUnit.Framework;
using System.Collections;
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
    bool IsWithinWorld(Vector2Int pos)
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
