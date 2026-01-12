using UnityEngine;

public class HarvestAction : IAction
{
    public float progress = 0f;
    public float unitProgress = 0f;
    int targetAmount;
    float speed;
    public bool IsFinished => progress >= 1f;
    
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
        if (targetObj.resources == null)
        {
            EventBus.Log("I can't harvest that.");
            Cancel();   
        }
            
        unitProgress = 0f;
        progress = 0f;
        targetAmount = targetObj.resources.Get(targetItem);
        if(targetAmount <= 0 )
        {
            EventBus.Log("target is depleted.");
            Cancel();
        }
        resPile = EstablishPile(0);
    }

    public void Tick(float dt)
    {
        unitProgress += dt * speed;
        progress += dt * speed / targetAmount;

        while(unitProgress >= 1f)
        {
            unitProgress -= 1;
            targetObj.resources.Remove(targetItem, 1);
            int overflow = resPile.pile.Add(1);
            if (overflow > 0) 
            {
                resPile = EstablishPile(overflow);
            }
        }
    }

    TileObject EstablishPile(int amount = 0)
    {
        TileData tile = world.GetTileData(targetObj.tileCoords);
        TileObject pileObj = tile.ContainsPileOf(targetItem);
        if (pileObj != null)
        {
            pileObj.pile.Add(amount);
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
      
    }
}
