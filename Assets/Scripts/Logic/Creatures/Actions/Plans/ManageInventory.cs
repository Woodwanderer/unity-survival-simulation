using System.Collections.Generic;
public class ManageInventory : IPlan
{
    World world;
    ItemSlot order;
    Stockpile destination;
    public ManageInventory(ItemSlot order, Stockpile destination, World world)
    {
        this.world = world;
        this.order = order;
        this.destination = destination;
    }
    public IEnumerable<IAction> Build()
    {
        yield return new Movement(world, destination.area);
        
    }
}
