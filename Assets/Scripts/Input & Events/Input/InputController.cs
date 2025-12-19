using System.Xml.Serialization;
using UnityEngine;

public class InputController: MonoBehaviour 
{
    public World world;
    public RenderWorld renderWorld;
    public CameraMovement cam;
    private DoubleClickDetector doubleLeftClickDetector = new DoubleClickDetector(0);
    public Inventory inventory;
    bool inventoryOpen = false;
    
    

    //KEYBOARD KEYS Mappings
    //Camera
    KeyCode cameraCenter = KeyCode.C;
    KeyCode cameraFollowProtagonist = KeyCode.F1;

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
        if (Input.GetKeyDown(KeyCode.I)) //Items are not added to inv while it's hidden - TO FIX!!!!!!!!!!!!!
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
