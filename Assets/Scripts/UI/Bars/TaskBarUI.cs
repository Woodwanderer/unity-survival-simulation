using UnityEngine;
using UnityEngine.UI;

public class TaskBarUI : MonoBehaviour
{
    GameState gameState;
    Button button;
    Image image; 
    public void Init(GameState gameState)
    {
        this.gameState = gameState;

        button = GetComponentInChildren<Button>();
        image = GetComponentInChildren<Image>();
        button.onClick.AddListener(OnBuildClick);
    }
    public void Show(bool active)
    {
        gameObject.SetActive(active);
    }
    public void OnBuildClick()
    {
        if (gameState.currentTool is BuildModeTool)
        {
            gameState.SetTool(null);
            EventBus.Log($"{gameState.currentTool} deactivated.");
            return;
        }

        gameState.SetTool(new BuildModeTool(gameState.buildBarUI));
        EventBus.Log($"{gameState.currentTool} activated.");

    }
}
