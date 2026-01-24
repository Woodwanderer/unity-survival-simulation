using UnityEngine;
using UnityEngine.UI;

public class NighttimeOverlay : MonoBehaviour
{
    [SerializeField] Image overlay;
    GameTime gameTime;

    [Range(0f, 1f)]
    public float nightStrength = 0.9f;

    Color color;
    private void Awake()
    {
        color = overlay.color;
    }

    public void Init(GameTime gameTime)
    {
        this.gameTime = gameTime;
    }
    public void Tick(float dt)
    {
        float t = gameTime.TimeOfDay * 2;
        if (gameTime.BeforeNoon)
        {
            color.a = Mathf.Lerp(nightStrength, 0f, t);
        }
        else
        {
            color.a = Mathf.Lerp(0f, nightStrength, t - 1f);
        }

        overlay.color = color;

    }
    
}
