using System.Collections.Generic;
using UnityEngine;

public class WorldObject : TileEntity
{
    public WorldObjDef Definition {get;}
    public int Age { get;}

    public HarvestSource harvestSource;
    public bool HasItems => harvestSource != null && !harvestSource.Depleted;

    public WorldObject(WorldObjDef def, Vector2Int tileCoords, int age) : base(tileCoords)
    {
        Definition = def;
        Age = age;
    }
    public IEnumerable<ItemSlot> GetItemSlots()
    {
        if (harvestSource != null)
        {
            foreach (ItemSlot slot in harvestSource.Snapshot())
            {
                if (slot.Amount > 0) 
                yield return slot;
            }
        }
    }
}



