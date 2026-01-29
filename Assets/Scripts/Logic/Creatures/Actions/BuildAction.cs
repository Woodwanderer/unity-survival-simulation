using UnityEngine;

public class BuildAction : IAction
{
    World world;
    Building building;
    float workTime;
    public float progress = 0f;
    float speed;
    public bool IsFinished => progress >= 1f;

    public BuildAction(Building building, CharacterSheet stats, World world)
    {
        this.world = world;
        this.building = building;
        workTime = building.WorkTime;
        speed = stats.buildSpeed;
    }
    public ActionStatus Status { get; private set; } = ActionStatus.NotStarted;
    public void Start()
    {
        progress = building.constructionProgress;
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
            world.OnBuildingConstructed(building);
    }    
    public void Cancel()
    {

    }
 }
   


