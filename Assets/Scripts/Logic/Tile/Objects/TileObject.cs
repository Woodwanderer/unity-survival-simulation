using System.Collections.Generic;
using UnityEngine;

public class TileObject
{
    public TileObjectsType type { get; }
    public Vector2Int tileCoords { get; }
    public VirtualResources Resources { get; }

    public TileObject(TileObjectsType type_in, Dictionary<ItemDefinition, int> startResources, Vector2Int tileCoords )
    {
        type = type_in;
        this.tileCoords = tileCoords;
        Resources = new VirtualResources();
        foreach(var kv  in startResources)
            Resources.Add( kv.Key, kv.Value );
    }

}



