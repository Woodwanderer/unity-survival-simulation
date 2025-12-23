using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ProtagonistData
{
    float hourDuration;
    public Vector2Int mapCoords { get; private set; }
    public ProtagonistRoute route = new();
    
    public CharacterSheet charSheet;
    public float speed { get; private set; } = 2.0f;

    //Actions
    bool actionChanged = false;
    public CharacterActionState GetActionState()
    {
        return charSheet.actions.State;
    }
    public ProtagonistData(Vector2Int mapCoords, float hourDuration, VirtualResources global)
    {
        this.hourDuration = hourDuration;
        this.mapCoords = mapCoords;
        charSheet = new CharacterSheet(hourDuration, global);
    }    
    public void Tick(float deltaTime)
    {
        charSheet.Tick(deltaTime);
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
