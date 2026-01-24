using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimePanelUI : MonoBehaviour
{
    public TMP_Text time;
    public GameTime gameTime;
    public Image timeBar;

    public void Init(GameTime gameTime_in)
    {
        gameTime = gameTime_in;
    }
    private void Update()
    {
        time.text = gameTime.GetTimeString();
        timeBar.fillAmount = gameTime.TimeOfDay;
    }
}
