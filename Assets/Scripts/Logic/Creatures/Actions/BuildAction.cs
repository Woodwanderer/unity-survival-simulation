using UnityEngine;

public class BuildAction : IAction
{
    RenderWorld render;
    Stockpile stockpile;
    float workTime;
    public float progress = 0f;
    float speed;
    public bool IsFinished => progress >= 1f;
    public BuildAction(Stockpile stockpile, CharacterSheet stats, RenderWorld render)
    {
        this.render = render;
        this.stockpile = stockpile;
        workTime = stockpile.WorkTime;
        speed = stats.buildSpeed;
    }
    public void Start()
    {
        progress = stockpile.constructionProgress;
    }
    public void Tick(float dt)
    {
        progress += dt * speed / workTime;        
        stockpile.constructionProgress = progress;

        if (progress >= 1f)
            OnFinished();
    }
    void OnFinished()
    {
        stockpile.constructionProgress = 1f;
        render.ShowStockpile(stockpile);
    }
    public void Cancel()
    {
        stockpile.constructionProgress = progress;
    }
 }
   


