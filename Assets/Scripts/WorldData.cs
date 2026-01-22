using UnityEngine;

[CreateAssetMenu(fileName = "WorldData", menuName = "Scriptable Objects/WorldData")]
public class WorldData : ScriptableObject
{
    public WorldObjData objDatabase;
    public ItemsDatabase itemsDatabase;
    public BiomeData biomeData;
    public BuildingsData buildingsData;
}
