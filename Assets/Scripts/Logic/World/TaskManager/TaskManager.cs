using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class TaskManager
{
    Queue<BuildTask> buildTasks = new();
    Queue<HaulTask> haulTasks = new();
    Pathfinder pathfinder;

    public TaskManager(Pathfinder pathfinder)
    {
        this.pathfinder = pathfinder;
    }

    public List<Stockpile> stockpiles = new();
    public List<ResourcePile> piles = new();

    public void Tick(float dt)
    {
        GenerateBuildTasks();
        GenerateHaulTasks();
    }
    void GenerateHaulTasks()
    {
        piles.RemoveAll(pile => pile.IsEmpty);

        foreach(var pile  in piles)
        {
            if (HasHaulTaskFor(pile))
            {
                continue;
            }

            Stockpile closest = GetClosestStockpile(pile);

            if (closest == null)
                continue;

            List<Vector2Int> deliveryPath = new(pathfinder.FindPath(pile.TileCoords, closest.area.center));

            if (deliveryPath != null)
                haulTasks.Enqueue(new HaulTask(pile, closest, deliveryPath));
        }
    }
    Stockpile GetClosestStockpile(ResourcePile pile)
    {
        Stockpile target = null;
        int bestDist = int.MaxValue;
        foreach (var stockpile in stockpiles)
        {
            if (!stockpile.IsConstructed)
                continue;
            int capacity = stockpile.CalculateFreeSpaceFor(pile.Slot);
            if (capacity == 0) 
                continue;

            int dist = (pile.TileCoords - stockpile.area.center).sqrMagnitude;
            if (dist < bestDist)
            {
                bestDist = dist;
                target = stockpile;
            }
        }
        return target;
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
    bool HasHaulTaskFor(ResourcePile pile)
    {
        foreach (HaulTask haulTask in haulTasks)
        {
            if (haulTask.source == pile)
            {
                return true;
            }
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
        while (buildTasks.Count > 0) 
        {
            task = buildTasks.Dequeue();
            if (task.IsValid) 
                return task;
        }
        while (haulTasks.Count > 0)
        {
            task = haulTasks.Dequeue();
            if (task.IsValid)
                return task;
        }

        return null;
    }
}
