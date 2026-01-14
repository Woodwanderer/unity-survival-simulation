using UnityEngine;

public class HarvestAction : IAction
{
    public float progress = 0f;
    public float unitProgress = 0f;
    int targetAmount;
    float speed;
    bool wasCanceled;
    public bool IsFinished => progress >= 1f || wasCanceled;
    
    public TileObject targetObj;
    TileObject resPile = null;
    ItemDefinition targetItem;
    World world;
    RenderWorld render;

    public HarvestAction(TileObject targetObj, ItemDefinition targetItem, float speed, World world, RenderWorld render)
    {
        this.targetObj = targetObj;
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

        resPile = EstablishPile(0);
    }

    public void Tick(float dt)
    {
        unitProgress += dt * speed;
        progress += dt * speed / targetAmount;

        while(unitProgress >= 1f)
        {
            unitProgress -= 1;
            targetObj.harvestSource.Harvest(targetItem, 1);
            int overflow = resPile.itemSlot.Add(targetItem, 1);
            if (overflow > 0) 
            {
                resPile = EstablishPile(overflow);
            }
        }
    }

    TileObject EstablishPile(int amount = 0)
    {
        TileData tile = world.GetTileData(targetObj.tileCoords);
        TileObject pileObj = tile.ContainsItemSlotOf(targetItem);
        if (pileObj != null)
        {
            pileObj.itemSlot.Add(targetItem, amount);
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
