using UnityEngine;

public class ContextActionBarUI : MonoBehaviour
{
    CharacterActions characterActions;
    TileObject actionSource = null;
    ContextABButton[] buttons;
    
    public ItemIcons icons;

    void Awake()
    {
        buttons = GetComponentsInChildren<ContextABButton>();
        Hide();
    }
    public void Init(CharacterActions actions)
    {
        this.characterActions = actions;
    }
    public void Show(TileObject obj)
    {
        if (obj == null)
            return;

        gameObject.SetActive(true);
        actionSource = obj;

        Refresh();
    }
    void Refresh()
    {
        if (actionSource == null)
            return;

        int i = 0;
        foreach (var kv in actionSource.Resources.All())
        {
            buttons[i].gameObject.SetActive(true);

            Sprite icon = icons.GetIcon(kv.Key);
            buttons[i].SetIcon(icon);

            buttons[i].SetAmount($"{kv.Value}");

            ItemType capturedItem = kv.Key;
            buttons[i].SetAction(() =>
            {
                HarvestObject(capturedItem);
            });
            i++;
        }
        while (i < buttons.Length)
        {
            buttons[i].gameObject.SetActive(false);
            i++;
        }
    }
    public void Hide()
    {
        foreach (var button in  buttons)
        {
            button.Clear();
            button.gameObject.SetActive(false);
        }
        actionSource = null;
        gameObject.SetActive(false);
    }
    public void HarvestObject(ItemType item)
    {
        characterActions.HarvestAttempt(actionSource, item);
    }
}
