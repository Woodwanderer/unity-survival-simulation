using System.Linq;
public class Deliver : IAction
{
    //External Data
    Inventory inventory;
    Stockpile destination;
    //Driveratives
    float speed;

    //Generic iAction
    public ActionToken Token {  get; set; }
    public ActionStatus Status { get; private set; } = ActionStatus.NotStarted;
    public bool IsFinished => Status == ActionStatus.Succeeded || Status == ActionStatus.Cancelled;

    public float progress;
    float unitProgress;

    int targetAmount;
    
    public Deliver(Inventory inventory, CharacterSheet stats, Stockpile destination)
    {
        this.inventory = inventory;
        this.speed = stats.harvestSpeed;
        this.destination = destination;
    }
    
    public void Start()
    {
        targetAmount = 0;
        foreach (var slot in inventory.Slots)
        {
            if (!slot.IsEmpty) 
                targetAmount += slot.Amount;
        }
        if (targetAmount <= 0) 
        {
            Status = ActionStatus.Succeeded;
            return;
        }
        Status = ActionStatus.Running;
    }
    public void Tick(float dt)
    {
        if (inventory.IsEmpty)
        {
            Status = ActionStatus.Succeeded;
            return;
        }

        unitProgress += dt * speed;
        progress += dt * speed / targetAmount;

        while (unitProgress >= 1f)
        {
            unitProgress -= 1;
            ItemSlot slot = inventory.Slots.FirstOrDefault(s => !s.IsEmpty && s.Item != null);

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
