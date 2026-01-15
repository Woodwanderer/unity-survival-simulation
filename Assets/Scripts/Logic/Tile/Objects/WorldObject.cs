using System.Collections.Generic;
using UnityEngine;

public class WorldObject : TileEntity
{
    public TileObjectDefinition Definition {get;}
    public HarvestSource harvestSource;
    public bool HasItems => harvestSource != null && !harvestSource.Depleted;
    public WorldObject(TileObjectDefinition def, Vector2Int tileCoords) : base(tileCoords)
    {
        Definition = def;
    }
    public IEnumerable<ItemSlot> GetItemSlots()
    {
        if (harvestSource != null)
        {
            foreach (ItemSlot slot in harvestSource.Snapshot())
                yield return slot;
        }
    }
}



