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
        inventoryUI.Init(world.protagonistData.actions.inventory);
        EventBus.OnObjectClick += HandleObjectClick;
    }
    public void Tick(float deltaTime)
    {   
        //CANCEL
        if(inputContr.ConsumeCancel()) 
            HandleCancel();

        SelectZoneInput();
    }
    //Select Tiles
    bool isDragging = false;
    Vector2Int dragStart;
    public void SelectZoneInput()
    {
        if (Input.GetMouseButtonDown(0) && isDragging == false) 
        {
            dragStart = GetMouseTile();
            isDragging = true;
        }
        if (Input.GetMouseButton(0) && isDragging) 
        {
            Vector2Int current = GetMouseTile();
            world.SelectZone(dragStart, current);
        }
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            Vector2Int dragEnd = GetMouseTile();

            world.SelectConditionedZone(dragStart, dragEnd);
            isDragging = false;
        }
    }
    Vector2Int GetMouseTile()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;
        return  renderWorld.WorldToMap(worldPos);
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
        world.ClearZoneSelection();
        DeselectCurrentObj();
    }

}
