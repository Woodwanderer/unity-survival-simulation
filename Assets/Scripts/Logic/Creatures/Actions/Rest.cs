using UnityEngine;

public class Rest : IAction
{
    CharacterSheet stats;
    float comfort;
    public Rest(CharacterSheet stats, float comfort = 0.5f)
    {
        this.stats = stats;
        this.comfort = comfort;
    }
    public bool IsFinished => stats.MaxEnergy;
    public ActionStatus Status { get; private set; } = ActionStatus.NotStarted;
    public void Start()
    {
        comfort = 1f + comfort;
    }
    public void Tick(float dt)
    {
        stats.energy += stats.energyDrainRate * comfort * dt;
        stats.energy = Mathf.Clamp01(stats.energy);
    }
    public void Cancel()
    {
        comfort = 1f;
    }
    
}
