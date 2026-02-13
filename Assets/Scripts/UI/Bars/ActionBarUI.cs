using UnityEngine;

public class ActionBarUI : MonoBehaviour
{
    CharacterBrain brain;

    public void Init( CharacterBrain brain)
    {
        this.brain = brain;
    }
    public void Eat()
    {
        brain.actionRunner.SetAction(new EatAction(brain));
    }
}
