public class RestGoal : IGoal
{
    public int Priority => (int)GoalPriority.Urgent;
    public bool IsValid => true;
    public bool IsFinished => hero.stats.MaxEnergy;
    CharacterActions hero;
    bool goingHome = false;
    public void Start(CharacterActions hero)
    {
        this.hero = hero;
    }
    public void Tick(float dt)
    {
        if (IsFinished)
            hero.stats.restGoalAssigned = false;

        if (hero.IsResting)
            return;

        if (!goingHome)
        {
            if (hero.TryRest())
                return;

            goingHome = hero.TryMoveToTile(hero.stats.shelter.TileCoords);
            if (!goingHome)
            {
                EventBus.Log($"{hero.stats.name} couldn't find way home!");
                hero.stats.shelter = null;
            }
        }
    }
    public void Cancel()
    {
        hero.stats.restGoalAssigned = false;
    }
}
