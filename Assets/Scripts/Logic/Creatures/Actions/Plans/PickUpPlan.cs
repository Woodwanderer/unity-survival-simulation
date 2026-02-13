using System.Collections.Generic;
public class PickUpPlan : IPlan
{
    CharacterBrain brain;
    ItemSlot order;
    Stockpile from;
    public PickUpPlan(CharacterBrain brain, ItemSlot order, Stockpile stockpile)
    {
        this.brain = brain;
        this.order = order;
        this.from = stockpile;
    }
    public IEnumerable<IAction> Build()
    {
        yield return new Movement(brain.world, from.area.center);
        yield return new PickUp(from, order, brain.stats);
    }
}
