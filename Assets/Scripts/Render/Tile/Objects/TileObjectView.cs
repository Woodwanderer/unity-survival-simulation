using UnityEngine;
using UnityEngine.EventSystems;

public class TileObjectView : MonoBehaviour
{
    TileObject data;
    SpriteRenderer sR;
    bool selected = false;
    float size;
    void Awake()
    {
        sR = GetComponent<SpriteRenderer>(); //Nie trzeba podpinac z Unity, sR jest private
    }
    public void Init(Sprite spr, float tileSize, TileObject objData)
    {
        data = objData;
        sR.sprite = spr;

        //Random Mirror
        if (Random.Range(0, 2) == 1)
            sR.flipX = true;

        //Rotation Spread
        float rotRange = Random.Range(-10, 10);
        sR.transform.localRotation = Quaternion.Euler(0, 0, rotRange);

        //Scale - to expand - trees gonna grow etc. Create dependancy from TileData - TileObject: resource: capacity like wood, stone quantity i.e.:10 -> scale of 10> quasntity 3 i.e.
        float scale = Random.Range(0.6f, 1.4f);
        sR.transform.localScale = (Vector3.one) * scale;

        //Local offset
        size = tileSize;
        float safetyBound = 0.15f;
        float localOffset = size / 2 - safetyBound;
        float posX = Random.Range(-localOffset, +localOffset);
        float posY = Random.Range(-localOffset, +localOffset);

        transform.localPosition = new Vector3(posX, posY, 0);

        var col = GetComponent<PolygonCollider2D>();
        if (col != null)
        {
            Destroy(col);
            col = gameObject.AddComponent<PolygonCollider2D>();
            col.isTrigger = true;
        }
    }
    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        SetSelected(selected = !selected);
        EventBus.ObjectClick(data);
    }
    public void SetSelected(bool selected)
    {
        sR.color = selected ? Color.yellow : Color.white;
    }

}
