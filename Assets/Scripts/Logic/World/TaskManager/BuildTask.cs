using UnityEngine;
using System.Collections.Generic;

public class BuildTask : ITask
{
    public Building building;
    public bool IsValid => !building.IsConstructed;
    public Vector2Int Location => building.Area.center;
    public List<Vector2Int> PathToTask { get; set; }

    public BuildTask(Building building)
    {
        this.building = building;
    }
}
