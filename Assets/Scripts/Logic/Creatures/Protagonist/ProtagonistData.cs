using UnityEngine;
public class ProtagonistData
{
    public Vector2Int mapCoords { get; private set; }

    public Pathfinder pathfinder { get; private set; }

    public CharacterActions actions;
    public ProtagonistData(Vector2Int mapCoords, World world, RenderWorld render)
    {
        this.mapCoords = mapCoords;
        actions = new CharacterActions(world, this, render);
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
