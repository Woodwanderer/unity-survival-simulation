using UnityEngine;
using System;
public class CharacterSheet
{
    float hunger = 1f;
    public float Hunger => hunger;
    float hungerRate;
    float starvationThreshold = 0.5f;
    public bool Starvation => hunger < starvationThreshold;
    public event Action OnStarvationStart;

    float hourDuration;

    CharacterActions actions;
    public Inventory inventory;
    //stats
    public float carryWeight = 200;
    //speed
    public float eatSpeed;
    public float harvestSpeed;
    public float buildSpeed = 1f;
    float speedDefault = 2.0f; //walking
    float SpeedMod 
    { 
        get
        {
            if (hunger > starvationThreshold) 
                return 1f;

            float t = Mathf.InverseLerp(starvationThreshold, 0f, hunger);
            t = t * t; //ease-in
            return Mathf.Lerp(0.8f, 0.5f, t);
        }
    }
    public float Speed => speedDefault * SpeedMod;

    public CharacterSheet(float hourDuration, CharacterActions actions)
    {
        this.hourDuration = hourDuration;
        this.actions = actions;
        this.inventory = actions.inventory;

        InitStats();
    }
    public void InitStats()
    {
        hungerRate = hourDuration * 24;
        eatSpeed = 30 / hourDuration;

        harvestSpeed = 100 / hourDuration;
    }
    public void Tick(float deltaTime)
    {
        bool starvingBefore = Starvation;

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

        bool starvingNow = Starvation;

        if (!starvingBefore && starvingNow)
            OnStarvationStart?.Invoke();
    }
}
