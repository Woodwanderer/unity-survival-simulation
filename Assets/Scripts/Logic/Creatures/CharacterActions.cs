using UnityEngine;
using System.Collections.Generic;


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
    float harvestSpeed;
    float harvested = 0;

    VirtualResources globalRes;

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

        //Check for pending action
        if (state == CharacterActionState.Idle && pendingAction != null)
            state = pendingAction.type;


        //Harvesting
        if (state == CharacterActionState.Harvesting)
        {
            //Harvesting(dt); rewrite later -> harvesting action and make class for actions to resolve it's progress here 3 vars (max, step, speed.. or smth like in eating, harvesting etc.)
            if (pendingAction.type == CharacterActionState.Harvesting)
                Harvesting(dt);
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
        if (!globalRes.Remove(food, ration))
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
    public void HarvestAttempt(TileObject tileObject, ItemType item)
    {
        pendingAction = new PendingAction(CharacterActionState.Harvesting, tileObject, item);

        if (tileObject.tileCoords == protagonistData.mapCoords && state == CharacterActionState.Idle)
        {
            state = pendingAction.type;
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
            


            harvested += deltaTime * harvestSpeed;

            while (harvested >= 1)
            {
                harvested -= 1;
                obj.Resources.Remove(itemHarvested, 1);
                world.resources.Add(itemHarvested, 1);
            }
        }
        else
        {
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
