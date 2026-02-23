using System.Collections.Generic;
using UnityEngine;
public class TaskManager
{
    List<BuildTask> buildTasks = new();
    List<HaulTask> haulTasks = new();
    Pathfinder pathfinder;

    public TaskManager(Pathfinder pathfinder)
    {
        this.pathfinder = pathfinder;
    }

    public List<ResourcePile> piles = new();

    public List<Stockpile> stockpiles = new();

    public List<Building> constructions = new();

    public List<Building> buildings = new();

    public void Tick(float dt)
    {
        UpdateTasks();
        GenerateBuildTasks();
        GenerateHaulTasks();
    }
    void UpdateTasks()
    {
        buildTasks.RemoveAll(task => !task.IsValid);
        haulTasks.RemoveAll(task => !task.IsValid);
    }
    public void OnStockpileAdded(Stockpile stockpile)
    {
        stockpiles.Add(stockpile);
        haulTasks.Clear();
    }
    void GenerateHaulTasks()
    {
        piles.RemoveAll(pile => pile.IsEmpty);

        foreach(var pile  in piles)
        {
            if (pile.Item == null) 
                continue; 

            if (HasHaulTaskFor(pile))
            {
                continue;
            }

            Stockpile closest = GetClosestStockpileFor(pile.Slot, pile.TileCoords);

            if (closest == null)
                continue;

            List<Vector2Int> deliveryPath = new(pathfinder.FindPath(pile.TileCoords, closest.area.center));

            if (deliveryPath != null)
                haulTasks.Add(new HaulTask(pile, closest, deliveryPath));
        }
    }
    public Stockpile GetClosestStockpileFor(ItemSlot order, Vector2Int coords)
    {
        Stockpile target = null;
        int bestDist = int.MaxValue;
        foreach (var stockpile in stockpiles)
        {
            int capacity = stockpile.CalculateFreeSpaceFor(order);
            if (capacity == 0) 
                continue;

            int dist = (coords - stockpile.area.center).sqrMagnitude;
            if (dist < bestDist)
            {
                bestDist = dist;
                target = stockpile;
            }
        }
        return target;
    }
    public Stockpile FindClosestStockpileWith(ItemSlot order, Vector2Int to)
    {
        Stockpile target = null;
        int bestDist = int.MaxValue;
        foreach (var stockpile in stockpiles)
        {
            if (!stockpile.Has(order))
                continue;
          
            int dist = (to - stockpile.area.center).sqrMagnitude;
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
        foreach(var building in constructions )
        {
            if (!HasBuildTaskFor(building))
                Add(new BuildTask(building));
        }
    }
    bool HasBuildTaskFor(Building building)
    {
        foreach(BuildTask bT in buildTasks)
        {
            if (bT.building == building ) 
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
            buildTasks.Add(b);
        }
    }
    public ITask TakeBestTask(Vector2Int position)
    {
        
        List<Vector2Int> bestPath = new();
        int distance = int.MaxValue;

        if (buildTasks.Count > 0)
        {
            BuildTask bestTask = null;

            foreach (var bt in buildTasks)
            {
                List<Vector2Int> current = pathfinder.FindPathToArea(position, bt.building.Area);
                if (current != null)
                {
                    if (current.Count > distance)
                        continue;

                    distance = current.Count;
                    bestPath = current;
                    bestTask = bt;
                }
            }
            if (bestTask != null)
            {
                bestTask.PathToTask = bestPath;
                return bestTask;
            }
        }
        if (haulTasks.Count > 0)
        {
            HaulTask bestTask = null;

            foreach (var ht in haulTasks)
            {
                List<Vector2Int> current = pathfinder.FindPath(position, ht.Location);
                if (current != null)
                {
                    if (current.Count > distance)
                        continue;

                    distance = current.Count;
                    bestPath = current;
                    bestTask = ht;
                }
            }
            if (bestTask != null)
            {
                bestTask.PathToTask = bestPath;
                return bestTask;
            }
        }
        return null;
    }
}
