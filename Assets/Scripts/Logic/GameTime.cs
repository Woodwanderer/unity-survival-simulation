using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class GameTime
{
    int day = 0;
    int hour = 0;
    float secondsInHour = 0;
    float hourDuration = 60; //seconds 60 - default game
    public float HourDuration => hourDuration;
    string timeString;
    float timeOfDay = 0; //0...1
  
    public void Tick(float deltaTime)
    {
        float dt = deltaTime * Game.Config.timeScale;

        timeOfDay += dt / (hourDuration * 24);
        secondsInHour += dt;

        while (secondsInHour >= hourDuration) //while -> prevents lag issue
        {
            secondsInHour -= hourDuration;
            hour++;
        }
        while (hour >= 24) 
        {
            timeOfDay -= 1;
            hour -= 24;
            day++;
        }
        timeString = $"Day: {day:D2} Time: {hour:D2}: {(int)Mathf.Floor(secondsInHour):D2}";
    }
    public float GameDeltaTime(float realDt)
    {
        return realDt * Game.Config.timeScale;
    }
    public string GetTimeString()
    {
        return timeString;
    }
    public float GetTime()
    {
        return timeOfDay;
    }
}
