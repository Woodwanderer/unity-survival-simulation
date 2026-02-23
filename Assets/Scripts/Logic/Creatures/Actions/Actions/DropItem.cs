public class DropItem : IAction
{
    //External Data
    CharacterBrain brain;
    ItemSlot source;

    //Deriveratives
    World world;
    float speed;

    //Generic IAction
    public ActionToken Token { get; set; }
    public ActionStatus Status { get; private set; } = ActionStatus.NotStarted;

    public float progress;
    float unitProgress;

    ResourcePile resPile;
    int targetAmount;
    public bool IsFinished => Status == ActionStatus.Succeeded || Status == ActionStatus.Cancelled;
    public DropItem(ItemSlot source, CharacterBrain brain)
    {
        this.world = brain.world;
        this.source = source;
        this.speed = brain.stats.harvestSpeed;
    }
    
    public void Start()
    {
        targetAmount = source.Amount;

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

            if (resPile != null)
            {
                int overflow = resPile.Add();
                if (overflow > 0)
                {
                    resPile = EstablishPile(overflow);
                }
            }
            if (resPile == null)
                EstablishPile(1);
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
