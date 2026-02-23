using UnityEngine;
using UnityEngine.EventSystems;

public class TileEntityView : MonoBehaviour
{
    TileEntity data;
    public TileEntity Data => data;

    public SpriteRenderer sR;
    bool selected = false;
    float size;
    void Awake()
    {
        sR = GetComponent<SpriteRenderer>();
    }
    public void Init(Sprite spr, float tileSize, TileEntity ent)
    {
        data = ent;
        sR.sprite = spr;

        //Random Mirror
        if (Random.Range(0, 2) == 1)
            sR.flipX = true;

        //Rotation Spread
        float rotRange = Random.Range(-10, 10);
        sR.transform.localRotation = Quaternion.Euler(0, 0, rotRange);

        if (ent is WorldObject wo)
        {
            float agePercent = (float)wo.Age / wo.Definition.maxAge;

            float scale = Mathf.Lerp(0.5f, 2.5f, agePercent);

            float variation = Random.Range(0.95f, 1.05f);
            scale *= variation;

            sR.transform.localScale = (Vector3.one) * scale;
        }

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
    public void SetSprite(Sprite sprite)
    {
        sR.sprite = sprite;
        sR.enabled = sprite != null;
    }
    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        EventBus.ObjectClick(this);
    }
    public void SetSelected(bool value)
    {
        selected = value;
        sR.color = selected ? new Color32(248, 20, 207, 200) : Color.white;
    }
}
