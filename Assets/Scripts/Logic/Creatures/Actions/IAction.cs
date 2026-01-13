public interface IAction
{
    void Start();
    void Tick(float dt);
    void Cancel();
    bool IsFinished {  get; }
}
