using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileObjectsDatabase", menuName = "Scriptable Objects/TileObjectsDatabase")]
public class TileObjectsDatabase : ScriptableObject
{
    //Class to easily access TileObjectDefinition by simple Get(TileObjectType type) through type
    public TileObjectDefinition[] definitions;
    Dictionary<TileObjectsType, TileObjectDefinition> lookup;

    private void OnEnable()
    {
        lookup = new Dictionary<TileObjectsType, TileObjectDefinition>();
        foreach (TileObjectDefinition def in definitions)
        {
            lookup[def.objType] = def; //populating all data from tab
        }
    }

    public TileObjectDefinition Get(TileObjectsType type)
    {
        return lookup[type];
    }
}
