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

    //UI
    public ActionBarUI actionBarUI;          
    public TimePanelUI timePanelUI;
    public CharacterSheetUI charSheetUI;
    public ContextActionBarUI contextActionbar;
    public BuildBarUI buildBarUI;
    public ModeBarUI modeBarUI;

    private void Start()
    {
        world = new World(objDatabase, itemsDatabase, renderWorld, gameTime); //dlaczego nie moglem dac database w Initialise? spytac gpt stowrzyony tylko pod to przekazanie konstruktor.. aha.. bo te dane. sa wstrzykiwane chyba. reszta genralnie istnieje. To ne jest mono, wiec rzeba podac tak
        world.Initialise(renderWorld);
        renderWorld.Initialise(world);
        gameState = new GameState(world, renderWorld, cam, inputController, inventoryUI, contextActionbar, buildBarUI, actionBarUI, modeBarUI);
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
