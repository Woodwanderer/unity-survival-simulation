using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting;


public class CharacterActions
{
    World world;
    ProtagonistData protagonistData;
    RenderWorld renderWorld;


    CharacterActionState state = CharacterActionState.Idle;
    public CharacterActionState State => state;
    PendingAction pendingAction = null;

    Movement movement;

    float nutritionValue = 0; // will be gone after food - ItemType improvement
    float hourDuration;
    float nutriRate;
    float nutrition = 0;

    GameObject ActionProgMiniBarUI;
    float harvestSpeed; //Set in constructor
    float harvested = 0;
    public float harvestedOfWhole = 0;
    int wholeHarvestSize = 0;

    int actionIterationUnitCount = 0;

    public IAction currentAction;
    void SetAction(IAction newAction)
    {
        currentAction = newAction;
        currentAction.Start();
        currentAction?.Cancel();
    }


    class PendingAction
    {
        public CharacterActionState type;
        public TileObject target;
        public ItemType itemType;
        public PendingAction(CharacterActionState type, TileObject target, ItemType itemType)
        {
            this.type = type;
            this.target = target;
            this.itemType = itemType;
        }
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
        movement = new Movement(protagonistData, renderWorld);
        EventBus.OnTileCommanded += MoveToTile;
    }
    public void Tick(float dt)
    {
        movement.Tick(dt);

        currentAction?.Tick(dt);

        if(currentAction != null && currentAction.IsFinished) 
            currentAction = null;

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
        if (protagonistData.mapCoords == target.tileCoords)
            SetAction(new HarvestAction(target, item, harvestSpeed, world.resources));
        else
        {
            //MoveAction -> to write
        }
    }
    public void HarvestAttempt(TileObject tileObject, ItemType item)
    {
        pendingAction = new PendingAction(CharacterActionState.Harvesting, tileObject, item);

        if (tileObject.tileCoords == protagonistData.mapCoords)
        {
            wholeHarvestSize = tileObject.Resources.Get(item);
            if (wholeHarvestSize <= 0)
            {
                pendingAction = null;
                return;
            }
            harvestedOfWhole = 0;
            state = CharacterActionState.Harvesting;
        }
        else
        {
            MoveToTile(tileObject.tileCoords);
        }
    }

    public void Harvesting(float deltaTime)
    {
        ItemType itemHarvested = pendingAction.itemType;
        TileObject obj = pendingAction.target;

        if (obj.Resources.Has(itemHarvested))
        {
            //Harvest by units
            harvested += deltaTime * harvestSpeed;

            while (harvested >= 1)
            {
                harvested -= 1;
                obj.Resources.Remove(itemHarvested, 1);
                world.resources.Add(itemHarvested, 1);
                actionIterationUnitCount++;
            }

            //Get progress into Harvesting of full obj
            float totalHarvested = harvested + actionIterationUnitCount;
            float progress = totalHarvested / wholeHarvestSize;
            harvestedOfWhole = Mathf.Clamp01(progress);

        }
        else
        {
            harvestedOfWhole = 0;
            pendingAction = null;
            state = CharacterActionState.Idle;

            if (obj.Resources.Depleted)
            {
                TileData currentTile = world.GetProtagonistTileData();
                EventBus.ObjectDepleted(obj);
                currentTile.RemoveObject(obj);
            }
        }
    }
    //MOVE
    public void MoveToTile(Vector2Int tileCoords)
    {
        if (protagonistData.mapCoords == tileCoords || world.GetTileData(tileCoords).isWalkable == false)
            return;

        List<Vector2Int> newPath = world.pathfinder.FindPath(protagonistData.mapCoords, tileCoords);

        world.CancelSelection();
        movement.SetPath(newPath);

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
