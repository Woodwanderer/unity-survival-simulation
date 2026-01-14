using System.Collections.Generic;
public class TaskManager
{
    Queue<BuildTask> buildTasks = new();
    public List<Stockpile> stockpiles = new();

    public void Tick(float dt)
    {
        GenerateBuildTasks();
    }
    void GenerateBuildTasks()
    {
        foreach (Stockpile stockpile in stockpiles)
        {
            if (!stockpile.IsConstructed)
            {
                if (!HasTaskFor(stockpile))
                    Add(new BuildTask(stockpile));
            }
        }
    }
    bool HasTaskFor(Stockpile stockpile)
    {
        foreach(BuildTask bT in buildTasks)
        {
            if (bT.stockpile == stockpile) 
                return true;
        }
        return false;
    }

    void Add(ITask task)
    {         
        if (task is BuildTask b)
        {
            buildTasks.Enqueue(b);
        }
    }
    public ITask TakeTask()
    {
        ITask task = null;
        if (buildTasks.Count > 0)
        {
            task = buildTasks.Dequeue();
        }
        return task;
    }
}
