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
    ItemDefinition targetItem;
    World world;
    RenderWorld render;

    public HarvestAction(WorldObject wo, ItemDefinition targetItem, float speed, World world, RenderWorld render)
    {
        this.targetObj = wo;
        this.targetItem = targetItem;
        this.world = world;
        this.render = render;

        //Set stats
        this.speed = speed;
    }
    public void Start()
    {
        unitProgress = 0f;
        progress = 0f;
        targetAmount = targetObj.harvestSource.Get(targetItem);
    }
    public void Tick(float dt)
    {
        unitProgress += dt * speed;
        progress += dt * speed / targetAmount;

        while(unitProgress >= 1f)
        {
            unitProgress -= 1;
            targetObj.harvestSource.Harvest(targetItem, 1);

            if (resPile == null)
                resPile = EstablishPile(1);
            else
            {
                int overflow = resPile.Add(targetItem, 1);
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
        ResourcePile pileObj = tile.FindInPiles(targetItem);
        if (pileObj != null)
        {
            pileObj.Add(targetItem, amount);
            resPile = pileObj;
        }
        else
        {
            resPile = world.CreateResourcePile(tile, targetItem, amount);
            render.SpawnResourcePile(resPile);
        }
        return resPile;
    }
    public void Cancel()
    {
        wasCanceled = true;
    }
}
