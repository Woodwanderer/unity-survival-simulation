using UnityEngine;

public class DropItem : IAction
{
    //External Data
    World world;
    ItemSlot source;

    float speed;

    //Generic IAction
    public ActionToken Token { get; set; }
    public ActionStatus Status { get; private set; } = ActionStatus.NotStarted;

    public float progress;
    float unitProgress;

    ResourcePile resPile;
    int targetAmount;
    public bool IsFinished => Status == ActionStatus.Succeeded;
    public DropItem(ItemSlot source, CharacterSheet stats, World world)
    {
        this.world = world;
        this.source = source;
        this.speed = stats.harvestSpeed;
    }
    
    public void Start()
    {
        targetAmount = source.Amount;

        EstablishPile();

        Status = ActionStatus.Running;

    }
    public void Tick(float dt)
    {
        unitProgress += dt * speed;
        progress += dt * speed / targetAmount;

        while (unitProgress >= 1f)
        {
            unitProgress -= 1;
            source.Remove();

            //TODO: check for overflow
            resPile.Add();        
        }

        if (source.IsEmpty)
            Status = ActionStatus.Succeeded;

    }
    ResourcePile EstablishPile(int amount = 1)
    {
        TileData tile = world.GetProtagonistTileData();
        ResourcePile pile = tile.FindInPiles(source.Item);
        if (pile != null)
        {
            pile.Add(amount);
            resPile = pile;
        }
        else
        {
            ItemSlot slot = new(source.Item, amount);
            resPile = world.CreateResourcePile(tile, slot);
        }
        return resPile;
    }
    public void Cancel()
    {
        Status = ActionStatus.Cancelled;
    }
}
