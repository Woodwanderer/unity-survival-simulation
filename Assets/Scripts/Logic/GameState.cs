using UnityEngine;
using UnityEngine.EventSystems;
public class GameState
{
    //External Data
    public InputController input;

    public World world;
    public RenderWorld renderWorld;

    CameraMovement cam;
    //UI
    InventoryUI inventoryUI;
    ContextActionBarUI contextActionBarUI;
    BuildingBarUI buildingBarUI;
    public BuildBarUI buildBarUI;
    ActionBarUI actionBarUI;
    ModeBarUI taskBarUI;

    CharacterSheetUI characterSheetUI;

    //This
    TileEntityView currentObj;
    Building currentBuilding;
    PlayerCommandRouter pCR;

    public IGameMode currentMode;
    public IGameTool currentTool;
    public GameState(World world, RenderWorld render, CameraMovement cam, InputController input_in, InventoryUI inventoryUI, ContextActionBarUI contextActionBarUI, BuildBarUI buildBarUI, ActionBarUI actionBarUI, ModeBarUI taskBarUI, BuildingBarUI buildingBarUI, CharacterSheetUI characterSheetUI)
    {
        this.world = world;
        this.renderWorld = render;
        this.cam = cam;
        this.input = input_in;
        this.inventoryUI = inventoryUI;
        this.contextActionBarUI = contextActionBarUI;
        this.buildingBarUI = buildingBarUI;
        this.buildBarUI = buildBarUI;
        this.actionBarUI = actionBarUI;
        this.taskBarUI = taskBarUI;

        this.characterSheetUI = characterSheetUI;
    }

    public void Initialise()
    {
        inventoryUI.Init(world.protagonistData.brain.inventory, world.protagonistData.brain.stats);

        EventBus.OnObjectClick += SelectObj;

        pCR = new PlayerCommandRouter(world.protagonistData.brain);
        characterSheetUI.Init(pCR, world.protagonistData.brain);
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

        PrintMouseTileInfo();
        SelectAreaBuilding();

        //CANCEL
        if(input.ConsumeCancel()) 
            HandleCancel();
    }

    //Select Tiles
    bool isDragging = false;
    Vector2Int dragStart;
    void PrintMouseTileInfo()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!Input.GetMouseButtonDown(0)) 
            return;

        if (!TryGetMouseTile(out Vector2Int coords))
            return;

        TileData tile = world.GetTileData(coords);
        EventBus.Log($"There's a {tile.biome} biome here.");
        foreach(var ent  in tile.entities)
        {
            if (ent is WorldObject w)
            {
                EventBus.Log($"There're a {w.Definition.objType} here.");
            }
        }
    }
    public void SelectTile()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!TryGetMouseTile(out Vector2Int coords))
                return;

            world.SelectTile(coords);
        }
    }
    public void SelectZoneDrag()
    {
        if (Input.GetMouseButtonDown(0) && !isDragging) 
        {
            if (!TryGetMouseTile(out dragStart))
                return;

            isDragging = true;
        }
        if (Input.GetMouseButton(0) && isDragging) 
        {
            if (TryGetMouseTile(out Vector2Int current)) 
                world.SelectZone(dragStart, current);
        }
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            if (TryGetMouseTile(out Vector2Int dragEnd)) 
                world.SelectConnectedZone(dragStart, dragEnd);

            isDragging = false;
        }
    }
    bool TryGetMouseTile(out Vector2Int tile)
    {
        tile = default;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Vector2Int coords = renderWorld.WorldToMap(worldPos);

        if (!world.pathfinder.IsWithinWorld(coords))
            return false;
            
        tile = coords;
        return true;
    }

    //Buildings
    void SelectAreaBuilding()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0)) 
        {
            DeselectCurrentBuilding();

            if (!TryGetMouseTile(out Vector2Int tileCoords))
                return;

            TileData tile = world.GetTileData(tileCoords);

            if (!tile.HasBuilding)
                return;

            currentBuilding = tile.building;
            renderWorld.SelectAreaBuilding(currentBuilding, true);

            if (currentBuilding is Stockpile s) 
                buildingBarUI.Show(s, world.protagonistData.brain);
        }
    }
    void DeselectCurrentBuilding()
    {
        if (currentBuilding == null)
            return;

        renderWorld.SelectAreaBuilding(currentBuilding, false);
        buildingBarUI.Hide();
        currentBuilding = null;
    }

    //Objects
    void SelectObj(TileEntityView obj)
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
    
    //General
    void HandleCancel()
    {
        SetTool(null);
        SetMode(null);
        if (cam.cameraFollow)
        {
            cam.StopFollow();
            return;
        }
        world.ClearTileSelected();
        world.ClearZoneSelection();
        DeselectCurrentObj();
        DeselectCurrentBuilding();
    }

}
