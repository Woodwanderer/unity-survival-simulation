using UnityEngine;

[CreateAssetMenu(fileName = "BuildingAppearance", menuName = "Scriptable Objects/BuildingAppearance")]
public class BuildingAppearance : ScriptableObject
{
    public BuildingEntry[] buildings;

    public BuildingEntry Get(BuildingType type)
    {
        foreach(BuildingEntry building in buildings)
        {
            if(building.type == type)
                return building;
        }
        return null;
    }

    [System.Serializable]
    public class BuildingEntry
    {
        public BuildingType type;
        public Sprite construction;
        public Sprite building;
    }
}

public enum BuildingType
{
    stockpile,
}
