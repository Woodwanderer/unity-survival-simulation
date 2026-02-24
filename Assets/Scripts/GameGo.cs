using UnityEngine;

public class GameGo: MonoBehaviour
{
    //Variables 
    private World world;
    public RenderWorld renderWorld;        
    GameState gameState;

    //Input & Events
    public InputController inputController;
    public CameraMovement cam;
    
    public GameTime gameTime = new();

    //Data Assets
    public GameConfig gameConfig;
    public WorldData worldData;
    public IActionVisualData IActionVisualData;
    
    //UI
    public ActionBarUI actionBarUI;          
    public TimePanelUI timePanelUI;
    public CharacterSheetUI charSheetUI;
    public ContextActionBarUI contextActionbar;
    public InventoryUI inventoryUI;

    public BuildBarUI buildBarUI;
    public ModeBarUI modeBarUI;
    public BuildingBarUI buildingBarUI;

    public NighttimeOverlay nighttimeOverlay;
    public GameSpeedUI gameSpeedUI;

    private void Awake()
    {
        Game.Config = gameConfig;
    }
    private void Start()
    {
        world = new World(worldData, renderWorld); 
        world.Initialise();
        renderWorld.Initialise(world);

        //renderWorld.StartDebugWorldGen(world); // LAND GENERATOR DEBUG ANIMATION

        gameState = new GameState(world, renderWorld, cam, inputController, inventoryUI, contextActionbar, buildBarUI, actionBarUI, modeBarUI, buildingBarUI, charSheetUI);
        gameState.Initialise();

        //UI
        actionBarUI.Init(world.protagonistData.brain);
        timePanelUI.Init(gameTime);
        contextActionbar.Init(world.protagonistData.brain);
        buildBarUI.Init(gameState);
        modeBarUI.Init(gameState);
        nighttimeOverlay.Init(gameTime);
        gameSpeedUI.Init(gameTime);

    }
    private void Update()
    {
        float realDt = Time.deltaTime;

        inputController.Tick(realDt);
        gameTime.Tick(realDt);

        float gameDt = gameTime.GameDeltaTime(realDt);

        nighttimeOverlay.Tick(gameDt);
        gameState.Tick(gameDt);
        world.Tick(gameDt);
    }
}
