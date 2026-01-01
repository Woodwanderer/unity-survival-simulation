using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ProtagonistData
{
    float hourDuration;
    public Vector2Int mapCoords { get; private set; }

    public Pathfinder pathfinder { get; private set; }
    
    public CharacterSheet charSheet;
    public ProtagonistData(Vector2Int mapCoords, float hourDuration, VirtualResources global, World world, RenderWorld render)
    {
        this.hourDuration = hourDuration;
        this.mapCoords = mapCoords;
        charSheet = new CharacterSheet(hourDuration, global, world, this, render);
    }    
    public void Tick(float deltaTime)
    {
        charSheet.Tick(deltaTime);

   
    }
    public void MoveTo(Vector2Int coords)
    {
        mapCoords = coords;
    }
 
  
}
