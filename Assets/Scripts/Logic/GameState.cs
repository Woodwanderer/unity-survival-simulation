using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class GameState
{
    InputController inputContr;
    World world;
    RenderWorld renderWorld;
    CameraMovement cam;
    //UI
    InventoryUI inventory;
    
    MapStates mapState = MapStates.None;
    ProtagonistStates protagonistState = ProtagonistStates.None;
    bool routeEstablished = false;

    public GameState(World world, RenderWorld render, CameraMovement cam, InputController input_in, InventoryUI inv)
    {
        this.world = world;
        this.renderWorld = render;
        this.cam = cam;
        this.inputContr = input_in;
        this.inventory = inv;
    }

    public void Initialise()
    {
        EventBus.OnTileClicked += HandleTileClicked; // send by tile
        EventBus.OnMovementAnimationComplete += RestoreStates; // send by ProtagonistMovement
        EventBus.OnObjectClick += HandleObjectClick;
    }
    public void Tick(float deltaTime)
    {
        //CONFIRM
        if (inputContr.ConsumeConfirm())
            HandleConfirm();
        //CANCEL
        if(inputContr.ConsumeCancel()) 
            HandleCancel();


        //Check for base resources changes
        if(world.resources.removed == true)
        {
            inventory.RemoveEntry(world.resources.wasRemoved);
            world.resources.ClearEntry();
        }
        //Check for Protagonist ActionState to trigger apropriate animation
        
        bool isEating = world.protagonistData.charSheet.actions.State == CharacterActionState.Eating;
        renderWorld.animator.SetAnimation(isEating);
        

    }
    void HandleTileClicked(TileData tile)
    {
        if (world.TrySelectTile(tile))
        {
            mapState = MapStates.TileSelected;
            if(tile.objects.Count != 0)
                EventBus.Log("Objects here: " + tile.objects[0].type.ToString());
            
        }
    }
    void HandleObjectClick(TileObject objectData)
    {
        EventBus.Log($"Object clicked: {objectData.type}");
    }

    void HandleCancel()
    {
        //Clear CAMERA Follow
        if (cam.cameraFollow)
        {
            cam.StopFollow();
            return;
        }

        // Clear Highlight on Tile
        if (mapState == MapStates.TileSelected)
        {
            if (world.CancelSelection())
            {
                mapState = MapStates.None;
                return;
            }
        }
        // Clear ROUTE
        if (routeEstablished && protagonistState != ProtagonistStates.Moving)
        {
            renderWorld.DrawPath(world.protagonistData.pathCoords, false);
            world.CancelRoute();
            routeEstablished = false;
            mapState = MapStates.TileSelected;
            return;
        }
    }
    void HandleConfirm()
    {
        // Establish ROUTE
        if (protagonistState == ProtagonistStates.None && mapState == MapStates.TileSelected)
        {
            if (!routeEstablished)
            {
                if (world.EstablishRoute())
                {
                    renderWorld.DrawPath(world.protagonistData.pathCoords, true);
                    routeEstablished = true;
                }
                return;
            }
        }
        // Protagonist MOVEMENT
        if (protagonistState == ProtagonistStates.None && routeEstablished)
        {
            protagonistState = ProtagonistStates.Moving;
            world.CancelSelection();
            mapState = MapStates.None;
            renderWorld.MoveProt();
            return;
        }
    }

    //ACTION BAR
    public void AttemptHarvest()
    {
        if (protagonistState == ProtagonistStates.None)
        {
            if (world.Harvest())
            {
                inventory.AddToInv(world.resources.wasAdded); //Update InventoryUI
                world.resources.ClearEntry();
            }
        }
    } // function is added to button only



    //STATES
    private enum MapStates
    {
        TileSelected,
        None,
    }
    private enum ProtagonistStates
    {
        Moving,
        None,
    }
    private void SetProtagonistIdleState()
    {
        protagonistState = ProtagonistStates.None;
    }
    private void RestoreStates()
    {
        SetProtagonistIdleState();
        routeEstablished = false;
    }
}
