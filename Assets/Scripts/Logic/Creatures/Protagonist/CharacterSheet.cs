using System;
using UnityEngine;

public class CharacterSheet
{
    float hunger = 1f;
    public float Hunger => hunger; //Getter so that var hunger is safe and private
    float hourDuration;
    float hungerRate;
    public CharacterSheet(float hourDuration)
    {
        this.hourDuration = hourDuration;
        hungerRate = hourDuration * 24;
    }
    public void Tick(float deltaTime)
    {
        hunger -= deltaTime / hungerRate; // full bar / day
        hunger = Mathf.Clamp01(hunger); // prevents from going below 0
    }
}
