using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ProtagonistData
{
    public Vector2Int mapCoords { get; private set; }
    public ProtagonistRoute route;
    //STATS :)
    public float speed { get; private set; } = 2.0f;

    public ProtagonistData(Vector2Int mapCoords)
    {
        this.mapCoords = mapCoords;
        route = new ProtagonistRoute();
    }    
    public void SetRouteTo(Vector2Int targetCoords)
    {
        SetRouteStepsTo(targetCoords);
        SetRouteCoords();
    }
    private void SetRouteStepsTo(Vector2Int targetCoords)
    {
        route.SetSteps(targetCoords - mapCoords);
    }
    private void SetRouteCoords()
    {
        route.SetPathCoordsFrom(mapCoords);
    }
    public void MoveByStep(Vector2Int step)
    {
        mapCoords += step;
    }
}
