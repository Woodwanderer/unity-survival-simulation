public class EatAction : IAction
{
    //External Data
    CharacterActions brain;
    ItemSlot meal;              //Eating that
    //Deriveratives
    Inventory inventory;        //Eating from
    float speed;

    //Generic IAction
    public ActionToken Token { get; set; }
    public ActionStatus Status { get; private set; } = ActionStatus.NotStarted;
    public bool IsFinished => Status == ActionStatus.Succeeded || Status == ActionStatus.Cancelled || Status == ActionStatus.Failed;

    float nutritionValue;
    public float nutrition;     //-hunger: used by CharacterSheet

    public float progress = 0f;
    float unitProgress = 0f;

    public EatAction(CharacterActions brain, ItemSlot meal = null)
    {
        this.brain = brain;
        this.meal = meal;

        this.inventory = brain.inventory;
        this.speed = brain.stats.eatSpeed;
    }
    public void Start()
    {
        if (meal == null)
        {
            ItemDefinition food = brain.world.itemsDatabase.Get("foodRaw");
            meal = new ItemSlot(food, 5);
        }

        if (!inventory.Snapshot().Has(meal.Item, meal.Amount))
        {
            Status = ActionStatus.Failed;
            return;
        }   

        nutritionValue = 0.25f; //percent of full HUNGER bar -> how much of a bar it will fill
        
        Status = ActionStatus.Running;
    }
    public void Tick(float dt)
    {
        unitProgress += dt * speed;
        progress += dt * speed / meal.Amount;

        while (unitProgress >= 1f)
        {
            unitProgress -= 1f;
            inventory.Remove(meal.Item, 1);
        }

        nutrition = dt * speed / meal.Amount * nutritionValue;

        if (progress >= 1f)
            Status = ActionStatus.Succeeded;
    }
    public void Cancel()
    {
        Status = ActionStatus.Cancelled;
    }
}
