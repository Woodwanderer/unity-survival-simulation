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
    public WorldObjData objDatabase;
    public ItemsDatabase itemsDatabase;
    public IActionVisualData IActionVisualData;
    public BiomeData biomeData;

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
        world = new World(objDatabase, itemsDatabase, renderWorld, gameTime, biomeData); 
        world.Initialise(renderWorld);
        renderWorld.Initialise(world);

        //renderWorld.StartDebugWorldGen(world); // LAND GENERATOR DEBUG ANIMATION

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
        float realDt = Time.deltaTime;

        inputController.Tick(realDt);
        gameTime.Tick(realDt);

        float gameDt = gameTime.GameDeltaTime(realDt);

        gameState.Tick(gameDt);
        world.Tick(gameDt);
    }
}
