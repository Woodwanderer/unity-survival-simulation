using UnityEngine;
using UnityEngine.EventSystems;
public class GameState
{
    public InputController input;
    public World world;
    public RenderWorld renderWorld;
    CameraMovement cam;
    //UI
    InventoryUI inventoryUI;
    ContextActionBarUI contextActionBarUI;
    BuildingActionBarUI buildingActionBarUI;
    public BuildBarUI buildBarUI;
    ActionBarUI actionBarUI;
    ModeBarUI taskBarUI;

    TileObjectView currentObj;
    Stockpile currentBuilding;

    public IGameMode currentMode;
    public IGameTool currentTool;
    public GameState(World world, RenderWorld render, CameraMovement cam, InputController input_in, InventoryUI inventoryUI, ContextActionBarUI contextActionBarUI, BuildBarUI buildBarUI, ActionBarUI actionBarUI, ModeBarUI taskBarUI, BuildingActionBarUI buildingActionBarUI)
    {
        this.world = world;
        this.renderWorld = render;
        this.cam = cam;
        this.input = input_in;
        this.inventoryUI = inventoryUI;
        this.contextActionBarUI = contextActionBarUI;
        this.buildingActionBarUI = buildingActionBarUI;
        this.buildBarUI = buildBarUI;
        this.actionBarUI = actionBarUI;
        this.taskBarUI = taskBarUI;
    }

    public void Initialise()
    {
        inventoryUI.Init(world.protagonistData.actions.inventory);
        EventBus.OnObjectClick += SelectObj;
    }
    public void SetMode(IGameMode newMode)
    {
        currentMode?.Exit();
        currentMode = newMode;
        currentMode?.Enter();
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

        SelectAreaBuilding();

        //CANCEL
        if(input.ConsumeCancel()) 
            HandleCancel();
    }
    //Select Tiles
    bool isDragging = false;
    Vector2Int dragStart;
    public void SelectZoneDrag()
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

    //Buildings
    void SelectAreaBuilding()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0)) 
        {
            DeselectCurrentBuilding();
            Vector2Int tileCoords = GetMouseTile();
            TileData tile = world.GetTileData(tileCoords);

            if (!tile.HasBuilding)
                return;

            currentBuilding = tile.stockpile;
            renderWorld.SelectAreaBuilding(currentBuilding, true);
            buildingActionBarUI.Show(tile.stockpile, world.protagonistData.actions);
        }
    }
    void DeselectCurrentBuilding()
    {
        if (currentBuilding == null)
            return;
        renderWorld.SelectAreaBuilding(currentBuilding, false);
        buildingActionBarUI.Hide();
        currentBuilding = null;
    }

    //Objects
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
        SetMode(null);
        if (cam.cameraFollow)
        {
            cam.StopFollow();
            return;
        }
        world.ClearZoneSelection();
        DeselectCurrentObj();
        DeselectCurrentBuilding();
    }

}
