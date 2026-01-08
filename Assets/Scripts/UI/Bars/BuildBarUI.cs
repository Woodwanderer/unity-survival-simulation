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


}
