using UnityEngine;
using System.Collections.Generic;

public class CharacterActions
{
    World world;
    ProtagonistData protagonistData;
    RenderWorld renderWorld;

    CharacterActionState state = CharacterActionState.Idle;
    public CharacterActionState State => state;

    float nutritionValue = 0; // will be gone after food - ItemType improvement
    float hourDuration;
    float nutriRate;
    float nutrition = 0;

    float harvestSpeed; //Set in constructor

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
        this.hourDuration = hourDuration;

        nutriRate = hourDuration * 0.16f; //full bar in 10 minutes // 10 sec in game 1/6th of an hour
        harvestSpeed = 100 / hourDuration;

        this.renderWorld = renderWorld;
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

        //Eating
        if (state == CharacterActionState.Eating)
            renderWorld.animator.SetEatingAnimation(true);
    }

    //EAT
    public bool BlocksHunger
    {
        get
        {
            return state == CharacterActionState.Eating;
        }
    }
    public void EatInit(ItemType food)
    {
        nutritionValue = 0.25f; // get from food -> improve ItemType

        int ration = 5;
        if (!world.resources.Has(food, ration))
        {
            EventBus.Log("You don't have enough food.");
        }
        else
        {
            state = CharacterActionState.Eating;
        }

    }
    public float Eating(float deltaTime)
    {
        if (nutrition >= nutritionValue)
        {
            state = CharacterActionState.Idle;
            nutritionValue = 0;
            nutrition = 0;
        }
        nutrition += deltaTime / nutriRate;
        return deltaTime / nutriRate;
    }

    //HARVEST
    public void TryHarvest(TileObject target, ItemType item)
    {
        IAction harvest = new HarvestAction(target, item, harvestSpeed, world.resources);
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
        
        SetAction(new Movement(protagonistData, renderWorld, newPath));
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
