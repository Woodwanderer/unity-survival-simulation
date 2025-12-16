using UnityEngine;

public class ActionBarUI : MonoBehaviour
{
    private GameState gameState;

    public void Init(GameState gameState)
    {
        this.gameState = gameState;
    }
    public void Harvest()
    {
        gameState.AttemptHarvest();
    }


}
