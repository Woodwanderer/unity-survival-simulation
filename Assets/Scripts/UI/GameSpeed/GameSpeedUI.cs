using UnityEngine;

public class GameSpeedUI : MonoBehaviour
{
    GameTime gTime;
    public void Init(GameTime gTime)
    {
        this.gTime = gTime;
    }
    public void SetNormalTimeScale()
    {
        gTime.inGametimeScale = 1f;
        EventBus.Log("Normal timescale.");
    }
    public void SetTimeScaleX2()
    {
        gTime.inGametimeScale = 2f;
        EventBus.Log("Time x2 set.");
    }
    public void SetTimeScaleX4()
    {
        gTime.inGametimeScale = 4f;
        EventBus.Log("Time x4 set.");
    }
    public void SetTimeScaleX8()
    {
        gTime.inGametimeScale = 8f;
        EventBus.Log("Time x8 set.");
    }
    public void SetTimeScaleX16()
    {
        gTime.inGametimeScale = 16f;
        EventBus.Log("Time x16 set.");
    }
    public void SetTimeScaleX24()
    {
        gTime.inGametimeScale = 24f;
        EventBus.Log("Time x32 set.");
    }
}
