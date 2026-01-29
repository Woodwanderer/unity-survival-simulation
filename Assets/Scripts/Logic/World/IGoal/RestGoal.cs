public class RestGoal : IGoal
{
    public int Priority => (int)GoalPriority.Urgent;
    public bool IsValid => !hero.stats.MaxEnergy;
    bool finished = false;
    public bool IsFinished => finished;
    CharacterActions hero;
    bool goingHome = false;
    public void Start(CharacterActions hero)
    {
        this.hero = hero;
    }
    public void Tick(float dt)
    {
        if (!IsValid)
            OnFinish();

        if (hero.IsResting)
            return;

        if (hero.stats.IsHomeless) 
        {
            hero.SetAction(new Rest(hero.stats));
            return;
        }

        if (hero.protagonistData.mapCoords == hero.stats.shelter.TileCoords) 
        {
            hero.SetAction(new Rest(hero.stats));
            return;
        }

        if (goingHome)
            return;

        goingHome = hero.TryMoveToTile(hero.stats.shelter.TileCoords);

        if (!goingHome) 
        {
            EventBus.Log($"{hero.stats.name} couldn't find way home!");
            hero.stats.shelter = null;
            return;
        }

    }
    public void Cancel()
    {
        hero.stats.restGoalAssigned = false;
    }
    public void OnFinish()
    {
        hero.stats.restGoalAssigned = false;
        finished = true;
    }
}
