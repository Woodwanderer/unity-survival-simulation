using UnityEngine;

public class BuildMode : IGameMode
{
    BuildBarUI buildBarUI;
    public BuildMode(BuildBarUI buildBarUI)
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
}
