using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextActionBarUI : MonoBehaviour
{
    CharacterActions characterActions;
    TileObject actionSource = null;
    public GameObject buttonPrefab;
    List<GameObject> buttonList = new List<GameObject>();
    public ItemIcons icons;

    
    public void Init(CharacterActions actions)
    {
        this.characterActions = actions;
    }
    public void GetActionSource(TileObject obj)
    {
        actionSource = obj;
        SetButtons();
    }
    void SetButtons()
    {
        if (actionSource != null)
        {
            foreach (KeyValuePair<ItemType, int> item in actionSource.Items) 
            {
                GameObject btnObj = Instantiate(buttonPrefab, transform);
                buttonList.Add(btnObj);

                ContextABButton button = btnObj.GetComponent<ContextABButton>();

                Sprite icon = icons.GetIcon(item.Key);
                button.SetIcon(icon);
                button.SetAmount($"{item.Value}");


                ItemType capturedItem = item.Key;
                button.SetAction(() =>
                {
                    HarvestObject(capturedItem);
                });

            }
        }
    }
    public void ClearButtons()
    {
        foreach (GameObject button in  buttonList)
        {
            Destroy(button);
        }
        buttonList.Clear();
    }

    public void HarvestObject(ItemType item)
    {
        characterActions.RequestHarvest(actionSource, item);
    }



}
