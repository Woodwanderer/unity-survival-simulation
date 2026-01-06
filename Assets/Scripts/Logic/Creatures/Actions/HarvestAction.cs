using UnityEngine;

public class HarvestAction : IAction
{
    public float progress = 0f;
    public float unitProgress = 0f;
    int targetAmount;
    float speed;
    public bool IsFinished => progress >= 1f;
    
    public TileObject targetObj;
    TileObject stockpile = null;
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
        targetAmount = targetObj.resources.Get(targetItem);
        if(targetAmount <= 0 )
        {
            progress = 1f;
            EventBus.Log("target is depleted.");
            return;
        }

        CreateNewPile(0);
    }

    public void Tick(float dt)
    {
        if (!targetObj.resources.Has(targetItem))
        {
            progress = 1f;
            return;
        }
            
        unitProgress += dt * speed;
        progress += dt * speed / targetAmount;

        while(unitProgress >= 1f)
        {
            unitProgress -= 1;
            targetObj.resources.Remove(targetItem, 1);
            int overflow = stockpile.pile.Add(1);
            if (overflow > 0) 
            {
                stockpile = CreateNewPile(overflow);
            }
        }
    }

    TileObject CreateNewPile(int amount = 0)
    {
        TileData tile = world.GetTileData(targetObj.tileCoords);
        stockpile = world.CreateResourcePile(tile, targetItem, amount);
        render.SpawnResourcePile(stockpile);
        return stockpile;
    }
    public void Cancel()
    {

    }
}
