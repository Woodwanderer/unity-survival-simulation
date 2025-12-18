using System.Collections.Generic;
using UnityEngine;

public class TileObject
{
    public TileObjectsType type;
    bool isCollectible = true;
    public int quantity;
    Dictionary<ItemType,int> items = new Dictionary<ItemType,int>();



    public TileObject(TileObjectsType typeIn, bool collectible)
    {
        this.type = typeIn;
        this.isCollectible = collectible;
    }

    public void SetWithDictionary(TileObjectsType type_in, Dictionary<ItemType, int> startResources)
    {
        items = startResources;
        quantity = 0;
    }
}



