using System.Collections.Generic;
public class CharacterBrain
{
    //External Data
    public World world;
    public ProtagonistData protagonistData;

    public CharacterSheet stats;
    public Inventory inventory = new(16);
    public ActionRunner actionRunner;

    //goals
    IGoal currentGoal;
    readonly List<IGoal> goals = new();
    public bool HasGoal<T>() where T : IGoal
    {
        if (currentGoal is T) 
            return true;
        foreach (var goal in goals) 
        {
            if (goal is T) 
                return true;
        }

        return false;
    }

    public bool IsWorking => actionRunner.currentAction is CollectItem || actionRunner.currentAction is HarvestAction || actionRunner.currentAction is BuildAction || actionRunner.currentAction is PickUp;
    public bool IsResting => actionRunner.currentAction is Rest;
    public CharacterBrain(World world, ProtagonistData protagonistData)
    {
        this.world = world;
        this.protagonistData = protagonistData;

        stats = new CharacterSheet(this);

        Init();
    }
    public void Init()
    {
        actionRunner = new(world);
        EventBus.OnTileCommanded += coords =>
        {
            ExecutePlayerCommand(new Movement(world, coords));
        };
    }
    public void Tick(float dt)
    {
        stats.Tick(dt);
        if (currentGoal != null && currentGoal.IsFinished)
            currentGoal = null;

        SelectNextGoal();

        currentGoal?.Tick(dt);
        actionRunner.Tick(dt);

        if (actionRunner.currentAction == null && currentGoal == null)
        {
            ITask task = world.taskManager.TakeTask();
            if (task is BuildTask bt)
            {
                actionRunner.ExecutePlan(new BuildPlan(this, bt.building));
            }
            if (task is HaulTask ht)
            {
                actionRunner.ExecutePlan(new HaulPlan(this, ht));
            }
        }
    }
    public void AddGoal(IGoal newGoal)
    {
        if (currentGoal == null)
        {
            currentGoal = newGoal;
            currentGoal.Start(this);
            EventBus.Log($"Added new Goal: {newGoal}");
            return;
        }
        if (currentGoal.Priority >= newGoal.Priority)
        {
            goals.Add(newGoal);
            EventBus.Log($"Added new Goal to queue: {newGoal}");
        }
        else
        {
            goals.Add(currentGoal);
            currentGoal = newGoal;
            currentGoal.Start(this);
            EventBus.Log($"Replaced current Goal with :{newGoal}");
        }
    }
    void SelectNextGoal()
    {
        if (currentGoal != null)
            return;
        if (goals.Count == 0)
            return;

        IGoal best = goals[0];
        for (int i = 1; i < goals.Count; i++)
        {
            if (goals[i].Priority > best.Priority)
                best = goals[i];
        }

        goals.Remove(best);
        currentGoal = best;
        currentGoal.Start(this);

    }
    //Player Command
    public void ExecutePlayerCommand(IAction action = null, IPlan plan = null)
    {
        if (currentGoal != null)
        {
            currentGoal.Cancel();
            currentGoal = null;
        }

        goals.Clear();

        actionRunner.ClearQueue();

        if (action != null)
            actionRunner.SetAction(action);
        else if (plan != null) 
            actionRunner.ExecutePlan(plan);
    } 
    public IPlan CreatAcqusitionPlan(ItemSlot order)
    {
        Stockpile from = world.taskManager.FindClosestStockpileWith(order, protagonistData.mapCoords);
        if (from != null)
        {
            return new PickUpPlan(this, order, from);
        }
        TileEntity ent = world.FindNearest(order, protagonistData.mapCoords);
        if (ent != null)
        {
            return new HarvestPlan(this, ent, order);
        }

        return null;
    }
}
