using System.Collections.Generic;

public class GoalManager
{
    //external refs
    public List<CharacterSheet> heroes = new();

    public Queue<Shelter> freeShelters = new();
    public HashSet<Shelter> occupiedSheletrs = new();


    public GoalManager(CharacterSheet hero)
    {
        heroes.Add(hero);
    }
    public GoalManager(List<CharacterSheet> heroes)
    {
        this.heroes = heroes;
    }

    public void Tick(float dt) // perhaps make it per hour
    {
        ResolveHousing();
        ResolveEnergy();
    }
    void ResolveHousing()
    {
        foreach (var hero in heroes) 
        {
            if (!hero.IsHomeless)
                continue;

            if (freeShelters.Count == 0)
                return;

            Shelter shelter = freeShelters.Peek();
            hero.shelter = shelter;
            shelter.Capacity--;

            if (shelter.Capacity == 0)
            {
                freeShelters.Dequeue();
                occupiedSheletrs.Add(shelter);
            }
        }
    }
    void ResolveEnergy()
    {
        foreach (var hero in heroes)
        {
            if (hero.EnergyLow && !hero.restGoalAssigned) 
            {
                IGoal rest = new RestGoal();
                SetGoal(hero.actions, rest);
                hero.restGoalAssigned = true;
            }
        }
    }
    
    void SetGoal(CharacterActions hero, IGoal newGoal)
    {
        if (hero.currentGoal == null)
        {
            hero.currentGoal = newGoal;
            hero.currentGoal.Start(hero);
            EventBus.Log($"Added new Goal: {newGoal}");
            return;
        }
        if (hero.currentGoal.Priority >= newGoal.Priority)
        {
            hero.goals.Add(newGoal);
            EventBus.Log($"Added new Goal to queue: {newGoal}");
        }
        else
        {
            hero.goals.Add(hero.currentGoal);
            hero.currentGoal = newGoal;
            hero.currentGoal.Start(hero);
            EventBus.Log($"Replaced current Goal with :{newGoal}");
        }
    }

}
