using UnityEngine;

public class HarvestAction : IAction
{
    public float progress = 0f;
    public float unitProgress = 0f;
    int targetAmount;
    float speed;
    public bool IsFinished => progress >= 1f;
    
    public TileObject targetObj;
    ItemDefinition targetItem;
    VirtualResources inventory;

    public HarvestAction(TileObject targetObj, ItemDefinition targetItem, float speed, VirtualResources inventory )
    {
        this.targetObj = targetObj;
        this.targetItem = targetItem;
        this.inventory = inventory;

        //Set stats
        this.speed = speed;
    }

    public void Start()
    {
        unitProgress = 0f;
        progress = 0f;
        targetAmount = targetObj.Resources.Get(targetItem);
    }

    public void Tick(float dt)
    {
        if (!targetObj.Resources.Has(targetItem))
            return; // just a failsafe

        unitProgress += dt * speed;
        progress += dt * speed / targetAmount;

        while(unitProgress >= 1f)
        {
            unitProgress -= 1;
            targetObj.Resources.Remove(targetItem, 1);
            inventory.Add(targetItem, 1);
        }
    }
    public void Cancel()
    {

    }
}
