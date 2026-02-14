using System.Collections.Generic;
public class GoalManager
{
    //external refs
    public List<CharacterSheet> heroes = new();

    public Queue<Shelter> freeShelters = new();
    public HashSet<Shelter> occupiedSheletrs = new();

    public GoalManager(CharacterSheet hero)
    {
        RegisterHero(hero);
    }
    public GoalManager(List<CharacterSheet> heroes)
    {
        foreach(CharacterSheet hero in heroes)
        {
            RegisterHero(hero);
        }
    }
    void RegisterHero(CharacterSheet hero)
    {
        heroes.Add(hero);
    }
    public void Tick(float dt) // perhaps make it per hour
    {
        ResolveHousing();
        ResolveEnergy();
        ResolveStarvation();

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
            if (hero.Tired && !hero.brain.HasGoal<RestGoal>()) 
            {
                hero.brain.AddGoal(new RestGoal());
            }
        }
    }
    void ResolveStarvation()
    {
        foreach(var hero in heroes)
        {
            if(hero.Starvation && !hero.brain.HasGoal<EnsureFood>())
            {
                hero.brain.AddGoal(new EnsureFood());
            }
        }
    }
}
