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
    ContextActionBarUI contextActionBarUI;
    
    MapStates mapState = MapStates.None;
    ProtagonistStates protagonistState = ProtagonistStates.None;
    bool routeEstablished = false;

    TileObjectView lastObjectSelected;

    public GameState(World world, RenderWorld render, CameraMovement cam, InputController input_in, InventoryUI inv, ContextActionBarUI contextActionBarUI)
    {
        this.world = world;
        this.renderWorld = render;
        this.cam = cam;
        this.inputContr = input_in;
        this.inventory = inv;
        this.contextActionBarUI = contextActionBarUI;
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
    void HandleObjectClick(TileObjectView objectView)
    {
        if (lastObjectSelected == objectView) //deselect
        {
            CancelObjSelection();
            contextActionBarUI.ClearButtons();
        }
        else
        {
            CancelObjSelection();
            contextActionBarUI.ClearButtons();

            objectView.SetSelected();
            lastObjectSelected = objectView;
            EventBus.Log($"Object clicked: {objectView.Data}");
            SetContextActionBarUI();
        }
    }
    void CancelObjSelection()
    {
        if (lastObjectSelected != null)
        {
            lastObjectSelected.SetSelected();
            lastObjectSelected = null;
        }
    }
    void HandleCancel()
    {
        // SEQUENCE matters

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

        CancelObjSelection();
        contextActionBarUI.ClearButtons();
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
    public void AttemptHarvest() // function is added to button only
    {
        if (protagonistState == ProtagonistStates.None)
        {
            if (world.Harvest())
            {
                inventory.AddToInv(world.resources.wasAdded); //Update InventoryUI
                world.resources.ClearEntry();
            }
        }
    } 
    void SetContextActionBarUI() // for HandleObjectClick
    {
        contextActionBarUI.GetActionSource(lastObjectSelected.Data);
        
    }
    public void AttemptHarvestObject(TileObject tileObject)
    {

    }


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
