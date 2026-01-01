using UnityEngine;

public class ActionBarUI : MonoBehaviour
{
    private CharacterActions characterActions;

    public void Init( CharacterActions actions)
    {
        this.characterActions = actions;
    }
    public void Eat()
    {
        characterActions.EatInit(ItemType.FoodRaw); //pick from inv
    }


}
