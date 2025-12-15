using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class EventBus // to tylko statyczna klasa, jak biblioteka na eventy i funkcje obslugujace
{
    //CONSOLE
    public static event Action<string> OnLogMessage;
    public static void Log(string msg) => OnLogMessage?.Invoke(msg); 


    //MOVEMENT
    public static event Action OnMovementAnimationComplete;    
    public static void MovementAnimationComplete() => OnMovementAnimationComplete?.Invoke();



    //TILE CLICK
    public static event Action<TileData> OnTileClicked;
    public static event Action<TileData, TileData> OnTileHighlight;

    public static void TileHighlight(TileData previousTile, TileData currentTile) => OnTileHighlight?.Invoke(previousTile, currentTile);
    public static void TileClicked(TileData tile) => OnTileClicked?.Invoke(tile);


    
    //GAME STATE NAVIGATION (CONTEXT ACTIONS)
    public static event Action OnCancel;
    public static event Action OnConfirm;

    public static void Cancel() => OnCancel?.Invoke();
    public static void Confirm() => OnConfirm?.Invoke();
}    


