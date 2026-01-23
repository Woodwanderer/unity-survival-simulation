using System.Collections.Generic;
using UnityEngine;

public abstract class Building : TileEntity
{
    protected BuildingsData.BuildingDef def { get; }
    public BuildingType Type => def.type;

    public Area Area { get; protected set; }

    protected List<ItemSlot> materials = new();

    //Contruction
    public float constructionProgress = 0;
    public bool IsConstructed => constructionProgress >= 1f;
    public virtual float WorkTime => def.workTime * Game.Config.hourDuration;

    protected Building(Area area, BuildingsData.BuildingDef def) : base(area.center)
    {
        this.def = def;
        this.Area = area;

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
