using JetBrains.Annotations;
using UnityEngine;

public class CharacterActions
{
    public CharacterActionState state = CharacterActionState.Idle;
    float nutritionValue = 0; // will be gone after food - ItemType improvement
    float hourDuration;
    float nutriRate;
    float nutrition = 0;
    public CharacterActions(float hourDuration)
    {
        this.hourDuration = hourDuration;
        nutriRate = hourDuration * 10f; //full bar in 10 minutes // 10 sec in game
    }
    public void EatInit(ItemType food)
    {
        nutritionValue = 0.25f; // get from food -> improve ItemType
        state = CharacterActionState.Eating;
    }
    public float Eating(float deltaTime)
    {
        if (nutrition >= nutritionValue)
        {
            state = CharacterActionState.Idle;
            nutritionValue = 0;
        }
        return nutrition += deltaTime / nutriRate;
    }
    
}
