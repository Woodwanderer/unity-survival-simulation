using UnityEngine;

public class GameGo: MonoBehaviour
{
    //Variables 
    private World world;
    public RenderWorld renderWorld;         //mono
    private GameState gameState;
    public InputController inputController;
    public TileObjectsDatabase database;
    public CameraMovement cam;
    public InventoryUI inventory;

    public GameTime gameTime = new();

    //UI
    public ActionBarUI actionBar;          
    public TimePanelUI timePanelUI;
    public CharacterSheetUI charSheetUI;
    public ContextActionBarUI contextActionbar;


    private void Start()
    {
        world = new World(database, gameTime); //dlaczego nie moglem dac database w Initialise? spytac gpt stowrzyony tylko pod to przekazanie konstruktor.. aha.. bo te dane. sa wstrzykiwane chyba. reszta genralnie istnieje. To ne jest mono, wiec rzeba podac tak
        world.Initialise();
        renderWorld.Initialise(world);
        gameState = new GameState(world, renderWorld, cam, inputController, inventory, contextActionbar);
        gameState.Initialise();

        //UI
        actionBar.Init(gameState, world.protagonistData.charSheet.actions);
        timePanelUI.Init(gameTime);
        charSheetUI.Init(world.protagonistData.charSheet);
        contextActionbar.Init(world.protagonistData.charSheet.actions);

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
