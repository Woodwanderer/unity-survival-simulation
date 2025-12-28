using JetBrains.Annotations;
using UnityEngine;

public class CharacterActions
{
    CharacterActionState state = CharacterActionState.Idle;
    public CharacterActionState State => state;

    float nutritionValue = 0; // will be gone after food - ItemType improvement
    float hourDuration;
    float nutriRate;
    float nutrition = 0;

    VirtualResources globalRes;

    public CharacterActions(float hourDuration, VirtualResources baseRes)
    {
        this.hourDuration = hourDuration;
        globalRes = baseRes;
        nutriRate = hourDuration * 0.16f; //full bar in 10 minutes // 10 sec in game 1/6th of an hour
    }

    //EAT
    public bool BlocksHunger
    {
        get
        {
            return state == CharacterActionState.Eating;
        }
    }
    public void EatInit(ItemType food)
    {
        nutritionValue = 0.25f; // get from food -> improve ItemType

        int ration = 5;
        if (!globalRes.RemoveItem(food, ration))
        {
            EventBus.Log("You don't have enough food.");
        }
        else
        {

            state = CharacterActionState.Eating;
        }

    }
    public float Eating(float deltaTime)
    {
        if (nutrition >= nutritionValue)
        {
            state = CharacterActionState.Idle;
            nutritionValue = 0;
            nutrition = 0;
        }
        nutrition += deltaTime / nutriRate;
        return deltaTime / nutriRate;
    }
    //HARVEST
    public void HarvestObject(TileObject tileObject)
    {
        
        state = CharacterActionState.Moving;
    }
    
}
