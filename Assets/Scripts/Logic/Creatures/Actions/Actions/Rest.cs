using UnityEngine;

public class Rest : IAction
{
    public ActionToken Token {  get; set; }
    public ActionStatus Status { get; private set; } = ActionStatus.NotStarted;

    CharacterSheet stats;
    float comfort = 1f;
    public Rest(CharacterSheet stats)
    {
        this.stats = stats;
    }
    public bool IsFinished 
        => Status == ActionStatus.Succeeded;
    
    public void Start()
    {
        if (!stats.IsHomeless)
        {
            comfort += stats.shelter.Comfort;
        }
        Status = ActionStatus.Running;
    }
    public void Tick(float dt)
    {
        stats.energy += stats.energyDrainRate * comfort * dt;
        stats.energy = Mathf.Clamp01(stats.energy);

        if (stats.MaxEnergy) 
            Status = ActionStatus.Succeeded;

    }
    public void Cancel()
    {
        comfort = 1f;
        Status = ActionStatus.Cancelled;
    }
    
}
