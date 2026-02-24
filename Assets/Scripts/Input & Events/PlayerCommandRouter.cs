using UnityEngine;

public class PlayerCommandRouter
{
    CharacterBrain selectedBrain;
    public PlayerCommandRouter(CharacterBrain selectedBrain)
    {
        Bind(selectedBrain);
    }
    public void Bind(CharacterBrain newBrain)
    {
        UnBind();
        this.selectedBrain = newBrain;
        selectedBrain.SwitchPlayerControl(true);
        EventBus.OnTileCommanded += HandleMovement;
    }
    public void UnBind()
    {
        if (selectedBrain == null) 
            return;

        selectedBrain.SwitchPlayerControl(false);
        EventBus.OnTileCommanded -= HandleMovement;
        selectedBrain = null;
    }
    void HandleMovement(Vector2Int coords)
    {
        if (selectedBrain == null) 
            return;

        selectedBrain.ExecutePlayerCommand(new Movement(selectedBrain.world, coords));
    }
}
