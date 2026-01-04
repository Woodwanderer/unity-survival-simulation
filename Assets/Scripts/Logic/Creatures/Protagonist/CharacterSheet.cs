using JetBrains.Annotations;
using System;
using UnityEngine;

public class CharacterSheet
{
    float hunger = 1f;
    public float speed { get; private set; } = 2.0f;
    public float Hunger => hunger; //Getter so that var hunger is safe and private
    float hourDuration;
    float hungerRate;
    public CharacterActions actions;

    public float eatSpeed;

    void InitStats()
    {
        eatSpeed = 30 / hourDuration;
    }

    public CharacterSheet(float hourDuration, World world, ProtagonistData protagonistData, RenderWorld render )
    {
        this.hourDuration = hourDuration;
        hungerRate = hourDuration * 24;
        actions = new CharacterActions(hourDuration, world, protagonistData, render, this);
        actions.Init();
        InitStats();
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
