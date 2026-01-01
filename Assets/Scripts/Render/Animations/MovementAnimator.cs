using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class MovementAnimator : MonoBehaviour
{
    public IEnumerator MoveOneTile(Vector2 direction, float tileSize, float duration)
    {
        Vector3 startLoc = transform.position;
        Vector3 endLoc = startLoc + new Vector3(direction.x, direction.y, 0) * tileSize;

        float t = 0f;

        while (t < 1f) 
        {
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(startLoc, endLoc, t);
            yield return null;
        }
        transform.position = endLoc;
    }

}
