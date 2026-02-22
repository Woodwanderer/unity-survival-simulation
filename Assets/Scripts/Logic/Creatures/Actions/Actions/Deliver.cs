using System.Linq;
public class Deliver : IAction
{
    //External Data
    Inventory inventory;
    Stockpile destination;
    ItemSlot order;
    //Driveratives
    float speed;

    //Generic iAction
    public ActionToken Token {  get; set; }
    public ActionStatus Status { get; private set; } = ActionStatus.NotStarted;
    public bool IsFinished => Status == ActionStatus.Succeeded || Status == ActionStatus.Cancelled;

    int carriedAmount;
    public float progress;
    float unitProgress;
    
    public Deliver(Inventory inventory, CharacterSheet stats, Stockpile destination, ItemSlot order)
    {
        this.inventory = inventory;
        this.speed = stats.harvestSpeed;
        this.destination = destination;
        this.order = order;
    }
    
    public void Start()
    {
        carriedAmount = inventory.Slots.Where(s => s.Item == order.Item).Sum(s => s.Amount);
        
        Status = ActionStatus.Running;
    }
    public void Tick(float dt)
    {
        if (carriedAmount == 0) 
        {
            Status = ActionStatus.Succeeded;
            return;
        }

        unitProgress += dt * speed;
        progress += dt * speed / carriedAmount;

        while (unitProgress >= 1f)
        {
            unitProgress -= 1;
            ItemSlot slot = inventory.Slots.FirstOrDefault(s => !s.IsEmpty && s.Item == order.Item);

            if (slot == null) 
            {
                Status = ActionStatus.Succeeded;
                return;
            }

            int overflow = destination.Add(slot.Item, 1);
            if (overflow == 0)
                slot.Remove(1);
            else
            {
                //Destination is Full
                Status = ActionStatus.Cancelled;
                return;
            }
        }
        if (progress >= 1f)
        {
            Status = ActionStatus.Succeeded;
        }
    }
    public void Cancel()
    {
        Status = ActionStatus.Cancelled;
    }
}
