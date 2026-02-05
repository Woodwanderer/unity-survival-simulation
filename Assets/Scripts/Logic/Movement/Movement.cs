using System.Collections.Generic;
using UnityEngine;

public class Movement : IAction
{
    //Exterior data
    World world;
    Vector2Int destination;
    Area targetArea;
    List<Vector2Int> path = new();

    //Deriveratives
    ProtagonistData data;
    CharacterActions brain;
    RenderWorld render;

    //Action Generic
    public ActionToken Token {  get; set; }
    public ActionStatus Status { get; private set; } = ActionStatus.NotStarted;
    public bool IsFinished => Status == ActionStatus.Succeeded;

    MoveMode mode;
    float speed;
    float moveT;
    int pathIndex = 0;

    Vector3 fromPos;
    Vector3 toPos;

    enum MoveMode
    {
        ToTile,
        ToArea,
        ByPath
    }
    void Init()
    {
        data = world.protagonistData;
        brain = data.actions;
        speed = brain.stats.Speed;
        render = world.render;
    }
    public Movement(World world, Vector2Int tileCoords)
    {
        this.world = world;
        destination = tileCoords;
        mode = MoveMode.ToTile;

        Init();
    }
    public Movement(World world, Area destination)
    {
        this.world = world;
        targetArea = destination;
        mode = MoveMode.ToArea;

        Init();
    }
    public Movement(World world, List<Vector2Int> path)
    {
        this.world = world;
        this.path = path;
        mode= MoveMode.ByPath;

        Init();
    }
    public void Start()
    {
        switch(mode)
        {
            case MoveMode.ToTile:
                if (data.mapCoords == destination) 
                {
                    Status = ActionStatus.Succeeded;
                    return;
                }
                path = world.pathfinder.FindPath(data.mapCoords, destination);
                break;

            case MoveMode.ToArea:
                path = world.pathfinder.FindPathToArea(data.mapCoords, targetArea);
                if (path.Count == 0)
                {
                    Status = ActionStatus.Succeeded;
                    return;
                }
                break;

            case MoveMode.ByPath:
                break;
        }

        if (path == null)
        {
            Status = ActionStatus.Failed;
            return;
        }

        Status = ActionStatus.Running;
        render.DrawPath(path, true);
        pathIndex = 0;
        moveT = 1;
    }
    public void Tick(float dt)
    {
        MoveInTime(dt);

        if (pathIndex >= path.Count) 
            Status = ActionStatus.Succeeded;
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
    public void Cancel()
    {
        render.DrawPath(path, false);
        Status = ActionStatus.Cancelled;
    }
}
