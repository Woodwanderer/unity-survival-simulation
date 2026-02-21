using UnityEngine;

public class CollectItem : IAction
{
    //External Data
    CharacterSheet stats;
    ItemSlot order;
    public ResourcePile pile;

    //Deriveratives
    Inventory inventory = null;


    //Generic IAction
    public ActionToken Token {  get; set; }
    public ActionStatus Status { get; private set; } = ActionStatus.NotStarted;
    public bool IsFinished => Status == ActionStatus.Succeeded || Status == ActionStatus.Cancelled || Status == ActionStatus.Failed;

    public float progress = 0f;
    public float unitProgress = 0f;
    int targetAmount;
    float speed;
    
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
        targetAmount = order.Amount; //UI call gives order.Amount = pile amount, order call gives order amount

        unitProgress = 0f;
        progress = 0f;
        Status = ActionStatus.Running;
    }

    public void Tick(float dt)
    {
        if (progress >= 1f)
        {
            Status = ActionStatus.Succeeded;
            return;
        }
        if (pile.Amount <= 0)
        {
            Status = ActionStatus.Succeeded;
            return;
        }
        if (inventory.CalculateWeight(order.Item) >= stats.carryWeight)
        {
            Status = ActionStatus.Succeeded;
            return;
        }

        unitProgress += dt * speed;
        progress += dt * speed / targetAmount;

        while (unitProgress >= 1f)
        {
            unitProgress -= 1;
            pile.Remove();
            inventory.Add(order.Item, 1);
        }
    }

    public void Cancel()
    {
        Status = ActionStatus.Cancelled;
    }
}
