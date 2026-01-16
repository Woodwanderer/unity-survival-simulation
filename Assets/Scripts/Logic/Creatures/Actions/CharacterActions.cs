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
                world.ClearTileEntity(h.targetObj);
                render.RemoveObjectSprite(h.targetObj);
            }
            if (currentAction is CollectItem c && c.pile.Amount <= 0)
            {
                world.ClearTileEntity(c.pile);
                render.RemoveObjectSprite(c.pile);
            }

            if (actionQueue.Count > 0)
                SetAction(actionQueue.Dequeue());
            else
                SetAction(null);
        }
        if (currentAction == null)
        {
            ITask task = world.taskManager.TakeTask();
            if (task is BuildTask bt)
            {
                TryBuild(bt.stockpile);
            }
            if (task is HaulTask ht)
            {
                TryHaul(ht);
            }
        }
    }
    public void TryHaul(HaulTask ht)
    {

        IAction collect = new CollectItem(ht.source, ht.source.Item, stats);
        if (ht.source.TileCoords == protagonistData.mapCoords) 
            SetAction(collect);
        else
        {
            bool canMove = TryMoveToTile(ht.source.TileCoords);
            if (canMove)
            {
                actionQueue.Enqueue(collect);
                IAction moveToStockpile = new Movement(protagonistData, render, stats.Speed, ht.deliveryPath);
                actionQueue.Enqueue(moveToStockpile);
                IAction deliver = new Deliver(inventory, stats, ht.destination);
                actionQueue.Enqueue(deliver);
            }
            else
                EventBus.Log("I can't reach this destination.");
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
    public void TryHarvest(TileEntity target, ItemDefinition item)
    {
        IAction transfer = null;

        if (target is ResourcePile pile && pile != null) 
        {
            transfer = new CollectItem(pile, item, stats);
        }
        else if(target is WorldObject wo)
        {
            transfer = new HarvestAction(wo, item, stats.harvestSpeed, world, render);
        }
        else
        {
            return;
        }

        if (protagonistData.mapCoords == target.TileCoords)
        {
            SetAction(transfer);
        }
        else
        {
            bool canMove = TryMoveToTile(target.TileCoords);

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
        TileEntity ent = world.FindNearestItem(item, protagonistData.mapCoords);
        if (ent != null)
        {
            TryHarvest(ent, item);
        }
    }
}
