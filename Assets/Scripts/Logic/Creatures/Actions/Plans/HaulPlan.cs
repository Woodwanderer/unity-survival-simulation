using System.Collections.Generic;
public class HaulPlan : IPlan
{
    //External Data
    CharacterBrain brain;
    HaulTask ht;
    //Deriveratives
    World world;
    public HaulPlan(CharacterBrain brain, HaulTask haulTask)
    {
        this.brain = brain;
        this.ht = haulTask;

        world = brain.world;
    }
    public IEnumerable<IAction> Build()
    {
        yield return new Movement(world, ht.PathToTask);

        yield return new CollectItem(ht.source, ht.source.Slot, brain.stats);

        //Move to Stockpile
        yield return new Movement(world, ht.deliveryPath);

        yield return new Deliver(brain.inventory, brain.stats, ht.destination);
    }
}
