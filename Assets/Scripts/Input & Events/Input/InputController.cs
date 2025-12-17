using UnityEngine;

public class InputController: MonoBehaviour 
{
    public World world;
    private DoubleClickDetector doubleLeftClickDetector = new DoubleClickDetector(0);
    public Inventory inventory;
    bool inventoryOpen = false;

    public void Update()
    {
        // CONFIRM
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || doubleLeftClickDetector.CheckDoubleClick() )
        {
            EventBus.Confirm();
        }


        // Cancel
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            EventBus.Cancel();
        }

        //INVENTORY
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryOpen = !inventoryOpen;
            if (inventoryOpen == true)
            {
                inventory.Show();
            }
            else
                inventory.Hide();
            
        
        }

    }
}
