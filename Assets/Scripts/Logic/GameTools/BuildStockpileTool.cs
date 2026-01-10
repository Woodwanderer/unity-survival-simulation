using UnityEngine;

public class BuildStockpileTool : IGameTool
{
    GameState gameState;
    public BuildStockpileTool(GameState gameState)
    {
        this.gameState = gameState;
    }
    public void Enter()
    {
        
    }
    public void Exit()
    {

    }
    public void OnConfirm()
    {
        if(gameState.world.area == null)
            return;

        if (gameState.input.ConsumeConfirm()) 
        {
            gameState.world.BuildStockpile();
            gameState.SetTool(null);
        }
    }
    public void Tick(float dt)
    {
        gameState.SelectZoneDrag();
        OnConfirm();
        
    }
}
