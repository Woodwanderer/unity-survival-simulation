public class HarvestAction : IAction
{
    //External Data
    public WorldObject targetObj;
    ItemSlot order;
    float speed;
    World world;

    //Generic IAction
    public ActionToken Token { get; set; }
    public ActionStatus Status { get; private set; } = ActionStatus.NotStarted;
    public bool IsFinished => Status == ActionStatus.Succeeded || Status == ActionStatus.Cancelled;

    public float progress = 0f;
    public float unitProgress = 0f;
    int targetAmount;
    
    ResourcePile resPile = null;
    
    public HarvestAction(WorldObject wo, ItemSlot order, float speed, World world)
    {
        this.targetObj = wo;
        this.order = order;
        this.world = world;

        //Set stats
        this.speed = speed;
    }
    
    public void Start()
    {
        unitProgress = 0f;
        progress = 0f;
        targetAmount = order.Amount;

        Status = ActionStatus.Running;
    }

    public void Tick(float dt)
    {
        if (!targetObj.isValid || progress >= 1f) 
        {
            Status = ActionStatus.Succeeded;
            return;
        }

        unitProgress += dt * speed;
        progress += dt * speed / targetAmount;

        while(unitProgress >= 1f)
        {
            unitProgress -= 1;
            targetObj.harvestSource.Harvest(order.Item, 1);

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
    }
    ResourcePile EstablishPile(int amount)
    {
        TileData tile = world.GetTileData(targetObj.TileCoords);
        ResourcePile pileObj = tile.FindInPiles(order.Item);
        if (pileObj != null)
        {
            pileObj.Add(amount);
            resPile = pileObj;
        }
        else
        {
            ItemSlot slot = new(order.Item, amount);
            resPile = world.CreateResourcePile(tile, slot);
        }
        return resPile;
    }
    public void Cancel()
    {
        Status = ActionStatus.Cancelled;
    }
}
