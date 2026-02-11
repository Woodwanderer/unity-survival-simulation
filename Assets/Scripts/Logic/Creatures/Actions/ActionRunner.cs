using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;


public class ActionRunner
{
    //External Data
    World world;
    //Deriveratives
    RenderWorld render;

    //actions
    public IAction currentAction;

    int nextId = 0;

    ActionToken? lastFinishedToken;
    ActionStatus lastFinishedStatus;
    

    public Queue<IAction> actionQueue = new Queue<IAction>();

    public ActionRunner(World world)
    {
        this.world = world;
        this.render = world.render;
    }
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

    public void Tick(float dt)
    {
        currentAction?.Tick(dt);

        if (currentAction != null && currentAction.IsFinished)
        {
            //TODO: Clearing after action -> remove to other decision making entity 
            if (currentAction is HarvestAction h && h.targetObj.harvestSource.Depleted)
            {
                world.ClearTileEntity(h.targetObj);
                render.RemoveObjectSprite(h.targetObj);
            }
            if (currentAction is CollectItem c && c.pile.IsEmpty) 
            {
                world.ClearTileEntity(c.pile);
                render.RemoveObjectSprite(c.pile);
            }

            FinishCurrentAction();

            if (actionQueue.Count > 0)
                SetAction(actionQueue.Dequeue());
        }
    }
}
