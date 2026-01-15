using UnityEngine;

public class ContextActionBarUI : MonoBehaviour
{
    CharacterActions characterActions;
    TileEntity actionSource = null;
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
    public void Show(TileEntity ent)
    {
        if (ent == null)
            return;

        gameObject.SetActive(true);
        actionSource = ent;

        Refresh();
    }
    void Refresh()
    {
        if (actionSource is ResourcePile rp)
        {
            if (rp == null)
            {
                Hide();
                return;
            }

            buttons[0].gameObject.SetActive(true);

            buttons[0].SetIcon(rp.Item.icon);
            buttons[0].SetAmount($"{rp.Amount}");

            buttons[0].SetAction(() =>
            {
                HarvestObject(rp.Item);
            });

            int i = 1;
            while (i < buttons.Length)
            {
                buttons[i].gameObject.SetActive(false);
                i++;
            }
        }
        else if (actionSource is WorldObject wo)
        {
            if (wo == null) 
            {
                Hide();
                return;
            }
            int i = 0;
            foreach (ItemSlot slot in wo.GetItemSlots())
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
