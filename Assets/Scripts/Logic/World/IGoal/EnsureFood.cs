public class EnsureFood : IGoal
{
    CharacterActions hero;
    ItemSlot order;
    bool finished = false;
    public int Priority => (int)GoalPriority.Survival;
    public bool IsValid => hero != null && hero.stats.Starvation;
    public bool IsFinished => finished;
    bool executingGoal;
    float waitingFor = 0;
    float logTimer = 0;
    public string Name => "EnsureFood";

    public void Start(CharacterActions hero)
    {
        this.hero = hero;
        executingGoal = false;
        ItemDefinition food = hero.world.itemsDatabase.Get("foodRaw");
        order = new(food, 5);
    }
    public void Tick(float dt)
    {
        if (!IsValid && !(hero.actionRunner.currentAction is EatAction)) 
        {
            finished = true;
            executingGoal = false;
            return;
        }

        if (!executingGoal && hero.actionRunner.currentAction != null)
        {
            float givenTime = 30f;
            waitingFor += dt;
            logTimer += dt;

            if (logTimer >= 5)
            {
                logTimer = 0;
                EventBus.Log($"[Goal] {Name} waiting {waitingFor:0.0}s / {givenTime:0.0}s");
            }

            if (waitingFor < givenTime)
                return;

            hero.actionRunner.currentAction.Cancel();
            hero.actionRunner.currentAction = null;
        }

        if (!executingGoal)
        {
            executingGoal = true;
            EventBus.Log($"New Goal: {this.Name} assumed control.");
        }

        //Wait if we're already doing smth form execution here
        if (hero.actionRunner.currentAction != null)
            return;


        if (hero.TryEat(order)) 
        {
            return;
        }

        bool found = hero.FindNearest(order);
        /*if (!found)
        {
            hero.stats.foodAvailable = false;
            finished = true;
        }*/
    }
    public void Cancel()
    {
        EventBus.Log($"Current Goal was canceled: {this.Name}.");
    }

  


}
