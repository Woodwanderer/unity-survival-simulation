using System.Collections.Generic;
using UnityEngine;
public class TilePrefab: MonoBehaviour
{
    float size;
    //SPRITES
    public SpriteRenderer terrain;
    public SpriteRenderer elevation;
    public SpriteRenderer path;
    public SpriteRenderer selection;

    public SpriteRenderer building;

    public GameObject tileObjectPrefab;
    List<TileObjectView> objects = new();

    //Referencja do danych świata
    private TileData tileData;

    //FUNCTIONS
    public void SetTileDataRef(TileData tileData)
    {
        this.tileData = tileData;
    }

    public void SetTerrain(Sprite _terrain)
    {
        terrain.sprite = _terrain;
    }
    public void SetElevation(Sprite _elevation)
    {
        elevation.sprite = _elevation;
    }

    public void ShowPath(bool visible)
    {
        path.enabled = visible;
    }
    public void ShowBuilding(bool visible, Sprite build)
    {
        building.sprite = build;
        building.enabled = visible;
    }
    public void SetSelected(bool active)
    {
        selection.enabled = active;
    }
    //Objects
    public void SetEntity(TileEntity ent, Sprite _object, float tileSize)
    {
        GameObject obj = Instantiate(tileObjectPrefab, this.transform);
        TileObjectView current = obj.GetComponent<TileObjectView>();
        objects.Add(current);
        current.Init(_object, tileSize, ent);
    }
    public void HideEntitySprite(TileEntity ent)
    {
        foreach (TileObjectView e in objects) 
        {
            if (e.Data == ent)
                e.SetDepleted();
        }
    }
    
}
