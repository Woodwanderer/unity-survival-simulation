using UnityEngine;

public class TaskBarUI : MonoBehaviour
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
}
