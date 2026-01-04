using System;
using UnityEngine;


public class EventBus
{
    //CONSOLE
    public static event Action<string> OnLogMessage;
    public static void Log(string msg) => OnLogMessage?.Invoke(msg); 


    //MOVEMENT
    public static event Action<Vector2Int> OnTileCommanded;
    public static void TileCommanded(Vector2Int mapPos) => OnTileCommanded?.Invoke(mapPos);
    


    //TILE CLICK
    public static event Action<TileData> OnTileClicked;
    public static event Action<TileData, TileData> OnTileHighlight;

    public static void TileHighlight(TileData previousTile, TileData currentTile) => OnTileHighlight?.Invoke(previousTile, currentTile);
    public static void TileClicked(TileData tile) => OnTileClicked?.Invoke(tile);


    //OBJECTS
    public static event Action<TileObjectView> OnObjectClick;

    public static void ObjectClick(TileObjectView objView) => OnObjectClick?.Invoke(objView);

}    


