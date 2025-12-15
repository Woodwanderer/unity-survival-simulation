using UnityEngine;

public class TileObject
{
    public TileObjectsType type;
    bool isCollectible;


    public TileObject(TileObjectsType typeIn, bool collectible = false)
    {
        this.type = typeIn;
        this.isCollectible = collectible;
    }

}


