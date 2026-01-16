using UnityEngine;
using System.Collections.Generic;
public class HaulTask : ITask
{
    public ResourcePile source;
    public Stockpile destination;
    public List<Vector2Int> deliveryPath;
    public bool IsValid => source != null && !source.IsEmpty;
    public HaulTask(ResourcePile source, Stockpile destination, List<Vector2Int> deliveryPath)
    {
        this.source = source;
        this.destination = destination;
        this.deliveryPath = deliveryPath;
    }
}
