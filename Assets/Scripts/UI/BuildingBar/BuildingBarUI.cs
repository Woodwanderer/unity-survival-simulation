using UnityEngine;
public class BuildingBarUI : MonoBehaviour
{
    Stockpile stockpile;
    bool lastIsConstructed;
    CharacterActions actions;
    public IActionVisualData actionVisualData;

    //BuildBar
    public GameObject buttonPrefab;
    public GameObject buttonsLayout;
    BuildingBarButton buildButt;
    BuildingBarButton invButt;

    //Inventory
    public InventoryUIGeneric inventoryView;
    
    void Awake()
    {
        Hide();
    }
    public void Show(Stockpile stockpile, CharacterActions actions)
    {
        bool changed = this.stockpile != stockpile;

        this.stockpile = stockpile;
        this.actions = actions;

        gameObject.SetActive(true);

        if (changed)
        {
            inventoryView.Hide();
            ClearButtons();
            lastIsConstructed = stockpile.IsConstructed;
            BuildButtonsForState();
        }
    }
    public void Hide()
    {
        inventoryView.Hide();
        gameObject.SetActive(false);
    }
    void Update()
    {
        if (stockpile == null)
            return;

        if (stockpile.IsConstructed != lastIsConstructed)
        {
            lastIsConstructed = stockpile.IsConstructed;
            BuildButtonsForState();
        }
        if (!stockpile.IsConstructed && buildButt != null)
            buildButt.SetBottomText($"{stockpile.constructionProgress * 100:0}%");
    }
    void BuildButtonsForState()
    {
        if (!stockpile.IsConstructed)
        {
            CreateBuildButton();
            ClearInvButton();
        }
        else
        {
            ClearBuildButton();
            CreateInventoryButton();
        }
    }
    void ClearButtons()
    {
        ClearBuildButton();
        ClearInvButton();
    }
    void ClearBuildButton()
    {
        if (buildButt == null)
            return;

        buildButt.DestroySelf();
        buildButt = null;
    }
    void ClearInvButton()
    {
        if (invButt == null)
            return;

        invButt.DestroySelf();
        invButt = null;
    }
    void CreateBuildButton()
    {
        GameObject button = Instantiate(buttonPrefab, buttonsLayout.transform);
        buildButt = button.GetComponent<BuildingBarButton>();
        buildButt.SetTopText("Build");
        Sprite icon = actionVisualData.GetIcon(IActionName.Build);
        buildButt.SetIcon(icon);
        buildButt.SetAction(() => actions.TryBuild(stockpile));
    }
    void CreateInventoryButton() 
    {
        GameObject button = Instantiate(buttonPrefab, buttonsLayout.transform);
        invButt = button.GetComponent<BuildingBarButton>();
        invButt.SetTopText("Inventory");
        Sprite icon = actionVisualData.GetIcon(IActionName.Collect);
        invButt.SetIcon(icon);
        invButt.SetAction(() => ToggleInventory());
    } 
    void ToggleInventory()
    {
        inventoryView.Toggle(stockpile);
    }
}
