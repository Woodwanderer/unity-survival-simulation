using JetBrains.Annotations;
using System;
using UnityEngine;

public class CharacterSheet
{
    float hunger = 1f;
    public float Hunger => hunger; //Getter so that var hunger is safe and private
    float hourDuration;
    float hungerRate;
    public CharacterActions actions;
    VirtualResources invChar;

    public CharacterSheet(float hourDuration, VirtualResources inv, World world, ProtagonistData protagonistData, RenderWorld render )
    {
        this.hourDuration = hourDuration;
        this.invChar = inv;
        hungerRate = hourDuration * 24;
        actions = new CharacterActions(hourDuration, invChar, world, protagonistData, render);
        actions.Init();
    }
    public void Tick(float deltaTime)
    {
        actions.Tick(deltaTime);

        if (!actions.BlocksHunger) 
        {
            hunger -= deltaTime / hungerRate; // full bar / day
            hunger = Mathf.Clamp01(hunger); // prevents from going below 0
        }
        else
        {
            hunger += actions.Eating(deltaTime);
            hunger = Mathf.Clamp01(hunger);
        }
        
    }
}
