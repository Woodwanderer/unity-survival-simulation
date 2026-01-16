using UnityEngine;

public abstract class TileEntity
{
    public Vector2Int TileCoords { get; protected set; }
    public bool isValid = true;

    protected TileEntity(Vector2Int tileCoords)
    {
        this.TileCoords = tileCoords;
    }

}
