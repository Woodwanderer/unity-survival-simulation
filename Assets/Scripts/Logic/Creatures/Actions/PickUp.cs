using UnityEngine;

public class PickUp :IAction
{
    public float progress = 0f;
    public float unitProgress = 0f;
    int targetAmount;
    float speed;
    public bool IsFinished => progress >= 1f || WasCanceled;
    public bool WasCanceled { get; private set; }

    IItemContainer target;
    Inventory inventory = null;
    ItemSlot order;
    CharacterSheet stats;
    public PickUp(IItemContainer target, ItemSlot order, CharacterSheet stats)
    {
        this.stats = stats;
        inventory = stats.inventory;
        this.target = target;
        this.order = order;

        //Set stats
        this.speed = stats.harvestSpeed;
    }
    public void Start()
    {
        unitProgress = 0f;
        progress = 0f;
        targetAmount = order.Amount;
    }

    public void Tick(float dt)
    {
        if (inventory.CalculateWeight(order.Item) >= stats.carryWeight)
        {
            Cancel();
            return;
        }

        unitProgress += dt * speed;
        progress += dt * speed / targetAmount;

        while (unitProgress >= 1f)
        {
            unitProgress -= 1;
            target.Remove(order.Item, 1);
            inventory.Add(order.Item, 1);
        }
    }

    public void Cancel()
    {
        WasCanceled = true;
    }
}
