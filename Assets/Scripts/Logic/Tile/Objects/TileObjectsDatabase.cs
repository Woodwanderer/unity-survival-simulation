using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileObjectsDatabase", menuName = "Scriptable Objects/TileObjectsDatabase")]
public class TileObjectsDatabase : ScriptableObject
{
    //Class to easily access TileObjectDefinition by simple Get(TileObjectType type) through type
    public TileObjectDefinition[] definitions; //Gets all TileObjectDef togather
    Dictionary<TileObjectsType, TileObjectDefinition> lookup;

    private void OnEnable()
    {
        foreach (TileObjectDefinition def in definitions)
        {
            lookup = new Dictionary<TileObjectsType, TileObjectDefinition>();
            lookup[def.objType] = def; //populating all data from tab
        }
    }

    public TileObjectDefinition Get(TileObjectsType type)
    {
        return lookup[type];
    }

    /*public TileObjectDefinition Get(TileObjectsType type)
    {
        foreach (TileObjectDefinition def in definitions)
        {
            lookup = new Dictionary<TileObjectsType, TileObjectDefinition>();
            lookup[type] = def;
            //lookup.Add(type, def); vs podpowiedzial - apytac sie gpt czy tak moze byc tez
        }
        return lookup[type];
    }*/
}
