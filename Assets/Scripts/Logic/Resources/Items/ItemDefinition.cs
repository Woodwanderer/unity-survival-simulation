using UnityEngine;

[CreateAssetMenu(fileName = "ItemDefinition", menuName = "Scriptable Objects/ItemDefinition")]
public class ItemDefinition : ScriptableObject
{
    public string id;
    public Sprite icon;

    public float weight;
    public float nutrition;
    public int maxStockpileSize;
}
