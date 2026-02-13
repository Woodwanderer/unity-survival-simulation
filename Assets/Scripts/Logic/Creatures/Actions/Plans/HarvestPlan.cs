using System.Collections.Generic;
public class HarvestPlan : IPlan
{
    //External Data
    CharacterBrain brain;
    TileEntity target;
    ItemSlot order;
    public HarvestPlan(CharacterBrain brain, TileEntity target, ItemSlot order)
    {
        this.brain = brain;
        this.target = target;
        this.order = order;
    }  
    public IEnumerable<IAction> Build()
    {
        yield return new Movement(brain.world, target.TileCoords);

        IAction transfer = null;

        if (target is ResourcePile pile && pile != null)
        {
            transfer = new CollectItem(pile, order, brain.stats);
        }
        else if (target is WorldObject wo)
        {
            transfer = new HarvestAction(wo, order, brain.stats.harvestSpeed, brain.world);
        }
        if (transfer == null) 
            yield break;

        yield return transfer;
    }
}
