using UnityEngine;

public class BuildAction : IAction
{
    //Exterior Data
    World world;
    Building building;

    //Deriveratives
    float workTime;
    float speed;

    //Generic IAction
    public ActionToken Token {  get; set; }
    public ActionStatus Status { get; private set; } = ActionStatus.NotStarted;

    public float progress = 0f;
    public bool IsFinished => Status == ActionStatus.Succeeded;

    public BuildAction(Building building, CharacterSheet stats, World world)
    {
        this.world = world;
        this.building = building;

        workTime = building.WorkTime;
        speed = stats.buildSpeed;
    }
    public void Start()
    {
        progress = building.constructionProgress;

        Status = ActionStatus.Running;
    }
    public void Tick(float dt)
    {
        if (IsFinished)
            return;

        progress += dt * speed / workTime;     
        progress = Mathf.Clamp01(progress);

        building.constructionProgress = progress;
        world.render.UpdateBuildingAppearance(building);

        if (progress >= 1f)
        {
            world.OnBuildingConstructed(building);
            Status = ActionStatus.Succeeded;
        }
    }    
    public void Cancel()
    {
        Status = ActionStatus.Cancelled;
    }
 }
   


