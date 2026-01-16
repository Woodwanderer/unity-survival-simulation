using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Deliver : IAction
{
    Inventory inventory;
    Stockpile destination;
    
    public float progress;
    float unitProgress;
    float speed;
    int targetAmount;
    
    public bool IsFinished => progress >= 1f;
    public Deliver(Inventory inventory, CharacterSheet stats, Stockpile destination)
    {
        this.inventory = inventory;
        this.speed = stats.harvestSpeed;
        this.destination = destination;
    }
    
    public void Start()
    {
        targetAmount = 0;
        foreach (var slot in inventory.Slots)
        {
            if (!slot.IsEmpty) 
                targetAmount += slot.Amount;
        }
    }
    public void Tick(float dt)
    {
        if (inventory.IsEmpty)
        {
            progress = 1f;
            return;
        }

        unitProgress += dt * speed;
        progress += dt * speed / targetAmount;

        while (unitProgress >= 1f)
        {
            unitProgress -= 1;
            ItemSlot slot = inventory.Slots.FirstOrDefault(s => !s.IsEmpty);

            int overflow = destination.Add(slot.Item, 1);
            if (overflow == 0)
                slot.Remove(1);
            else
            {
                //Destination is Full
                progress = 1f;
                return;
            }
        }
    }
    public void Cancel()
    {
        
    }
}
