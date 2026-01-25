using UnityEngine;

public class IGoalTemplate : IGoal
{
    public int Priority => (int)GoalPriority.Urgent;
    public bool IsValid => true;
    public bool IsFinished => false;

    public void Start(CharacterActions hero)
    {

    }
    public void Tick(float dt)
    {
        // assume control?
    }
    public void Cancel()
    {

    }
}
