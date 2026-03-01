using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class TilePrefab: MonoBehaviour
{
    float size;
    //SPRITES
    public SpriteRenderer terrain;
    public SpriteRenderer elevation;
    public SpriteRenderer path;
    public SpriteRenderer selection;

    //Building on Tile
    [SerializeField] public Transform buildingRoot;
    public SpriteRenderer building;
    [SerializeField] SpriteRenderer[] buildingVisuals;

    public GameObject tileObjectPrefab;
    List<TileEntityView> entities = new();

    //FUNCTIONS
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

    //Building
    public void ShowBuilding(bool visible, Sprite build)
    {
        building.sprite = build;
        building.enabled = visible;
    }
    public void SetSelected(bool active)
    {
        selection.enabled = active;
    }
    //Entities
    public void SetEntity(TileEntity ent, Sprite _object, float tileSize)
    {
        GameObject obj = Instantiate(tileObjectPrefab, this.transform);
        TileEntityView current = obj.GetComponent<TileEntityView>();
        entities.Add(current);
        current.Init(_object, tileSize, ent);
    }
    public void UpdateEntitySprite(WorldObject wo, Sprite sprite)
    {
        TileEntityView view = entities.FirstOrDefault(v =>  v.Data == wo);

        if (view == null)
            return;

        view.SetSprite(sprite);
    }
    public void RemoveEntitySprite(TileEntity ent)
    {
        TileEntityView view = entities.FirstOrDefault(v => v.Data == ent);

        if (view == null) 
            return;

        entities.Remove(view);
        Destroy(view.gameObject);
    }
    
}
