using UnityEngine;
using System.Collections.Generic;

public class CharacterActions
{
    World world;
    ProtagonistData protagonistData;
    RenderWorld renderWorld;
    public CharacterSheet stats;

    Queue<IAction> actionQueue = new Queue<IAction>();
    public IAction currentAction;
    void SetAction(IAction newAction)
    {
        currentAction?.Cancel(); //cancel previous
        currentAction = newAction;
        currentAction.Start();
    }
    public CharacterActions(float hourDuration, World world, ProtagonistData protagonistData, RenderWorld renderWorld)
    {
        this.world = world;
        this.protagonistData = protagonistData;
        this.renderWorld = renderWorld;
        stats = new CharacterSheet(hourDuration, this);

        Init();
    }

    public void Init()
    {
        EventBus.OnTileCommanded += MoveToTile;
    }
    public void Tick(float dt)
    {
        currentAction?.Tick(dt);

        if (currentAction != null && currentAction.IsFinished)
        {
            if (currentAction is HarvestAction h && h.targetObj.Resources.Depleted)
            {
                world.ClearTileObject(h.targetObj);
                renderWorld.RemoveObjectSprite(h.targetObj);
            }

            if (actionQueue.Count > 0)
                SetAction(actionQueue.Dequeue());
            else
                currentAction = null;
        }

    }

    //EAT
    public bool TryEat()
    {
        int ration = 5;

        ItemDefinition foodRaw = world.itemsDatabase.Get("foodRaw");

        if (!world.resources.Has(foodRaw, ration))
        {
            EventBus.Log("You don't have enough food.");
            return false;
        }

        IAction eat = new EatAction(world.resources, foodRaw, stats);
        SetAction(eat);

        return true;
    }

    //HARVEST
    public void TryHarvest(TileObject target, ItemDefinition item)
    {
        IAction harvest = new HarvestAction(target, item, stats.harvestSpeed, world.resources);
        if (protagonistData.mapCoords == target.tileCoords)
            SetAction(harvest);
        else
        {
            bool canMove = TryMoveToTile(target.tileCoords);

            if (canMove)
                actionQueue.Enqueue(harvest);
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

        world.CancelSelection();

        SetAction(new Movement(protagonistData, renderWorld, stats.speed, newPath));
        return true;
    }
    public void HandleConfirm()
    {
        // Move
        if (world.lastTileSelected != null)
        {
            MoveToTile(world.lastTileSelected.mapCoords);
        }
    }
}
