using UnityEngine;
using System;
public class CharacterSheet
{
    public String name = "Psylocyb Bong";
    public CharacterActions actions;
    public Inventory inventory;

    //housing
    public Shelter shelter = null;
    public bool IsHomeless => shelter == null;
    float hourDuration = Game.Config.hourDuration;

    //hunger
    float hunger = 1f;
    public float Hunger => hunger;
    float hungerRate;
    float starvationThreshold = 0.5f;
    public bool Starvation => hunger < starvationThreshold;
    public event Action OnStarvationStart;

    //energy
    public float energy = 1f;
    public bool MaxEnergy => energy >= 0.95f;
    public float energyDrainRate;
    public float energyGainRate;
    public float energyCapacityWh;
    float lowEnergyThreshold = 0.3f;
    public bool EnergyLow => energy < lowEnergyThreshold;
    public bool restGoalAssigned = false;

    //stats
    public float carryWeight = 200;
    //speed
    public float eatSpeed;
    public float harvestSpeed;
    public float buildSpeed = 1f;
    float speedDefault = 2.0f; //walking
    bool hasSpeedDebuff = false;
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

    public CharacterSheet( CharacterActions actions)
    {
        this.actions = actions;
        this.inventory = actions.inventory;

        InitStats();
    }
    public void InitStats()
    {
        hungerRate = hourDuration * 24;

        energyCapacityWh = hourDuration * 10;
        energyDrainRate = 1/energyCapacityWh;

        SetSpeed();

    }
    public void SetSpeed(float speedMod = 1f)
    {
        eatSpeed     = (30  / hourDuration) * speedMod;
        harvestSpeed = (100 / hourDuration) * speedMod;
        buildSpeed   = (1f                ) * speedMod;
        speedDefault = (2.0f              ) * speedMod;

    }
    public void Tick(float deltaTime)
    {
        //hunger
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


        HandleEnergyConsumption(deltaTime);
        if (EnergyLow)
        {
            SetSpeed(0.6f);
            hasSpeedDebuff = true;
        }
        else if (hasSpeedDebuff) 
        {
            SetSpeed();
            hasSpeedDebuff = false;
        }
        

    }
    void HandleEnergyConsumption(float deltaTime)
    {
        if (!actions.IsWorking)
            return;

        energy -= energyDrainRate * deltaTime;
        energy = Mathf.Clamp01(energy);
    }
    
}
