using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class GameState
{
    InputController inputContr;
    World world;
    RenderWorld renderWorld;
    CameraMovement cam;
    //UI
    InventoryUI inventoryUI;
    ContextActionBarUI contextActionBarUI;
    
    CharacterActionState protagonistState;

    TileObjectView lastObjectSelected;

    public GameState(World world, RenderWorld render, CameraMovement cam, InputController input_in, InventoryUI inventoryUI, ContextActionBarUI contextActionBarUI)
    {
        this.world = world;
        this.renderWorld = render;
        this.cam = cam;
        this.inputContr = input_in;
        this.inventoryUI = inventoryUI;
        this.contextActionBarUI = contextActionBarUI;
    }

    public void Initialise()
    {
        inventoryUI.Init(world.resources);
        EventBus.OnTileClicked += HandleTileClicked; // send by tile
        EventBus.OnObjectClick += HandleObjectClick;
    }
    public void Tick(float deltaTime)
    {
        //CONFIRM
        if (inputContr.ConsumeConfirm())
            world.protagonistData.charSheet.actions.HandleConfirm();
        //CANCEL
        if(inputContr.ConsumeCancel()) 
            HandleCancel();


        //Check for Protagonist ActionState to trigger apropriate animation

        bool isEating = world.protagonistData.charSheet.actions.State == CharacterActionState.Eating;
        renderWorld.animator.SetAnimation(isEating);

    }
    void HandleTileClicked(TileData tile)
    {
        world.TrySelectTile(tile);
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
        world.CancelSelection();
            
        CancelObjSelection();
        contextActionBarUI.ClearButtons();
    }
  


    //ACTION BAR
    public void AttemptHarvest() // function is added to button only
    {
        if (protagonistState == CharacterActionState.Idle)
        {
            world.protagonistData.charSheet.actions.Harvest();
        }
    } 
    void SetContextActionBarUI() // for HandleObjectClick
    {
        contextActionBarUI.GetActionSource(lastObjectSelected.Data);
        
    }

}
