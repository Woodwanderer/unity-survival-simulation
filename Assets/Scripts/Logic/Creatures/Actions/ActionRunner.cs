using System.Collections.Generic;


public class ActionRunner
{
    World world;
    RenderWorld render;

    //actions
    public IAction currentAction;

    int nextId = 0;

    ActionToken? lastFinishedToken;
    ActionStatus lastFinishedStatus;
    

    public Queue<IAction> actionQueue = new Queue<IAction>();

    public ActionRunner(World world, RenderWorld render)
    {
        this.world = world;
        this.render = render;
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
