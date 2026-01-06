using System.Collections.Generic;
using UnityEngine;

public class TileObject
{
    public TileObjectsType type { get; }
    public Vector2Int tileCoords { get; }
    public VirtualResources resources;
    public ResourcePile pile;
    public bool IsInit => pile != null || resources != null;

    public TileObject(TileObjectsType type, Vector2Int tileCoords)
    {
        this.type = type;
        this.tileCoords = tileCoords;
    }

}



