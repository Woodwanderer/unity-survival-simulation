using UnityEngine;

public class ActionBarUI : MonoBehaviour
{
    CharacterActions characterActions;

    public void Init( CharacterActions actions)
    {
        this.characterActions = actions;
    }
    public void Eat()
    {
        characterActions.TryEat();
    }


}
