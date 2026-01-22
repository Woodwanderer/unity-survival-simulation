using UnityEngine;

public class BuildBarUI : MonoBehaviour
{
    GameState gameState;
    public void Init(GameState gameState)
    {
        this.gameState = gameState;
    }
    public void Show(bool active)
    {
        gameObject.SetActive(active);
    }
    public void OnStockpileClick()
    {
        if (gameState.currentTool is BuildStockpileTool) 
            return;

        gameState.SetTool(new BuildStockpileTool(gameState));
    }
    public void OnShelterClick()
    {
        if (gameState.currentTool is BuildShelterTool)
            return;

        gameState.SetTool(new BuildShelterTool(gameState));
    }
}
