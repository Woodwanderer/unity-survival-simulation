using UnityEngine;

public class EnsureFood : IGoal
{
    CharacterActions hero;
    bool done = false;
    public int Priority => (int)GoalPriority.Survival;
    public bool IsValid => true;
    public bool IsFinished => done;

    public void Start(CharacterActions hero)
    {
        this.hero = hero;
    }
    public void Tick(float dt)
    {
        if (hero.currentAction != null)
            return;

        if (hero.TryEat()) 
        {
            done = true;
            return;
        }

        ItemDefinition food = hero.world.itemsDatabase.Get("foodRaw");
        ItemSlot order = new(food, 5);

        hero.FindNearest(order);
    }
    public void Cancel()
    {

    }

}
