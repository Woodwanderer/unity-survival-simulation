using UnityEngine;
using System.Collections.Generic;
public class HaulTask : ITask
{
    public ResourcePile source;
    public Stockpile destination;
    public List<Vector2Int> deliveryPath;
    public bool IsValid => source.Amount > 0 && destination.CalculateFreeSpaceFor(source.Slot) > 0;
    public Vector2Int Location => source.TileCoords;
    public List<Vector2Int> PathToTask {  get; set; }
    public HaulTask(ResourcePile source, Stockpile destination, List<Vector2Int> deliveryPath)
    {
        this.source = source;
        this.destination = destination;
        this.deliveryPath = deliveryPath;
    }
}
