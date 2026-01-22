using System.Collections.Generic;
using UnityEngine;

public abstract class Building : TileEntity
{
    protected BuildingsData.BuildingDef def { get; }
    public BuildingType Type => def.type;

    protected List<ItemSlot> materials = new();

    //Contruction
    public float constructionProgress = 0;
    public bool IsConstructed => constructionProgress >= 1f;
    public virtual float WorkTime => def.workTime * Game.Config.hourDuration;
    public abstract IEnumerable<Vector2Int> OccupiedTiles { get; }

    protected Building(Vector2Int coords, BuildingsData.BuildingDef def) : base(coords)
    {
        this.def = def;
        InitMaterials();
    }
    void InitMaterials()
    {
        foreach (var mat in def.materials)
        {
            ItemSlot material = new(mat.item, mat.amount);
            materials.Add(material);
        }
    }
}
