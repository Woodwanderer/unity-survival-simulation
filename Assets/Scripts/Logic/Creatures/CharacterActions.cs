using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using UnityEngine;

public class CharacterActions
{
    World world;
    ProtagonistData protagonistData;
    RenderWorld renderWorld;

    CharacterActionState state = CharacterActionState.Idle;
    public CharacterActionState State => state;
    PendingAction pendingAction = null;

    float nutritionValue = 0; // will be gone after food - ItemType improvement
    float hourDuration;
    float nutriRate;
    float nutrition = 0;

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
    public CharacterActions(float hourDuration, VirtualResources baseRes, World world, ProtagonistData protagonistData, RenderWorld renderWorld)
    {
        this.world = world;
        this.protagonistData = protagonistData;
        this.hourDuration = hourDuration;

        globalRes = baseRes;
        nutriRate = hourDuration * 0.16f; //full bar in 10 minutes // 10 sec in game 1/6th of an hour
        this.renderWorld = renderWorld;
    }

    public void Init()
    {
        EventBus.OnMovementAnimationComplete += SetIdle; // send by ProtagonistMovement
    }
    public void Tick(float dt)
    {
        //Check for pending action
        if (state == CharacterActionState.Idle && pendingAction != null)
            state = pendingAction.type;


        //Harvesting
        if (state == CharacterActionState.Harvesting)
        {
            //Harvesting(dt); rewrite later -> harvesting action and make class for actions to resolve it's progress here 3 vars (max, step, speed.. or smth like in eating, harvesting etc.)
            if (pendingAction.type == CharacterActionState.Harvesting) 
                Harvesting();
        }
    }
    void SetIdle() 
    {
        if(state == CharacterActionState.Moving) //after movement animation complete only so far
            state = CharacterActionState.Idle;
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
    public bool Harvest()
    {
        TileData currentTile = world.GetProtagonistTileData();
        if (currentTile.objects.Count == 0)
        {
            EventBus.Log("Nothing to gather here.");
            return false;
        }

        TileObject obj = currentTile.objects[0];

        foreach (var change in obj.Resources.DrainAll())
        {
            world.resources.Add(change);
            
        }

        //CLEAR - object fully depleted
        if (obj.Resources.isEmpty) 
        {
            EventBus.ObjectDepleted(currentTile.mapCoords);
            currentTile.objects.Clear();
        }

        return true;
    }
    public void RequestHarvest(TileObject tileObject, ItemType item)
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
    public void Harvesting()
    {
        TileObject obj = pendingAction.target;
        ItemType itemHarvested = pendingAction.itemType;

        if (obj.Resources.Has(itemHarvested)) 
        {
            int amount = obj.Resources.Get(itemHarvested);
            obj.Resources.Remove(itemHarvested, amount);
            world.resources.Add(itemHarvested, amount);
        }

        //CLEAR - object fully depleted
        if (obj.Resources.isEmpty) 
        {
            TileData currentTile = world.GetProtagonistTileData();
            EventBus.ObjectDepleted(currentTile.mapCoords);
            currentTile.objects.Clear();
        }

        pendingAction = null;
        state = CharacterActionState.Idle;
    }

    //MOVE
    public void MoveToTile(Vector2Int tileCoords)
    {
        if (state == CharacterActionState.Moving)
            return;

        if (EstablishRoute(tileCoords))
        {
            renderWorld.DrawPath(world.protagonistData.pathCoords, true);
            ProtagonistMove();
        }
    }

    //ROUTE
    public void CancelRoute()
    {
        protagonistData.pathCoords.Clear();
    }
    bool EstablishRoute(Vector2Int target)
    {
        if (protagonistData.mapCoords == target || world.GetTileData(target).isWalkable == false) 
            return false;

        protagonistData.pathCoords.Clear();
        protagonistData.pathCoords = world.pathfinder.FindPath(protagonistData.mapCoords, target);
        protagonistData.pathSteps.Clear();
        protagonistData.pathSteps = world.pathfinder.GetPathSteps(protagonistData.mapCoords, protagonistData.pathCoords);

        return true;
    }
    public void HandleConfirm()
    {
        // Establish ROUTE
        if (world.lastTileSelected != null)
        {
            MoveToTile(world.lastTileSelected.mapCoords);
        }
    }
    void ProtagonistMove()
    {
        state = CharacterActionState.Moving;
        world.CancelSelection();
        renderWorld.MoveProt();
    }
}
