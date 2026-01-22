using UnityEngine;

public class BuildShelterTool : IGameTool
{
    GameState gameState;
    public BuildShelterTool(GameState gameState)
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
        if (gameState.world.tileSelected == null)
            return;

        if (gameState.input.ConsumeConfirm())
        {
            gameState.world.BuildShelter(gameState.world.tileSelected.Value);
            gameState.SetTool(null);
        }
    }
    public void Tick(float dt)
    {
        gameState.SelectTile();
        OnConfirm();

    }
}
