using UnityEngine;

public class TileObject
{
    public TileObjectsType type;
    bool isCollectible = true;
    public int quantity = 0;


    public TileObject(TileObjectsType typeIn, bool collectible)
    {
        this.type = typeIn;
        this.isCollectible = collectible;
    }

}


