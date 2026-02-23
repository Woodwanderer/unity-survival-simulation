using System.Collections.Generic;
public class ActionRunner
{    
    public IAction currentAction;

    int nextId = 0;

    ActionToken? lastFinishedToken;
    ActionStatus lastFinishedStatus;

    public Queue<IAction> actionQueue = new Queue<IAction>();

    public ActionToken SetAction(IAction newAction)
    {
        var token = new ActionToken(++nextId);
        newAction.Token = token;

        currentAction?.Cancel();
        currentAction = newAction;
        currentAction?.Start();

        return token;
    }
    void FinishCurrentAction()
    {
        lastFinishedToken = currentAction.Token;
        lastFinishedStatus = currentAction.Status;
        currentAction = null;
    }
    public bool HasFinished(ActionToken token, out ActionStatus status)
    {
        if (lastFinishedToken.HasValue && lastFinishedToken.Value.Equals(token)) 
        {
            status = lastFinishedStatus;
            return true;
        }

        status = default;
        return false;
    }
    public void ExecutePlan(IPlan plan)
    {
        bool first = true;

        foreach (var action in plan.Build())
        {
            if (first)
            {
                SetAction(action);
                first = false;
            }
            else
            {
                actionQueue.Enqueue(action);
            }
        }
    }
    public void ClearQueue()
    {
        actionQueue.Clear();
    }

    public void Tick(float dt)
    {
        if (currentAction != null && currentAction.IsFinished)
        {
            FinishCurrentAction();

            if (actionQueue.Count > 0)
                SetAction(actionQueue.Dequeue());
        }
        currentAction?.Tick(dt);
    }
}
