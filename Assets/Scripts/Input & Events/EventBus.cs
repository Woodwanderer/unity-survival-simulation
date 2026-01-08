using System;
using System.Numerics;
using UnityEngine;


public class EventBus
{
    //CONSOLE
    public static event Action<string> OnLogMessage;
    public static void Log(string msg) => OnLogMessage?.Invoke(msg); 

    //MOVEMENT
    public static event Action<Vector2Int> OnTileCommanded;
    public static void TileCommanded(Vector2Int mapPos) => OnTileCommanded?.Invoke(mapPos);

    //OBJECTS
    public static event Action<TileObjectView> OnObjectClick;
    public static void ObjectClick(TileObjectView objView) => OnObjectClick?.Invoke(objView);

}    


