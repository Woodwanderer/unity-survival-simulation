using System.Collections.Generic;
using UnityEngine;
public class BuildPlan : IPlan
{
    //External Data
    CharacterBrain brain;
    Building building;
    List<Vector2Int> path;
    public BuildPlan(CharacterBrain brain, Building building, List<Vector2Int> path = null)
    {
        this.brain = brain;
        this.building = building;
        this.path = path;
    }
    public IEnumerable<IAction> Build()
    {
        if (path != null)
            yield return new Movement(brain.world, path);
        else
            yield return new Movement(brain.world, building.Area);

        yield return new BuildAction(building, brain.stats, brain.world);
    }
}
