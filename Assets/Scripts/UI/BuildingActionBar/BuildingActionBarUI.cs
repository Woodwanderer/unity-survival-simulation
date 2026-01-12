using UnityEngine;
using System.Collections.Generic;

public class BuildingActionBarUI : MonoBehaviour
{
    public GameObject buttonPrefab;
    BuildingActionBarButton build;
    Stockpile stockpile;
    CharacterActions actions;

    void Awake()
    {
        Hide();
    }

    public void Show(Stockpile stockpile, CharacterActions actions)
    {
        this.stockpile = stockpile;
        this.actions = actions;

        gameObject.SetActive(true);

        Refresh();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    void Refresh()
    {
        SetBuildButton();
    }
    void SetBuildButton()
    {
        if (!stockpile.IsConstructed && build == null)
        {
            GameObject button = Instantiate(buttonPrefab, transform);
            build = button.GetComponent<BuildingActionBarButton>();
            build.SetLowerText("Build");
            build.SetAction(() => actions.TryBuild(stockpile));
        }
        if(stockpile.IsConstructed && build != null) 
            build.Clear();
    }

}
