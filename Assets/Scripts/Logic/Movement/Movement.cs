using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    ProtagonistData data;
    RenderWorld render;
    
    List<Vector2Int> path = new();
    float speed;
    float moveT;
    int pathIndex = 0;
    bool isMoving => pathIndex < path.Count;
    public bool Moving => isMoving;

    Vector3 fromPos;
    Vector3 toPos;

    public Movement(ProtagonistData data, RenderWorld render)
    {
        this.data = data;
        this.render = render;
    }

    public void Tick(float dt)
    {
        if (!isMoving) 
            return;

        MoveInTime(dt);
    }
    public void SetPath(List<Vector2Int> newPath)
    {
        ClearPath();
        path = newPath;

        render.DrawPath(newPath, true);
        pathIndex = 0;
        moveT = 1;
        // Get Stats
        speed = data.charSheet.speed; //gest new speed cosue it may change in play time (in the future - i.e. some speed haste buff or being tired or wounded)
    }

    void MoveInTime(float dt)
    {
        while (moveT >= 1) //Set new step
        {
            render.ShowTilePath(data.mapCoords, false);
            fromPos = render.GetProtagonistLocation();
            toPos = render.MapToWorld(path[pathIndex]);
            moveT = 0;
        }

        moveT += dt * speed;

        float t = Mathf.Clamp01(moveT);
        render.protagonist.transform.position = Vector3.Lerp(fromPos, toPos, t);

        if(t >= 1)
        {
            data.MoveTo(path[pathIndex]);
            render.ShowTilePath(data.mapCoords, false);
            pathIndex++;
        }
    }
    public void ClearPath()
    {
        render.DrawPath(path, false);
        path.Clear();
        pathIndex = 0;
        moveT = 0;
    }

}
