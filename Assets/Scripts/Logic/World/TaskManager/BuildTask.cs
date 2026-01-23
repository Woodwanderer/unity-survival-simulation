using UnityEngine;

public class BuildTask : ITask
{
    public Building building;
    public bool IsValid => !building.IsConstructed;

    public BuildTask(Building building)
    {
        this.building = building;
    }
}
