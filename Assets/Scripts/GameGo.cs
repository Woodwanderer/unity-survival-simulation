using UnityEngine;

public class GameGo: MonoBehaviour
{
    //Variables 
    private World world;
    public RenderWorld renderWorld;         //mono
    private GameState gameState;
    public InputController inputController;
    public ActionBarUI actionBar;           //mono
    public TileObjectsDatabase database;
    public CameraMovement cam;
    public InventoryUI inventory;

    private void Start()
    {
        world = new World(database); //dlaczego nie moglem dac database w Initialise? spytac gpt stowrzyony tylko pod to przekazanie konstruktor.. aha.. bo te dane. sa wstrzykiwane chyba. reszta genralnie istnieje. To ne jest mono, wiec rzeba podac tak
        world.Initialise();
        renderWorld.Initialise(world);
        gameState = new GameState(world, renderWorld, cam, inputController, inventory);
        gameState.Initialise();
        actionBar.Init(gameState);
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        inputController.Tick(dt);
        gameState.Tick(dt);


        world.Tick(dt);
        renderWorld.Tick(dt);


    }
}
