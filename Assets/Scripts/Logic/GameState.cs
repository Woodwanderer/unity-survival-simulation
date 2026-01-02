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

    TileObjectView currentObj;

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

        

    }
    void HandleTileClicked(TileData tile)
    {
        world.TrySelectTile(tile);
    }
    void HandleObjectClick(TileObjectView obj)
    {
        if (currentObj == obj)
        {
            DeselectCurrentObj();
            return;
        }

        SelectObj(obj);
    }
    void SelectObj(TileObjectView obj)
    {
        DeselectCurrentObj();
        obj.SetSelected(true);
        currentObj = obj;
        contextActionBarUI.Show(obj.Data);
    }
    void DeselectCurrentObj()
    {
        if (currentObj == null)
            return;
        
        currentObj.SetSelected(false);
        currentObj = null;
        contextActionBarUI.Hide();
    }
    void HandleCancel()
    {
        if (cam.cameraFollow)
        {
            cam.StopFollow();
            return;
        }
     
        world.CancelSelection(); // Clear Highlight on Tile

        DeselectCurrentObj();
    }

}
