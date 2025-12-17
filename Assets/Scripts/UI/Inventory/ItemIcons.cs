using UnityEngine;

[CreateAssetMenu(fileName = "ItemIcons", menuName = "Scriptable Objects/ItemIcons")]
public class ItemIcons : ScriptableObject
{
    public Item[] items;

    public Sprite GetIcon(ItemType type)
    {
        foreach(Item item in items)
        {
            if(item.type == type)
                return item.icon;
        }
        return null;
    }

    [System.Serializable]
    public class Item
    {
        public ItemType type;
        public Sprite icon;
    }
}
