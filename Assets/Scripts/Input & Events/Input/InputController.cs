using UnityEngine;

public class InputController: MonoBehaviour 
{
    public World world;
    private DoubleClickDetector doubleLeftClickDetector = new DoubleClickDetector(0);

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

        

    }
}
