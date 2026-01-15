using UnityEngine;

public class HaulAction : IAction
{
    public float progress;
    public bool IsFinished => progress >= 1f;
    public void Start()
    {

    }
    public void Tick(float dt)
    {

    }
    public void Cancel()
    {

    }
}
