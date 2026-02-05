public class RestGoal : IGoal
{
    public int Priority => (int)GoalPriority.Urgent;
    public bool IsValid => !hero.stats.MaxEnergy;
    bool finished = false;
    public bool IsFinished => finished;
    CharacterActions hero;

    ActionToken? goHomeToken;
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
            hero.actionRunner.SetAction(new Rest(hero.stats));
            return;
        }

        if (goHomeToken.HasValue)
        {
            if (hero.actionRunner.HasFinished(goHomeToken.Value, out var status))
            {
                if (status == ActionStatus.Succeeded)
                    hero.actionRunner.SetAction(new Rest(hero.stats));
                if (status == ActionStatus.Failed)
                    hero.stats.shelter = null;
            }
        }
        else
            goHomeToken = hero.actionRunner.SetAction(new Movement(hero.world, hero.stats.shelter.TileCoords));
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
