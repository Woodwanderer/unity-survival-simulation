using UnityEngine;

public class EatAction : IAction
{
    //External Data
    Inventory inventory;        //Eating from
    ItemDefinition foodType;    //Eating that
    CharacterSheet stats;

    float nutritionValue;
    public float nutrition;     //-hunger: used by CharacterSheet

    //Deneric IAction
    public ActionToken Token {  get; set; }
    public ActionStatus Status { get; private set; } = ActionStatus.NotStarted;

    public bool IsFinished => Status == ActionStatus.Succeeded || Status == ActionStatus.Cancelled;

    public float progress = 0f;
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

        Status = ActionStatus.Running;
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
        nutrition = dt * speed / mealAmount * nutritionValue;

        if (progress >= 1f)
            Status = ActionStatus.Succeeded;
    }
    public void Cancel()
    {
        Status = ActionStatus.Cancelled;
    }
}
