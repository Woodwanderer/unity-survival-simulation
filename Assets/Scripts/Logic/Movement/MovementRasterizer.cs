using NUnit.Framework;
using System.Collections.Generic; // for List<>
using UnityEngine;

public class MovementRasterizer
{  
    
    public List<Vector2Int> RasterizeMovement(Vector2Int deltaPosition)
    {
        List<Vector2Int> steps = new();

        int x = deltaPosition.x;
        int y = deltaPosition.y;

        while (x != 0 || y != 0) 
        {
            steps.Add(CalculateNextMove(ref x, ref y));
        }
        return steps;
    }

    private Vector2Int CalculateNextMove(ref int x, ref int y)
    {

        int xAbs = Mathf.Abs(x);
        int yAbs = Mathf.Abs(y);

        Vector2Int step;

        if (xAbs == yAbs)
        {
            bool horizontal = Random.Range(0, 2) == 0;
            if (horizontal)
            {
                step = new Vector2Int((int)Mathf.Sign(x), 0); // Sign Zwraca wartość -1f,0f,1f, więc dostajemy jednostkowy wektor
            }
            else
            {
                step = new Vector2Int(0, (int)Mathf.Sign(y));
            }
        }
        else if (xAbs > yAbs)
        {
            step = new Vector2Int((int)Mathf.Sign(x), 0);
        }
        else
        {
            step = new Vector2Int(0, (int)Mathf.Sign(y));
        }

        x -= step.x;
        y -= step.y;

        return step;
    }
}
