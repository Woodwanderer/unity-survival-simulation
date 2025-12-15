using UnityEngine;

public class CreatureData
{
    public Vector2Int mapCoords;
    //STATS
    public float speed = 4f;

    public CreatureData(Vector2Int pos)
    { 
        this.mapCoords = pos; 
    }
    public void MoveByStep(Vector2Int step)
    {
        mapCoords += step;
    }
}
