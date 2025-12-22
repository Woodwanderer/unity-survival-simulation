using System;
using UnityEngine;

public class CharacterSheet
{
    float hunger = 1f;
    public float Hunger => hunger; //Getter so that var hunger is safe and private
    float hourDuration;
    float hungerRate;
    public CharacterActions actions;

    public CharacterSheet(float hourDuration)
    {
        this.hourDuration = hourDuration;
        hungerRate = hourDuration * 24;
        actions = new CharacterActions(hourDuration);
    }
    public void Tick(float deltaTime)
    {
        if (actions.state != CharacterActionState.Eating) 
        {
            hunger -= deltaTime / hungerRate; // full bar / day
            hunger = Mathf.Clamp01(hunger); // prevents from going below 0
        }
        else if (actions.state == CharacterActionState.Eating) 
        {
            hunger += actions.Eating(deltaTime);
            hunger = Mathf.Clamp01(hunger);
        }
        
    }
}
