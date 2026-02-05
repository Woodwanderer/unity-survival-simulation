using UnityEngine;

public class PickUp :IAction
{
    //External Data
    IItemContainer target;
    ItemSlot order;
    CharacterSheet stats;
    //Deriveratives
    Inventory inventory = null;
    float speed;

    //Generic IAction
    public ActionToken Token { get; set; }
    public ActionStatus Status { get; private set; } = ActionStatus.NotStarted;
    public bool IsFinished => progress >= 1f || Status == ActionStatus.Cancelled;

    public float progress = 0f;
    public float unitProgress = 0f;
    int targetAmount;
    
    public PickUp(IItemContainer target, ItemSlot order, CharacterSheet stats)
    {
        this.stats = stats;
        this.target = target;
        this.order = order;

        inventory = stats.inventory;
        this.speed = stats.harvestSpeed;
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

        if (progress >= 1f)
            Status = ActionStatus.Succeeded;
    }

    public void Cancel()
    {
        Status = ActionStatus.Cancelled;
    }
}
