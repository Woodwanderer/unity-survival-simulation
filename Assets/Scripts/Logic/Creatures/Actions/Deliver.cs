using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Deliver : IAction
{
    ItemSlot source;
    Stockpile destination;
    
    public float progress;
    float unitProgress;
    float speed;
    int targetAmount;
    
    public bool IsFinished => progress >= 1f;
    public Deliver(ItemSlot source, CharacterSheet stats, Stockpile destination)
    {
        this.source = source;
        this.speed = stats.harvestSpeed;
        this.destination = destination;
    }
    
    public void Start()
    {
        targetAmount = source.Amount;
    }
    public void Tick(float dt)
    {
        unitProgress += dt * speed;
        progress += dt * speed / targetAmount;

        while (unitProgress >= 1f)
        {
            unitProgress -= 1;
            source.Remove(1);
            destination.Add(source.Item, 1);
        }
    }
    public void Cancel()
    {
        
    }
}
