using UnityEngine;

public class BuildTask : ITask
{
    public Stockpile stockpile;
    public bool IsValid => !stockpile.IsConstructed;

    public BuildTask(Stockpile stockpile)
    {
        this.stockpile = stockpile;
    }
}
