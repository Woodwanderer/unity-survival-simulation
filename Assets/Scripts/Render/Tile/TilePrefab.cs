using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class TilePrefab: MonoBehaviour
{
    private float size;
    //SPRITES
    public SpriteRenderer terrain;
    public SpriteRenderer elevation;
    public SpriteRenderer highlight;

    public SpriteRenderer path;

    public SpriteRenderer tileObject;
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
        tileObject.sprite = _object;

        //Local offset
        size = tileSize;
        float safetyBound = 0.15f;
        float localOffset = size / 2 - safetyBound;
        float posX = Random.Range(-localOffset, +localOffset);
        float posY = Random.Range(-localOffset, +localOffset);

        tileObject.transform.localPosition = new Vector3(posX, posY, 0);

        //Random Mirror
        if (Random.Range(0, 2) == 1) 
            tileObject.flipX = true;

        //Rotation Spread
        float rotRange = Random.Range(-10, 10);
        tileObject.transform.localRotation = Quaternion.Euler(0, 0, rotRange);

        //Scale - to expand - trees gonna grow etc. Create dependancy from TileData - TileObject: resource: capacity like wood, stone quantity i.e.:10 -> scale of 10> quasntity 3 i.e.
        float scale = Random.Range(0.6f, 1.4f);
        tileObject.transform.localScale = (Vector3.one) * scale;

    }
    public void HideObjectSprite()
    {
        tileObject.enabled = false;
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
