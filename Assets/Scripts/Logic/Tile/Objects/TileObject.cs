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

        if (type == TileObjectsType.ResourcePile)
            pile = new(null, 0);
        else
            resources = new VirtualResources();
    }
    public IEnumerable<KeyValuePair<ItemDefinition, int>> GetRes()
    {
        if (type == TileObjectsType.ResourcePile)
            return pile.Get();
        
        return resources.All();
    }
    public bool Has(ItemDefinition item)
    {
        if (type == TileObjectsType.ResourcePile)
            return pile.Has(item);

        return resources.Has(item);
    }

}



