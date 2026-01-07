using System.Collections;
using System.Collections.Generic;
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
        if (characterActions.currentAction is HarvestAction) 
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
        if (actionSource == null || !actionSource.GetRes().Any())
        {
            Hide();
            return;
        }

        int i = 0;
        foreach (var kv in actionSource.GetRes())
        {
            buttons[i].gameObject.SetActive(true);

            buttons[i].SetIcon(kv.Key.icon);
            buttons[i].SetAmount($"{kv.Value}");

            ItemDefinition capturedItem = kv.Key;
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
