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
    ItemDefinition targetItem;
    CharacterSheet stats;
    public CollectItem(ResourcePile pile, ItemDefinition targetItem, CharacterSheet stats)
    {
        this.stats = stats;
        inventory = stats.inventory;
        this.pile = pile;
        this.targetItem = targetItem;
        
        //Set stats
        this.speed = stats.harvestSpeed;
    }
    public void Start()
    {        
        unitProgress = 0f;
        progress = 0f;
        targetAmount = pile.Amount;
    }

    public void Tick(float dt)
    {
        if (inventory.CalculateWeight(targetItem) >= stats.carryWeight)
        {
            Cancel();
            return;
        }
        
        unitProgress += dt * speed;
        progress += dt * speed / targetAmount;

        while (unitProgress >= 1f)
        {
            unitProgress -= 1;
            pile.Remove(targetItem, 1);
            inventory.Add(targetItem, 1);
        }
    }

    public void Cancel()
    {
        WasCanceled = true;
    }
}
