using UnityEngine;

public interface IGoal
{
    int Priority    { get; }
    bool IsValid    { get; }
    bool IsFinished { get; }

    void Start(CharacterActions hero);
    void Tick(float dt);
    void Cancel();
}
enum GoalPriority
{
    Survival = 100,
    Urgent   =  50,
    Normal   =  10
}
