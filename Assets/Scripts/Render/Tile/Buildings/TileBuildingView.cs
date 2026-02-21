using UnityEngine;

public class TileBuildingView : MonoBehaviour
{
    public SpriteRenderer building;
    public TileData tile;

    [SerializeField] SpriteRenderer[] slotRenderers;
    private void Awake()
    {
        foreach (var sR in slotRenderers)
        {
            sR.enabled = false;
        }
    }
    public void UpdateSlot(Stockpile.StockpileSlot slot)
    {
        if (slot.tile != tile)
            return;

        Sprite icon = slot.itemSlot.IsEmpty? null : slot.itemSlot.Item.icon;

        var sr = slotRenderers[slot.index];

        sr.sprite = icon;
        sr.enabled = icon != null;
    }
}
