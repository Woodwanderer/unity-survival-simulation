using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingsData", menuName = "Scriptable Objects/BuildingsData")]
public class BuildingsData : ScriptableObject
{
    public BuildingDef[] buildings;

    public BuildingDef GetDef(BuildingType type)
    {
        foreach(var building in buildings)
        {
            if (building.type == type) 
                return building;
        }

        return null;
    }

    [System.Serializable]
    public class BuildingDef
    {
        public BuildingType type;
        public float workTime;

        public List<material> materials;

        [System.Serializable]
        public struct material
        {
            public ItemDefinition item;
            public int amount;
        }
    }
}
public enum BuildingType
{
    stockpile,
    shelter
}