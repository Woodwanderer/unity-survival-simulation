using UnityEngine;

public class CollectItem : IAction
{
    public float progress = 0f;
    public float unitProgress = 0f;
    int targetAmount;
    float speed;
    public bool IsFinished => progress >= 1f;

    public TileObject targetObj;
    VirtualResources inventory = null;
    ItemDefinition targetItem;
    RenderWorld render;

    public CollectItem(TileObject targetObj, ItemDefinition targetItem, float speed, VirtualResources inv)
    {
        inventory = inv;
        this.targetObj = targetObj;
        this.targetItem = targetItem;
        
        //Set stats
        this.speed = speed;
    }

    public void Start()
    {
        if (targetObj.pile == null)
        {
            EventBus.Log("I can't pick that.");
            return;
        }

        unitProgress = 0f;
        progress = 0f;
        targetAmount = targetObj.pile.amount;
        if (targetAmount <= 0)
        {
            progress = 1f;
            EventBus.Log("No more items here.");
            return;
        }
    }

    public void Tick(float dt)
    {
        unitProgress += dt * speed;
        progress += dt * speed / targetAmount;

        while (unitProgress >= 1f)
        {
            unitProgress -= 1;
            targetObj.pile.Remove(1);
            inventory.Add(targetItem, 1);
        }
    }

    public void Cancel()
    {

    }
}
