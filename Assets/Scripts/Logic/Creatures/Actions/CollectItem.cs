using UnityEngine;

public class CollectItem : IAction
{
    public float progress = 0f;
    public float unitProgress = 0f;
    int targetAmount;
    float speed;
    public bool IsFinished => progress >= 1f || WasCanceled;
    public bool WasCanceled { get; private set; }

    public TileObject targetObj;
    VirtualResources inventory = null;
    ItemDefinition targetItem;
    CharacterSheet stats;

    public CollectItem(TileObject targetObj, ItemDefinition targetItem, CharacterSheet stats)
    {
        this.stats = stats;
        inventory = stats.inventory;
        this.targetObj = targetObj;
        this.targetItem = targetItem;
        
        //Set stats
        this.speed = stats.harvestSpeed;
    }

    public void Start()
    {        
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
        if (inventory.CalculateWeight(targetItem) >= stats.carryWeight)
        {
            EventBus.Log("Can't carry any more.");
            Cancel();
            return;
        }
        
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
        WasCanceled = true;
    }
}
