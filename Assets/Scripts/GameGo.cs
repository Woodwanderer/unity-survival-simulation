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

    private void Start()
    {
        world = new World(database);        
        world.Initialise();
        inputController.world = world;
        renderWorld.Initialise(world);
        gameState = new GameState(world, renderWorld);
        gameState.Initialise();
        actionBar.Init(gameState);

        
        
    }

}
