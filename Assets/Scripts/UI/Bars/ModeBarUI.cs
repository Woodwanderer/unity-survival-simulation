using UnityEngine;
using UnityEngine.UI;

public class ModeBarUI : MonoBehaviour
{
    GameState gameState;
    Button button;
    public void Init(GameState gameState)
    {
        this.gameState = gameState;

        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(OnBuildClick);
    }
    public void Show(bool active)
    {
        gameObject.SetActive(active);
    }
    public void OnBuildClick()
    {
        if (gameState.currentMode is BuildMode)
        {
            gameState.SetTool(null);
            gameState.SetMode(null);
            EventBus.Log($"{gameState.currentMode} deactivated.");
            return;
        }

        gameState.SetMode(new BuildMode(gameState.buildBarUI));
        EventBus.Log($"{gameState.currentMode} activated.");

    }
}
