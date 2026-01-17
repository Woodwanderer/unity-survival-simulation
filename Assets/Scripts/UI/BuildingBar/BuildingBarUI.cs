using UnityEngine;
public class BuildingBarUI : MonoBehaviour
{
    Stockpile stockpile;
    CharacterActions actions;
    public IActionVisualData actionVisualData;

    //BuildBar
    public GameObject buttonPrefab;
    public GameObject buttonsLayout;
    BuildingBarButton build;
    BuildingBarButton inventory;

    //Inventory
    public InventoryUIGneric inventoryView;
    
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
    void Update()
    {
        Refresh();
    }
    void Refresh()
    {
        SetBuildButton();
        SetInventoryButton();
    }
    void SetBuildButton()
    {
        if (!stockpile.IsConstructed && build == null)
        {
            GameObject button = Instantiate(buttonPrefab, buttonsLayout.transform);
            build = button.GetComponent<BuildingBarButton>();
            build.SetTopText("Build");
            Sprite icon = actionVisualData.GetIcon(IActionName.Build);
            build.SetIcon(icon);
            build.SetAction(() => actions.TryBuild(stockpile));
        }

        if (!stockpile.IsConstructed && build != null)
            build.SetBottomText($"{stockpile.constructionProgress * 100:0}%");

        if (stockpile.IsConstructed && build != null)
        {
            build.DestroySelf();
            build = null;
        }
    }
    void SetInventoryButton() 
    {
        if (stockpile.IsConstructed && inventory == null)
        {
            SetInventory();

            GameObject button = Instantiate(buttonPrefab, buttonsLayout.transform);
            inventory = button.GetComponent<BuildingBarButton>();
            inventory.SetTopText("Inventory");
            Sprite icon = actionVisualData.GetIcon(IActionName.Collect);
            inventory.SetIcon(icon);
            inventory.SetAction(() => ToggleInventory());
        }

        if (!stockpile.IsConstructed && inventory != null)
        {
            inventory.DestroySelf();
            inventory = null;
        }
    }
    void SetInventory()
    {
        if (inventoryView.IsInit)
            return;

        inventoryView.Init(stockpile);
    }
    void ToggleInventory()
    {
        inventoryView.Show();
    }
}
