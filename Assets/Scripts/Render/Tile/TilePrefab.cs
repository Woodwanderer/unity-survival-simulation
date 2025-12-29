using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class TilePrefab: MonoBehaviour
{
    float size;
    //SPRITES
    public SpriteRenderer terrain;
    public SpriteRenderer elevation;
    public SpriteRenderer highlight;

    public SpriteRenderer path;

    public GameObject tileObjectPrefab;
    GameObject tileObj; //Instance
    TileObjectView TObjView;

    private Vector3 centerToBottLeft = new(-0.5f, 0f, -0.5f);

    //Referencja do danych świata
    private TileData tileData;

    //FUNCTIONS
    public void GetTileDataRef(TileData tileData)
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
    public void SetObjects(Sprite _object, float tileSize)
    {
        tileObj = Instantiate(tileObjectPrefab, this.transform);
        TObjView = tileObj.GetComponent<TileObjectView>();

        TObjView.Init(_object, tileSize, tileData.objects[0]);
    }
    public void HideObjectSprite()
    {
        TObjView.SetDepleted(); //CHECK!!! tu popraw
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
