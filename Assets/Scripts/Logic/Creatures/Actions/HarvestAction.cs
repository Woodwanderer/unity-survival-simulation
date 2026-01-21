using UnityEngine;

public class HarvestAction : IAction
{
    public float progress = 0f;
    public float unitProgress = 0f;
    int targetAmount;
    float speed;
    bool wasCanceled;
    public bool IsFinished => progress >= 1f || wasCanceled;
    
    public WorldObject targetObj;
    ResourcePile resPile = null;
    ItemSlot order;
    World world;
    RenderWorld render;

    public HarvestAction(WorldObject wo, ItemSlot order, float speed, World world, RenderWorld render)
    {
        this.targetObj = wo;
        this.order = order;
        this.world = world;
        this.render = render;

        //Set stats
        this.speed = speed;
    }
    public void Start()
    {
        unitProgress = 0f;
        progress = 0f;
        targetAmount = order.Amount;
    }
    public void Tick(float dt)
    {
        unitProgress += dt * speed;
        progress += dt * speed / targetAmount;

        while(unitProgress >= 1f)
        {
            unitProgress -= 1;
            targetObj.harvestSource.Harvest(order.Item, 1);

            if (resPile == null)
                resPile = EstablishPile(1);
            else
            {
                int overflow = resPile.Add(order.Item, 1);
                if (overflow > 0)
                {
                    resPile = EstablishPile(overflow);
                }
            }
        }
    }
    ResourcePile EstablishPile(int amount)
    {
        TileData tile = world.GetTileData(targetObj.TileCoords);
        ResourcePile pileObj = tile.FindInPiles(order.Item);
        if (pileObj != null)
        {
            pileObj.Add(order.Item, amount);
            resPile = pileObj;
        }
        else
        {
            resPile = world.CreateResourcePile(tile, order.Item, amount);
            render.SpawnResourcePile(resPile);
        }
        return resPile;
    }
    public void Cancel()
    {
        wasCanceled = true;
    }
}
