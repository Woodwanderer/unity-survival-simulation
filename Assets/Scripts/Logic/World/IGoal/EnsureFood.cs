public class EnsureFood : IGoal
{
    CharacterActions hero;
    ItemSlot order;

    public int Priority => (int)GoalPriority.Survival;
    public bool IsValid => hero.stats.Starvation;
    bool finished = false;
    public bool IsFinished => finished;

    ActionToken? eatToken;
    bool executingGoal;

    float waitingFor = 0;
    float logTimer = 0;

    string Name = "EnsureFood";

    public void Start(CharacterActions hero)
    {
        this.hero = hero;
        executingGoal = false;
        ItemDefinition food = hero.world.itemsDatabase.Get("foodRaw");
        order = new(food, 5);
    }
    public void Tick(float dt)
    {
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

        if (eatToken.HasValue) 
        {
            if (hero.actionRunner.HasFinished(eatToken.Value, out var status))
            {
                eatToken = null;

                if (status == ActionStatus.Failed)
                {
                    bool found = hero.FindAndGetNearest(order);

                    if (!found)
                    {
                        hero.stats.foodAvailable = false;
                        finished = true;
                    }
                }
                else
                {
                    executingGoal = false;
                    finished = true;
                }
            }
        }
        else
        {
            eatToken = hero.actionRunner.SetAction(new EatAction(hero, order));
        }
    }
    public void Cancel()
    {
        finished = true;
        EventBus.Log($"Current Goal was canceled: {this.Name}.");
    }
}
