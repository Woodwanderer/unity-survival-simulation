using UnityEngine;

public class BuildAction : IAction
{
    Stockpile stockpile;
    float workTime;
    public float progress = 0f;
    float speed;
    public bool IsFinished => progress == 1f;
    public BuildAction(Stockpile stockpile, CharacterSheet stats)
    {
        this.stockpile = stockpile;
        workTime = stockpile.workTime;
        speed = stats.buildSpeed;
    }
    public void Start()
    {
        progress = stockpile.constructionProgress;
    }
    public void Tick(float dt)
    {
        progress += dt * speed / workTime;
    }
    public void Cancel()
    {
        stockpile.constructionProgress = progress;
    }
   

}
