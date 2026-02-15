using UnityEngine;
using System.Collections.Generic;

public interface ITask
{
    bool IsValid { get; }
    Vector2Int Location { get; }
    List<Vector2Int> PathToTask { get; set; }
}
