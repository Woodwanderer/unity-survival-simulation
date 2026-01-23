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
    public Sprite GetAppearance(BuildingType type, float progress)
    {
        BuildingEntry entry = Get(type);
        if (entry == null)
            return null;

        if (progress >= 1f)
            return entry.building;

        if (entry.constructionStages == null || entry.constructionStages.Length == 0)
            return entry.construction; 
        
        int stagesCount = entry.constructionStages.Length + 1;
        int index = Mathf.FloorToInt(progress * stagesCount);

        index = Mathf.Clamp(index, 0, entry.constructionStages.Length - 1); // just an array OOrange protection

        return entry.constructionStages[index];
    }

    [System.Serializable]
    public class BuildingEntry
    {
        public BuildingType type;
        public Sprite construction;
        public Sprite building;
        public Sprite[] constructionStages;
    }
}

