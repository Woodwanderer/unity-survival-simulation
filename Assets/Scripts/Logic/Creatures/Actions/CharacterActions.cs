using System.Collections.Generic;
using UnityEngine;
public class CharacterActions
{
    public World world;
    public ProtagonistData protagonistData;
    RenderWorld render;
    public CharacterSheet stats;
    public ActionRunner actionRunner;
    public Inventory inventory = new(16);

    //goals
    public IGoal currentGoal;
    public List<IGoal> goals = new();
    void SetGoal(IGoal newGoal)
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
            currentGoal.Cancel();
            EventBus.Log($"Replacing {currentGoal} with :{newGoal}");
            currentGoal = newGoal;
            currentGoal.Start(this);
        }
    }

    public bool IsWorking => actionRunner.currentAction is CollectItem || actionRunner.currentAction is HarvestAction || actionRunner.currentAction is BuildAction || actionRunner.currentAction is PickUp;
    public bool IsResting => actionRunner.currentAction is Rest;
    public CharacterActions(World world, ProtagonistData protagonistData, RenderWorld render)
    {
        this.world = world;
        this.protagonistData = protagonistData;
        this.render = render;
        stats = new CharacterSheet(this);

        Init();
    }
    public void Init()
    {
        actionRunner = new(world, render);
        EventBus.OnTileCommanded += MoveToTile;
        stats.OnStarvationStart += HandleStarvation;
    }
    public void Tick(float dt)
    {
        stats.Tick(dt);
        if (currentGoal != null && currentGoal.IsFinished)
            currentGoal = null;
        else if (currentGoal == null && goals.Count > 0) 
        {
            currentGoal = goals[0];
            for (int i = 1; i < goals.Count; i++) 
            {
                if (currentGoal.Priority < goals[i].Priority)
                    currentGoal = goals[i];
            }
            goals.Remove(currentGoal);
            currentGoal.Start(this);
        }

        currentGoal?.Tick(dt);
        actionRunner.Tick(dt);

        if (actionRunner.currentAction == null && currentGoal == null)
        {
            ITask task = world.taskManager.TakeTask();
            if (task is BuildTask bt)
            {
                TryBuild(bt.building);
            }
            if (task is HaulTask ht)
            {
                TryHaul(ht);
            }
        }
    }
    public void TryHaul(HaulTask ht)
    {
        IAction collect = new CollectItem(ht.source, ht.source.Slot, stats);
        actionRunner.SetAction(new Movement(world, ht.source.TileCoords));
        actionRunner.actionQueue.Enqueue(collect);

        IAction moveToStockpile = new Movement(world, ht.deliveryPath);
        actionRunner.actionQueue.Enqueue(moveToStockpile);
        IAction deliver = new Deliver(inventory, stats, ht.destination);
        actionRunner.actionQueue.Enqueue(deliver);
    }

    //Build
    public void TryBuild(Building building)
    {
        IAction build = new BuildAction(building, stats, world);

        if (building.Area.IsInRange(protagonistData.mapCoords)) 
            actionRunner.SetAction(build);
        else
        {
            actionRunner.SetAction(new Movement(world, building.Area));
            actionRunner.actionQueue.Enqueue(build);
        }
    }
    
    //EAT
    void HandleStarvation()
    {
        SetGoal(new EnsureFood());
    }
    public bool TryEat(ItemSlot meal = null)
    {
        if (meal == null)
        {
            ItemDefinition food = world.itemsDatabase.Get("foodRaw");
            meal = new ItemSlot(food, 5);
        }

        if (!inventory.Snapshot().Has(meal.Item, meal.Amount))
        {
            EventBus.Log("You don't have enough food.");
            return false;
        }

        IAction eat = new EatAction(inventory, meal.Item, stats);
        actionRunner.SetAction(eat);
        return true;
    }
    //HARVEST
    public void TryHarvest(TileEntity target, ItemSlot order)
    {
        IAction transfer = null;

        if (target is ResourcePile pile && pile != null) 
        {
            transfer = new CollectItem(pile, order, stats);
        }
        else if(target is WorldObject wo)
        {
            transfer = new HarvestAction(wo, order, stats.harvestSpeed, world);
        }
        else
        {
            return;
        }
        actionRunner.SetAction(new Movement(world,target.TileCoords));
        actionRunner.actionQueue.Enqueue(transfer);
    }
    public void TryPickUp(ItemSlot order, Stockpile from)
    {
        actionRunner.SetAction(new Movement(world, from.area.center));
        actionRunner.actionQueue.Enqueue(new PickUp(from, order, stats));
    }
    //MOVE
    public void MoveToTile(Vector2Int tileCoords)
    {
        actionRunner.SetAction(new Movement(world, tileCoords));
    }
    public void MoveToArea(Area area)
    {
        actionRunner.SetAction(new Movement(world, area));
    }
    public bool FindNearest(ItemSlot order)
    {
        Stockpile from = null;
        from = world.taskManager.FindClosestStockpileWith(order, protagonistData.mapCoords);

        if (from != null)
        {
            TryPickUp(order, from);
            return true;
        }
        TileEntity ent = world.FindNearest(order, protagonistData.mapCoords);
        if (ent != null)
        {
            TryHarvest(ent, order);
            return true;
        }
        return false;
    }
}
