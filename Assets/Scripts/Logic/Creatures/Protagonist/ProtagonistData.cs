using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ProtagonistData
{
    float hourDuration;
    public Vector2Int mapCoords { get; private set; }
    public ProtagonistRoute route = new();

    //STATS :)
    public CharacterSheet charState;
    public float speed { get; private set; } = 2.0f;

    //Actions
    public Actions actions = new Actions();
    public ProtagonistData(Vector2Int mapCoords, float hourDuration)
    {
        this.hourDuration = hourDuration;
        this.mapCoords = mapCoords;
        charState = new CharacterSheet(hourDuration);
    }    
    public void Tick(float deltaTime)
    {
        charState.Tick(deltaTime);
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
