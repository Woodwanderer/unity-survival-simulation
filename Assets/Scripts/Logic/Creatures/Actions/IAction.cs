public interface IAction
{
    void Start();
    void Tick(float dt);
    void Cancel();
    bool IsFinished {  get; }
    ActionStatus Status { get; }
}
public enum ActionStatus
{
    NotStarted,
    Running,
    Succeeded,
    Failed,
    Cancelled
}
