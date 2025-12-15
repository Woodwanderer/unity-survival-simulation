using UnityEngine;

public class GameGo: MonoBehaviour
{
    //Variables 
    private World world;
    public RenderWorld renderWorld;
    private GameState gameState;
    public InputController inputController;


    private void Start()
    {
        world = new World();        
        world.Initialise();
        inputController.world = world;
        gameState = new GameState(world, renderWorld);
        gameState.Initialise();
        renderWorld.Initialise(world);
        
    }

}
