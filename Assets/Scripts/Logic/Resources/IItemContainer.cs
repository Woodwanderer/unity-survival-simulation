using System.Collections.Generic;
public interface IItemContainer
{
    IEnumerable<ItemSlot> Slots { get; }

    int Add(ItemDefinition item, int amount);
    int Remove(ItemDefinition item, int amount);

    VirtualResources Snapshot();
    int Capacity { get; }
}
