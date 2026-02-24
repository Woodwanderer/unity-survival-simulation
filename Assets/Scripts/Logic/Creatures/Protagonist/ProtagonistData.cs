using UnityEngine;
public class ProtagonistData
{
    public Vector2Int mapCoords { get; private set; }

    public Pathfinder pathfinder { get; private set; }

    public CharacterBrain brain;
    public ProtagonistData(Vector2Int mapCoords, World world)
    {
        this.mapCoords = mapCoords;
        brain = new CharacterBrain(world, this);
    }    
    public void Tick(float deltaTime)
    {
        brain.Tick(deltaTime);   
    }
    public void MoveTo(Vector2Int coords)
    {
        mapCoords = coords;
    }
 
  
}
