using System.Collections.Generic;
using UnityEngine;

public class ContextActionBarUI : MonoBehaviour
{
    private CharacterActions characterActions;
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

    public void HarvestObject()
    {
        
    }
    public void Eat()
    {
        characterActions.EatInit(ItemType.FoodRaw); //pick from inv
    }


}
