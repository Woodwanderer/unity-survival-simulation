using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ProtagonistData
{
    float hourDuration;
    public Vector2Int mapCoords { get; private set; }

    public Pathfinder pathfinder { get; private set; }
    
    public CharacterSheet charSheet;
    public float speed { get; private set; } = 2.0f;

    //Actions
    public CharacterActionState GetActionState()
    {
        return charSheet.actions.State;
    }
    //Movement
    public List<Vector2Int> pathCoords = new List<Vector2Int>();
    public List<Vector2Int> pathSteps = new List<Vector2Int>();
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
    public void MoveByStep(Vector2Int step)
    {
        mapCoords += step;
    }
 
  
}
