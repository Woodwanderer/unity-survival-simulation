using UnityEngine;

public class CollectItem : IAction
{
    public float progress = 0f;
    public float unitProgress = 0f;
    int targetAmount;
    float speed;
    public bool IsFinished => progress >= 1f || WasCanceled;
    public bool WasCanceled { get; private set; }

    public ResourcePile pile;
    Inventory inventory = null;
    ItemSlot order;
    CharacterSheet stats;
    public CollectItem(ResourcePile pile, ItemSlot order, CharacterSheet stats)
    {
        this.stats = stats;
        inventory = stats.inventory;
        this.pile = pile;
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
            pile.Remove(order.Item, 1);
            inventory.Add(order.Item, 1);
        }
    }

    public void Cancel()
    {
        WasCanceled = true;
    }
}
