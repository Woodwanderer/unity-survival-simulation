using UnityEngine;

public class AnimateActions : MonoBehaviour
{
    public SpriteRenderer objAnimated;
    
    public void SetAnimation(bool active)
    {
        objAnimated.enabled = active;
    }


}
