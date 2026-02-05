using UnityEngine;

public class DropItem : IAction
{
    //External Data
    World world;
    ItemSlot source;
    //Deriveratives
    RenderWorld render;
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
        
        this.render = world.render;
    }
    
    public void Start()
    {
        targetAmount = source.Amount;

        EstablishPile(1);

        Status = ActionStatus.Running;

    }
    public void Tick(float dt)
    {
        unitProgress += dt * speed;
        progress += dt * speed / targetAmount;

        while (unitProgress >= 1f)
        {
            unitProgress -= 1;
            source.Remove(1);

            //TODO: check for overflow
            resPile.Add(source.Item, 1);        
        }

        if (source.IsEmpty)
            Status = ActionStatus.Succeeded;

    }
    ResourcePile EstablishPile(int amount)
    {
        TileData tile = world.GetProtagonistTileData();
        ResourcePile pile = tile.FindInPiles(source.Item);
        if (pile != null)
        {
            pile.Add(source.Item, amount);
            resPile = pile;
        }
        else
        {
            resPile = world.CreateResourcePile(tile, source.Item, amount);
            render.SpawnResourcePile(resPile);
        }
        return resPile;
    }
    public void Cancel()
    {
        Status = ActionStatus.Cancelled;
    }
}
