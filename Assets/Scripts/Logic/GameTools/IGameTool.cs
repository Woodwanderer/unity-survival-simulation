using UnityEngine;

public interface IGameTool
{
    void Enter();
    void Exit();
    void Tick(float dt);
}
