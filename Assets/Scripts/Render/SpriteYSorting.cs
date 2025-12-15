using UnityEngine;

public class SpriteYSorting : MonoBehaviour
{
    private SpriteRenderer sR;
    private void Awake()
    {
        sR = GetComponent<SpriteRenderer>();
    }
    private void LateUpdate()
    {
        sR.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}
