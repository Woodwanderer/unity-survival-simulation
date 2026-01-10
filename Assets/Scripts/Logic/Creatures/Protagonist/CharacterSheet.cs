using JetBrains.Annotations;
using System;
using UnityEngine;

public class CharacterSheet
{
    float hunger = 1f;
    public float Hunger => hunger;
    float hungerRate;

    float hourDuration; //calculate stats    

    //ACTIONS
    public CharacterActions actions;
    //stats
    public float eatSpeed;
    public float harvestSpeed;
    public float buildSpeed;
    public float speed { get; private set; } = 2.0f; //walking

    public CharacterSheet(float hourDuration, CharacterActions actions)
    {
        this.actions = actions;
        this.hourDuration = hourDuration;

        InitStats();
    }
    void InitStats()
    {
        hungerRate = hourDuration * 24;
        eatSpeed = 30 / hourDuration;
        harvestSpeed = 100 / hourDuration;
        buildSpeed = 1; //basicaly it's set by building type.. only modifiers applied here from character
    }

    public void Tick(float deltaTime)
    {
        actions.Tick(deltaTime);

        if (!(actions.currentAction is EatAction e))  
        {
            hunger -= deltaTime / hungerRate; // full bar / day
            hunger = Mathf.Clamp01(hunger); // prevents from going below 0
        }
        else
        {
            hunger += e.nutrition;
            hunger = Mathf.Clamp01(hunger);
        }
    }
}
