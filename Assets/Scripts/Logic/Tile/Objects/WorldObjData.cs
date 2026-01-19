using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldObjData", menuName = "Scriptable Objects/WorldObjData")]
public class WorldObjData : ScriptableObject
{
    //Class to easily access WorldObjDef by simple Get(TileObjectType type) through type
    public WorldObjDef[] definitions;
    Dictionary<WorldObjType, WorldObjDef> lookup;

    private void OnEnable()
    {
        lookup = new Dictionary<WorldObjType, WorldObjDef>();
        foreach (WorldObjDef def in definitions)
        {
            lookup[def.objType] = def; //populating all data from tab
        }
    }

    public WorldObjDef Get(WorldObjType type)
    {
        return lookup[type];
    }
}
