using UnityEngine;

public class DropItem : IAction
{
    ItemSlot source;
    World world;
    RenderWorld render;
    ResourcePile resPile;

    public float progress;
    float unitProgress;
    float speed;
    int targetAmount;
    public bool IsFinished => progress >= 1f;
    public DropItem(ItemSlot source, CharacterSheet stats, World world, RenderWorld render)
    {
        this.source = source;
        this.speed = stats.harvestSpeed;
        this.world = world;
        this.render = render;
    }
    public ActionStatus Status { get; private set; } = ActionStatus.NotStarted;
    public void Start()
    {
        targetAmount = source.Amount;
    }
    public void Tick(float dt)
    {
        unitProgress += dt * speed;
        progress += dt * speed / targetAmount;

        while (unitProgress >= 1f)
        {
            unitProgress -= 1;
            source.Remove(1);

            if (resPile == null)
                EstablishPile(1);
            else
                resPile.Add(source.Item, 1);
        }
    }
    ResourcePile EstablishPile(int amount)
    {
        TileData tile = world.GetProtagonistTileData();
        ResourcePile pile = tile.FindInPiles(source.Item);
        if (pile != null)
        {
            pile.Add(source.Item, amount);
            resPile = pile;
        }
        else
        {
            resPile = world.CreateResourcePile(tile, source.Item, amount);
            render.SpawnResourcePile(resPile);
        }
        return resPile;
    }
    public void Cancel()
    {

    }
}
