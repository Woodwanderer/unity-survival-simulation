using UnityEngine;

public class EatAction : IAction
{
    Inventory inventory; //Eating from
    ItemDefinition foodType;    //Eating that
    float nutritionValue;
    public float nutrition;     //-hunger: used by CharacterSheet
    CharacterSheet stats;

    bool wasCanceled;
    public bool IsFinished => progress >= 1 || wasCanceled;

    public float progress = 0f; //used by UI
    float unitProgress = 0f;
    float speed;
    int mealAmount;

    public EatAction(Inventory inventory, ItemDefinition foodType, CharacterSheet stats)
    {
        this.inventory = inventory;
        this.foodType = foodType;
        this.stats = stats;
    }
    public void Start()
    {
        nutritionValue = 0.25f; //percent of full HUNGER bar -> how much of a bar it will fill
        mealAmount = 5;         //minimum amount per meal -> gives: nutrition value
        speed = stats.eatSpeed;
    }
    public void Tick(float dt)
    {
        unitProgress += dt * speed;
        progress += dt * speed / mealAmount;

        while (unitProgress >= 1f)
        {
            unitProgress -= 1f;
            inventory.Remove(foodType, 1);
        }
        nutrition += dt * speed * nutritionValue;
    }
    public void Cancel()
    {
        wasCanceled = true;
    }
}
