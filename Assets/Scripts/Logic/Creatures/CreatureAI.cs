using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAI : MonoBehaviour
{
    private CreatureMovement movement;
    private void Start() // Inicjalizacja: Start wykona sie dopiero po EndFrame, jak wszystkie komponenty będą zainicjowane i awake itd.
    {
        movement = GetComponent<CreatureMovement>();
        StartCoroutine(BehaviourLoop());
    }
    IEnumerator BehaviourLoop()
    {
        while (true) //Endless AI loop - for now
        {
            // Losowy czas stania
            float waitTime = Random.Range(1, 6);
            yield return new WaitForSeconds(waitTime);

            // Losowy kierunek ruchu
            Vector2Int[] directions =
            {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right
            };
            
            Vector2Int step = directions[Random.Range(0, directions.Length)];
            List<Vector2Int> path = new() { step };
            


            yield return movement.MoveAlong(path);
        }
    }
}
