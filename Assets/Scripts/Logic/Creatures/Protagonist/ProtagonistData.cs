using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ProtagonistData
{
    public Vector2Int mapCoords { get; private set; }

    public Pathfinder pathfinder { get; private set; }

    public CharacterActions actions;
    public ProtagonistData(Vector2Int mapCoords, float hourDuration, World world, RenderWorld render)
    {
        this.mapCoords = mapCoords;
        actions = new CharacterActions(hourDuration, world, this, render);
    }    
    public void Tick(float deltaTime)
    {
        actions.Tick(deltaTime);   
    }
    public void MoveTo(Vector2Int coords)
    {
        mapCoords = coords;
    }
 
  
}
