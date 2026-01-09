using UnityEngine;

public class BuildModeTool : IGameTool
{
    BuildBarUI buildBarUI;
    public BuildModeTool(BuildBarUI buildBarUI)
    {        
        this.buildBarUI = buildBarUI;
    }
    public void Enter()
    {
        buildBarUI.Show(true);
    }
    public void Exit()
    {
        buildBarUI.Show(false);
    }
    public void Tick(float dt)
    {

    }
}
