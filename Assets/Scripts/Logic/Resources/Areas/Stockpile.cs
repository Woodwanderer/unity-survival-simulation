using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stockpile : IItemContainer
{
    World world;
    public Area area;
    public List<TileData> tiles = new();
    class StockpileSlot
    {
        public TileData tile;
        public int index;

        public ItemSlot itemSlot = new();

        public const int slotsInTile = 4;
        public StockpileSlot(TileData tile, int index)
        {
            Assert.IsTrue(index is >= 0 and < slotsInTile, $"Stockpile.Slot index must be 0..{slotsInTile - 1}");

            this.tile = tile;
            this.index = index;
        }
    }
    List<StockpileSlot> slots = new();
    public IEnumerable<ItemSlot> Slots => slots.Select(s => s.itemSlot);
    public VirtualResources Snapshot() => new VirtualResources(Slots);
    public int Capacity => slots.Count;
    public int CalculateFreeSpaceFor(ItemSlot income)
    {
        if (income == null || income.IsEmpty) 
            return 0;

        int freeSpace = 0;
        foreach (var slot in Slots)
        {
            freeSpace += slot.FreeSpaceFor(income.Item);
            if (freeSpace > income.Amount)
                return income.Amount;
        }
        return freeSpace;
    }
    
    //construction
    public float WorkTime => 6 * world.gameTime.HourDuration * area.Count; //work units: 6hours/tile
    public float constructionProgress = 0;
    public bool IsConstructed => constructionProgress >= 1f;

    public Stockpile(Area area, World world)
    {
        this.area = area;
        this.world = world;
        SetBuildingOnTiles();
        SetSlots();
    }
    void SetBuildingOnTiles()
    {
        foreach (var tile in area.tiles)
        {
            TileData current = world.GetTileData(tile);
            tiles.Add(current);
            current.SetBuilding(this);
        }
    }
    void SetSlots()
    {
        foreach (TileData tile in tiles)
        {
            for (int i = 0; i < StockpileSlot.slotsInTile; i++)
            {
                slots.Add(new StockpileSlot(tile, i));
            }
        }
    }
    public int Add(ItemDefinition item, int amount)
    {
        int remaining = amount;

        foreach (ItemSlot slot in Slots)
        {
            if (!slot.IsEmpty && slot.Item == item && !slot.IsFull)
            {
                remaining = slot.Add(item, remaining);
                if (remaining <= 0)
                    return 0;
            }
        }
        foreach (ItemSlot slot in Slots)
        {
            if (slot.IsEmpty)
            {
                remaining = slot.Add(item, remaining);
                if (remaining <= 0)
                    return 0;
            }
        }
        return remaining; // overflow
    }
    public int Remove(ItemDefinition item, int amount)
    {
        int remaining = amount;
        foreach (ItemSlot slot in Slots)
        {
            if (slot.Item != item || slot.IsEmpty)
                continue;

            remaining = slot.Remove(remaining);

            if (remaining <= 0)
                return 0;
        }
        return remaining;
    }
}
