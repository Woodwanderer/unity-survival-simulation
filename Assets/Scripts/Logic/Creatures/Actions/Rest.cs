using UnityEngine;

public class Rest : IAction
{
    CharacterSheet stats;
    float comfort = 1f;
    public Rest(CharacterSheet stats, float comfort = 0.5f)
    {
        this.stats = stats;
        this.comfort += comfort;
    }
    public bool IsFinished { get; }

    public void Start()
    {
        
    }
    public void Tick(float dt)
    {
        stats.energy += stats.energyDrainRate * comfort * dt;
        stats.energy = Mathf.Clamp01(stats.energy);
    }
    public void Cancel()
    {
        
    }
    
}
