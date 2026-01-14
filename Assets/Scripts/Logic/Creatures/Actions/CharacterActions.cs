using System.Collections.Generic;
using UnityEngine;
public class CharacterActions
{
    World world;
    ProtagonistData protagonistData;
    RenderWorld render;
    public CharacterSheet stats;
    public Inventory inventory = new(16);

    public Queue<IAction> actionQueue = new Queue<IAction>();
    public IAction currentAction;
    void SetAction(IAction newAction)
    {
        currentAction?.Cancel();
        currentAction = newAction;
        currentAction?.Start();
    }
    public CharacterActions(float hourDuration, World world, ProtagonistData protagonistData, RenderWorld render)
    {
        this.world = world;
        this.protagonistData = protagonistData;
        this.render = render;
        stats = new CharacterSheet(hourDuration, this);

        Init();
    }
    public void Init()
    {
        EventBus.OnTileCommanded += MoveToTile;
    }
    public void Tick(float dt)
    {
        stats.Tick(dt);
        currentAction?.Tick(dt);

        if (stats.Starvation) 
            EnsureFood();

        if (currentAction != null && currentAction.IsFinished)
        {
            if (currentAction is HarvestAction h && h.targetObj.harvestSource.Depleted)
            {
                world.ClearTileObject(h.targetObj);
                render.RemoveObjectSprite(h.targetObj);
            }
            if (currentAction is CollectItem c && c.targetObj.itemSlot.Amount <= 0)
            {
                world.ClearTileObject(c.targetObj);
                render.RemoveObjectSprite(c.targetObj);
            }

            if (actionQueue.Count > 0)
                SetAction(actionQueue.Dequeue());
            else
                SetAction(null);
        }
        if (currentAction == null)
        {
            ITask task = world.taskManager.TakeTask();
            if (task is BuildTask t)
            {
                TryBuild(t.stockpile);
            }
        }

    }
    //Build
    public void TryBuild(Stockpile stockpile)
    {
        IAction build = new BuildAction(stockpile, stats, render);
        if (stockpile.area.Contains(world.GetProtagonistCoords()))
            SetAction(build);
        else
        {
            bool canMove = TryMoveToTile(stockpile.area.center);

            if (canMove)
                actionQueue.Enqueue(build);
            else
                EventBus.Log("I can't reach this destination.");
        }
    }
    //EAT
    void EnsureFood()
    {
        if (currentAction != null && !(currentAction is BuildAction)) 
            return;
        if (TryEat())
            return;

        ItemDefinition foodRaw = world.itemsDatabase.Get("foodRaw");
        FindNearestRes(foodRaw);
    }
    public bool TryEat()
    {
        int ration = 5;

        ItemDefinition foodRaw = world.itemsDatabase.Get("foodRaw");

        if (!inventory.Snapshot().Has(foodRaw, ration))
        {
            EventBus.Log("You don't have enough food.");
            return false;
        }

        IAction eat = new EatAction(inventory, foodRaw, stats);
        SetAction(eat);
        return true;
    }

    //HARVEST
    public void TryHarvest(TileObject target, ItemDefinition item)
    {
        IAction transfer;

        if (target.type == TileObjectsType.ResourcePile)
        {
            transfer = new CollectItem(target, item, stats);
        }
        else
        {
            transfer = new HarvestAction(target, item, stats.harvestSpeed, world, render);
        }
        if (protagonistData.mapCoords == target.tileCoords)
        {
            SetAction(transfer);
        }
        else
        {
            bool canMove = TryMoveToTile(target.tileCoords);

            if (canMove)
                actionQueue.Enqueue(transfer);
            else
                EventBus.Log("I can't reach this destination.");
        }
    }
    //MOVE
    public void MoveToTile(Vector2Int tileCoords)
        => TryMoveToTile(tileCoords);
    public bool TryMoveToTile(Vector2Int tileCoords)
    {
        if (protagonistData.mapCoords == tileCoords || world.GetTileData(tileCoords).isWalkable == false)
            return false;

        List<Vector2Int> newPath = world.pathfinder.FindPath(protagonistData.mapCoords, tileCoords);
        if (newPath == null || newPath.Count == 0)
            return false;

        SetAction(new Movement(protagonistData, render, stats.Speed, newPath));
        return true;
    }
    void FindNearestRes(ItemDefinition item)
    {
        TileObject obj = world.FindNearestItem(item, protagonistData.mapCoords);
        if (obj != null)
        {
            TryHarvest(obj, item);
        }
    }
}
