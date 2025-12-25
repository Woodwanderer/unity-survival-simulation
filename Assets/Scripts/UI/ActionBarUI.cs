using UnityEngine;

public class ActionBarUI : MonoBehaviour
{
    private GameState gameState;
    private CharacterActions characterActions;

    public void Init(GameState gameState, CharacterActions actions)
    {
        this.gameState = gameState;
        this.characterActions = actions;
    }

    public void Harvest()
    {
        gameState.AttemptHarvest();
    }
    public void Eat()
    {
        characterActions.EatInit(ItemType.FoodRaw); //pick from inv
    }


}
