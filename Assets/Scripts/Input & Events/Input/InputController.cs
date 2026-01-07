using System.Xml.Serialization;
using UnityEngine;

public class InputController: MonoBehaviour 
{
    public RenderWorld renderWorld;
    public CameraMovement cam;
    
    private DoubleClickDetector doubleLeftClickDetector = new DoubleClickDetector(0);

    public InventoryUI inventory;
    bool inventoryOpen = false;
    

    //KEYBOARD KEYS Mappings
    //UI
    KeyCode toggleInventory = KeyCode.I;

    //Camera
    KeyCode cameraCenter = KeyCode.C;
    KeyCode cameraFollowProtagonist = KeyCode.F1;

    bool confirmPressed;
    bool cancelPressed;
    

    public void Tick(float deltaTime)
    {
        // Movement
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || doubleLeftClickDetector.CheckDoubleClick()) 
        {
            confirmPressed = true;
        }
        if(Input.GetMouseButtonDown(1))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            Vector2Int mapPos = renderWorld.WorldToMap(worldPos);

            EventBus.TileCommanded(mapPos);
        }
        // Cancel
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            cancelPressed = true;
        }

        //INVENTORY
        if (Input.GetKeyDown(toggleInventory)) // I
        {
            inventoryOpen = !inventoryOpen;
            if (inventoryOpen == true)
            {
                inventory.Show();
            }
            else
                inventory.Hide();
        }
        //CAMERA
        CameraPointAtProtagonist(); // C
        CameraFollowPlayer();       // F1
    }
    public bool ConsumeConfirm()
    {
        if (!confirmPressed)
            return false;
        confirmPressed = false;
        return true;
    }
    public bool ConsumeCancel()
    {
        if (!cancelPressed)
            return false;
        cancelPressed = false;
        return true;
    }
    private void CameraPointAtProtagonist()
    {
        if (Input.GetKeyDown(cameraCenter)) //Point at Protagonist
        {
            Vector3 pos = renderWorld.GetProtagonistLocation();
            Vector2 target = new(pos.x, pos.y);
            cam.PointTo(target);
        }
    }
    private void CameraFollowPlayer()
    {
        if (Input.GetKeyDown(cameraFollowProtagonist))
        {
            cam.StartFollow(renderWorld.GetProtagonistTransform());
        }
    }
}
