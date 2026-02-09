using UnityEngine;

public class ActionBarUI : MonoBehaviour
{
    CharacterActions brain;

    public void Init( CharacterActions brain)
    {
        this.brain = brain;
    }
    public void Eat()
    {
        brain.actionRunner.SetAction(new EatAction(brain));
    }
}
