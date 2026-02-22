using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldObject : TileEntity
{
    public WorldObjDef Definition {get;}
    public int Age { get;}

    public HarvestSource harvestSource;
    public event Action OnStateChanged;
    public bool HasItems => harvestSource != null && !harvestSource.Depleted;

    public WorldObject(WorldObjDef def, Vector2Int tileCoords, int age) : base(tileCoords)
    {
        Definition = def;
        Age = age;
    }
    public void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
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



