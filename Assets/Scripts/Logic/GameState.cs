using UnityEngine;
public class GameState
{
    public InputController input;
    World world;
    RenderWorld renderWorld;
    CameraMovement cam;
    //UI
    InventoryUI inventoryUI;
    ContextActionBarUI contextActionBarUI;
    public BuildBarUI buildBarUI;
    ActionBarUI actionBarUI;
    TaskBarUI taskBarUI;

    TileObjectView currentObj;

    public IGameTool currentTool;
    public GameState(World world, RenderWorld render, CameraMovement cam, InputController input_in, InventoryUI inventoryUI, ContextActionBarUI contextActionBarUI, BuildBarUI buildBarUI, ActionBarUI actionBarUI, TaskBarUI taskBarUI)
    {
        this.world = world;
        this.renderWorld = render;
        this.cam = cam;
        this.input = input_in;
        this.inventoryUI = inventoryUI;
        this.contextActionBarUI = contextActionBarUI;
        this.buildBarUI = buildBarUI;
        this.actionBarUI = actionBarUI;
        this.taskBarUI = taskBarUI;
    }

    public void Initialise()
    {
        inventoryUI.Init(world.protagonistData.actions.inventory);
        EventBus.OnObjectClick += HandleObjectClick;
    }
    public void SetTool(IGameTool newTool)
    {
        currentTool?.Exit();
        currentTool = newTool;
        currentTool?.Enter();
    }
    public void Tick(float deltaTime)
    {   
        currentTool?.Tick(deltaTime);

        //CANCEL
        if(input.ConsumeCancel()) 
            HandleCancel();

        if(taskBarUI)
            //button is pressed?

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

            world.SelectConnectedZone(dragStart, dragEnd);
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
        SetTool(null);
        if (cam.cameraFollow)
        {
            cam.StopFollow();
            return;
        }
        world.ClearZoneSelection();
        DeselectCurrentObj();
    }

}
