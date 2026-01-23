using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable Objects/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("World")]
    public int worldSizeX = 60;
    public int worldSizeY = 60;

    [Header("Time")]
    public float timeScale = 1f;
    public float hourDuration = 60f; //seconds

}
public static class Game
{
    public static GameConfig Config;
}
