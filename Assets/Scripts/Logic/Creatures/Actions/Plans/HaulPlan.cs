using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
public class HaulPlan : IPlan
{
    CharacterActions actions;
    HaulTask ht;
    World world;
    HaulPlan(CharacterActions actions, HaulTask haulTask)
    {
        this.actions = actions;
        this.ht = haulTask;

        world = actions.world;
    }
    public IEnumerable<IAction> Build()
    {
        //Move to Task Sequenece Location
        yield return new Movement(world, ht.source.TileCoords);
    }

    /*public void TryHaul(HaulTask ht)
    {

        IAction collect = new CollectItem(ht.source, ht.source.Slot, stats);
        if (ht.source.TileCoords == protagonistData.mapCoords)
            actionRunner.SetAction(collect);
        else
        {
            bool canMove = TryMoveToTile(ht.source.TileCoords);
            if (canMove)
            {
                actionRunner.actionQueue.Enqueue(collect);
                IAction moveToStockpile = new Movement(protagonistData, render, stats.Speed, ht.deliveryPath);
                actionRunner.actionQueue.Enqueue(moveToStockpile);
                IAction deliver = new Deliver(inventory, stats, ht.destination);
                actionRunner.actionQueue.Enqueue(deliver);
            }
            else
                EventBus.Log("I can't reach this destination.");
        }
    }*/

}
