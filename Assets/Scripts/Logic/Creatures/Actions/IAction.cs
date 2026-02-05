public interface IAction
{
    ActionStatus Status { get; }
    ActionToken Token { get; set; }
    void Start();
    void Tick(float dt);
    void Cancel();
    bool IsFinished {  get; }
    
}
public enum ActionStatus
{
    NotStarted,
    Running,
    Succeeded,
    Failed,
    Cancelled
}

public readonly struct ActionToken
{
    public readonly int Id;
    public ActionToken(int id) => Id = id;
    public override bool Equals(object  obj)
        => obj is ActionToken other && other.Id == Id;
    public override int GetHashCode()
        => Id.GetHashCode();

}
