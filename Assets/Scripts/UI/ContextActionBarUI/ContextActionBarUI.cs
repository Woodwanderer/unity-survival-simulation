using System.Linq;
using UnityEngine;

public class ContextActionBarUI : MonoBehaviour
{
    CharacterActions characterActions;
    TileObject actionSource = null;
    ContextABButton[] buttons;

    void Awake()
    {
        buttons = GetComponentsInChildren<ContextABButton>();
        Hide();
    }
    public void Init(CharacterActions actions)
    {
        this.characterActions = actions;
    }
    private void Update()
    {
        if (characterActions.currentAction is HarvestAction || characterActions.currentAction is CollectItem) 
            Refresh();
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
        //Resources    
        if (actionSource == null || !actionSource.HasItems)
        {
            Hide();
            return;
        }

        int i = 0;
        foreach (ItemSlot slot in actionSource.GetItemSlots())
        {
            buttons[i].gameObject.SetActive(true);


            ItemDefinition capturedItem = slot.Item;

            buttons[i].SetIcon(capturedItem.icon);
            buttons[i].SetAmount($"{slot.Amount}");

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
    public void HarvestObject(ItemDefinition item)
    {
        characterActions.TryHarvest(actionSource, item);
    }
}
