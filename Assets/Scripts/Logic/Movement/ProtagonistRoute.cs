using System.Collections.Generic;
using UnityEngine;

public class ProtagonistRoute
{
    public List<Vector2Int> pathSteps { get; private set; }
    public List<Vector2Int> pathCoords { get; private set; }
    private MovementRasterizer rasterizer;
    public ProtagonistRoute()
    {
        rasterizer = new MovementRasterizer();
        pathSteps = new List<Vector2Int>();
        pathCoords = new List<Vector2Int>();
    }
    public void SetSteps(Vector2Int deltaPos)
    {
        pathSteps.Clear();
        pathSteps = rasterizer.RasterizeMovement(deltaPos);
    }
    public void SetPathCoordsFrom(Vector2Int startCoords)
    {
        pathCoords.Clear();
        Vector2Int current = startCoords;
        foreach (Vector2Int step in pathSteps)
        {
            current += step;
            pathCoords.Add(current);
        }
    }
    public void Clear()
    {
        pathSteps.Clear();
        pathCoords.Clear();
    }
}
