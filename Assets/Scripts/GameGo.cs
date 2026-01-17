using UnityEngine;

public class GameGo: MonoBehaviour
{
    //Variables 
    private World world;
    public RenderWorld renderWorld;        
    GameState gameState;
    public InputController inputController;
    public CameraMovement cam;
    public InventoryUI inventoryUI;
    public GameTime gameTime = new();

    //Data Assets
    public TileObjectsDatabase objDatabase;
    public ItemsDatabase itemsDatabase;
    public IActionVisualData IActionVisualData;

    //UI
    public ActionBarUI actionBarUI;          
    public TimePanelUI timePanelUI;
    public CharacterSheetUI charSheetUI;
    public ContextActionBarUI contextActionbar;
    public BuildBarUI buildBarUI;
    public ModeBarUI modeBarUI;
    public BuildingBarUI buildingBarUI;

    private void Start()
    {
        world = new World(objDatabase, itemsDatabase, renderWorld, gameTime); 
        world.Initialise(renderWorld);
        renderWorld.Initialise(world);
        gameState = new GameState(world, renderWorld, cam, inputController, inventoryUI, contextActionbar, buildBarUI, actionBarUI, modeBarUI, buildingBarUI);
        gameState.Initialise();

        //UI
        actionBarUI.Init(world.protagonistData.actions);
        timePanelUI.Init(gameTime);
        charSheetUI.Init(world.protagonistData.actions.stats);
        contextActionbar.Init(world.protagonistData.actions);
        buildBarUI.Init(gameState);
        modeBarUI.Init(gameState);
    }
    private void Update()
    {
        float dt = Time.deltaTime;

        gameTime.Tick(dt);
        inputController.Tick(dt);
        gameState.Tick(dt);
        world.Tick(dt);

        //not used yet
        renderWorld.Tick(dt);
    }
}
