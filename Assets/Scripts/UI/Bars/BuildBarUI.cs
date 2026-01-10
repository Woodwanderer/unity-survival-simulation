using UnityEngine;
using UnityEngine.UI;

public class BuildBarUI : MonoBehaviour
{
    Button button;
    GameState gameState;
    public void Init(GameState gameState)
    {
        this.gameState = gameState;

        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(OnStockpileClick);
    }
    public void Show(bool active)
    {
        gameObject.SetActive(active);
    }
    void OnStockpileClick()
    {
        if (gameState.currentTool is BuildStockpileTool) 
            return;

        gameState.SetTool(new BuildStockpileTool(gameState));
    }

}
