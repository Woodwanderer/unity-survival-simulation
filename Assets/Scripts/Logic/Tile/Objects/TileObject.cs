using System.Collections.Generic;
using UnityEngine;

public class TileObject
{
    public TileObjectsType type { get; }
    public Vector2Int tileCoords { get; }

    public ItemSlot itemSlot;
    public HarvestSource harvestSource;
    public bool HasItems => itemSlot != null && !itemSlot.IsEmpty ||
        harvestSource != null && !harvestSource.Depleted;
    public TileObject(TileObjectsType type, Vector2Int tileCoords)
    {
        this.type = type;
        this.tileCoords = tileCoords;

        if (type == TileObjectsType.ResourcePile)
            itemSlot = new ItemSlot();
    }
    public IEnumerable<ItemSlot> GetItemSlots()
    {
        // Harvest source (trees, rocks)
        if (harvestSource != null)
        {
            foreach (ItemSlot itemSlot in harvestSource.Snapshot())
                yield return itemSlot;
        }

        // Resource pile (ItemSlot)
        if (itemSlot != null && !itemSlot.IsEmpty)
        {
            yield return itemSlot;
        }
    }

}



