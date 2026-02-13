using System.Collections.Generic;
public class BuildPlan : IPlan
{
    //External Data
    CharacterBrain brain;
    Building building;
    public BuildPlan(CharacterBrain brain, Building building)
    {
        this.brain = brain;
        this.building = building;
    }
    public IEnumerable<IAction> Build()
    {
        yield return new Movement(brain.world, building.Area);
        yield return new BuildAction(building, brain.stats, brain.world);
    }
}
