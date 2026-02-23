using System.Collections.Generic;
public class DeliverPlan : IPlan
{
    //External Data
    ItemSlot order;
    CharacterBrain brain;
    Stockpile destination;
    public DeliverPlan(CharacterBrain brain, Stockpile destination, ItemSlot order)
    {
        this.brain = brain;
        this.destination = destination;
        this.order = order;
    }
    public IEnumerable<IAction> Build()
    {
        //Move to Stockpile
        yield return new Movement(brain.world, destination.area);

        yield return new Deliver(brain.inventory, brain.stats, destination, order);
    }
}
