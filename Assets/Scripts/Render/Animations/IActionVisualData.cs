using UnityEngine;

[CreateAssetMenu(fileName = "IActionVisual", menuName = "Scriptable Objects/IActionVisual")]
public class IActionVisualData : ScriptableObject
{
    public IActionVisual[] actions;

    public Sprite GetIcon(IActionName name)
    {
        foreach (var action in actions)
        {
            if (action.name == name)
                return action.icon;
        }
        return null;
    }
    [System.Serializable]
    public class IActionVisual
    {
        public IActionName name;
        public Sprite icon;
    }
}
public enum IActionName
{
    Eat,
    Harvest,
    Collect,
    Build
}
