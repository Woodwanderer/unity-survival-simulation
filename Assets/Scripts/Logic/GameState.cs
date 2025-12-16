using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class GameState
{
    World world;
    RenderWorld render;
    MapStates mapState = MapStates.None;
    ProtagonistStates protagonistState = ProtagonistStates.None;
    bool routeEstablished = false;

    public GameState(World world, RenderWorld render)
    {
        this.world = world;
        this.render = render;
    }

    public void Initialise()
    {
        EventBus.OnTileClicked += HandleTileClicked;
        EventBus.OnMovementAnimationComplete += RestoreStates;
        EventBus.OnCancel += HandleCancel;
        EventBus.OnConfirm += HandleConfirm;

    }
    void HandleTileClicked(TileData tile)
    {
        if (world.TrySelectTile(tile))
            mapState = MapStates.TileSelected;
    }

    void HandleCancel()
    {
        // Clear Highlight on Tile
        if (mapState == MapStates.TileSelected)
        {
            if (world.CancelSelection())
            {
                mapState = MapStates.None;
                return;
            }
        }
        // Clear ROUTE
        if (routeEstablished && protagonistState != ProtagonistStates.Moving)
        {
            render.DrawPath(world.protagonistData.route.pathCoords, false);
            world.CancelRoute();
            routeEstablished = false;
            mapState = MapStates.TileSelected;
            return;
        }
    }
    void HandleConfirm()
    {
        // Establish ROUTE
        if (protagonistState == ProtagonistStates.None && mapState == MapStates.TileSelected)
        {
            if (!routeEstablished)
            {
                if (world.EstablishRoute())
                {
                    render.DrawPath(world.protagonistData.route.pathCoords, true);
                    routeEstablished = true;
                }
                return;
            }
        }
        // Protagonist MOVEMENT
        if (protagonistState == ProtagonistStates.None && routeEstablished)
        {
            protagonistState = ProtagonistStates.Moving;
            world.CancelSelection();
            mapState = MapStates.None;
            render.MoveProt();
            return;
        }
    }

    //ACTION BAR
    public void AttemptHarvest()
    {
        if (protagonistState == ProtagonistStates.None)
        {
            world.Harvest();
        }
    }
    //STATES
    private enum MapStates
    {
        TileSelected,
        None,
    }
    private enum ProtagonistStates
    {
        Moving,
        None,
    }
    private void SetProtagonistIdleState()
    {
        protagonistState = ProtagonistStates.None;
    }
    private void RestoreStates()
    {
        SetProtagonistIdleState();
        routeEstablished = false;
    }
}
