using Unity.VisualScripting;
using UnityEngine;

public class DoubleClickDetector
{
    float lastClickTime;
    private readonly float doubleClickTime = 0.25f;
    private readonly int mouseInput; 

    public DoubleClickDetector(int mouseInput)
    {
        this.mouseInput = mouseInput;
    }

    public bool CheckDoubleClick()
    {
        if (Input.GetMouseButtonDown(mouseInput))
        {
            if (Time.time - lastClickTime < doubleClickTime)
            {
                return true;
            }
            lastClickTime = Time.time;
        }
        return false;
    }
}
