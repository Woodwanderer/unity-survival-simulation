using UnityEngine;
public class ProtagonistView : MonoBehaviour
{
    [SerializeField] SpriteRenderer SelectionElipse;
    public void SetSelection(CharacterBrain brain)
    {
        SelectionElipse.enabled = brain.IsPlayerControlled;
    }
}
