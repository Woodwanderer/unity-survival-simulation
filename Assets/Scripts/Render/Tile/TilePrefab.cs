using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TilePrefab: MonoBehaviour
{
    float size;
    //SPRITES
    public SpriteRenderer terrain;
    public SpriteRenderer elevation;
    public SpriteRenderer highlight;

    public SpriteRenderer path;

    public GameObject tileObjectPrefab;
    List<TileObjectView> objects = new(); //Instance

    private Vector3 centerToBottLeft = new(-0.5f, 0f, -0.5f);

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
    public void SetObject(TileObject objData, Sprite _object, float tileSize)
    {
        GameObject obj = Instantiate(tileObjectPrefab, this.transform);
        TileObjectView current = obj.GetComponent<TileObjectView>();
        objects.Add(current);
        current.Init(_object, tileSize, objData);
    }
    public void HideObjectSprite(TileObject obj)
    {
        foreach (TileObjectView o in objects) 
        {
            if (o.Data == obj)
                o.SetDepleted();
        }
    }
    public void ShowPath(bool visible)
    {
        path.enabled = visible;
    }

    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        EventBus.TileClicked(tileData);
    }
    

}
