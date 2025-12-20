using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class GameState
{
    InputController inputContr;
    World world;
    RenderWorld render;
    CameraMovement cam;
    InventoryUI inventory;
    
    MapStates mapState = MapStates.None;
    ProtagonistStates protagonistState = ProtagonistStates.None;
    bool routeEstablished = false;

    public GameState(World world, RenderWorld render, CameraMovement cam, InputController input_in, InventoryUI inv)
    {
        this.world = world;
        this.render = render;
        this.cam = cam;
        this.inputContr = input_in;
        this.inventory = inv;
    }

    public void Initialise()
    {
        EventBus.OnTileClicked += HandleTileClicked; // send by tile
        EventBus.OnMovementAnimationComplete += RestoreStates; // send by ProtagonistMovement
    }
    public void Tick(float deltaTime)
    {
        //CONFIRM
        if (inputContr.ConsumeConfirm())
            HandleConfirm();
        //CANCEL
        if(inputContr.ConsumeCancel()) 
            HandleCancel();
    }
    void HandleTileClicked(TileData tile)
    {
        if (world.TrySelectTile(tile))
        {
            mapState = MapStates.TileSelected;
            EventBus.Log("Objects here: " + tile.objects[0].type.ToString());
            
        }
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
            render.DrawPath(world.protagonistData.route.pathCoords, false);
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
                    render.DrawPath(world.protagonistData.route.pathCoords, true);
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
            render.MoveProt();
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
