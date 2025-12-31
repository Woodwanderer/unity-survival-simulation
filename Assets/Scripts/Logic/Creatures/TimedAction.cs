using UnityEngine;

public class TimedAction
{
    float hourDuration;
    float progress = 0;
    float valuePerHour;
    float targetValue;
    float speed;

    public TimedAction(float hourDuration, float valuePerHour, float targetValue)
    {
        this.hourDuration = hourDuration;
        this.valuePerHour = valuePerHour;
        this.targetValue = targetValue;
    }
    public void Tick(float deltaTime)
    {
        TimedActionStep(deltaTime, speed);
    }

    public float TAction(float deltaTime)
    {
        speed = valuePerHour / hourDuration;
        if (progress <= targetValue)
            return TimedActionStep(deltaTime, speed);
        return progress;
    }
    public float TimedActionStep(float deltaTime, float speed)
    {
        return speed * deltaTime; 
    }
}
