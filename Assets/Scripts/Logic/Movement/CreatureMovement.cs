using System.Collections; //IEnumerator
using System.Collections.Generic; //List<>
using UnityEngine;

public class CreatureMovement : MonoBehaviour
{
    private CreatureData data;
    private MovementAnimator animator;
    private RenderWorld render;
    private World world;
    public void Initialise(CreatureData dataIn, RenderWorld renderIn)
    {
        this.data = dataIn;
        render = renderIn;
        this.world = render.world;
        animator = GetComponent<MovementAnimator>();
    }

    public IEnumerator MoveAlong(List<Vector2Int> path)
    {
        Vector2Int currentCoords = data.mapCoords;
        Vector2Int targetCoords = currentCoords;

        foreach (Vector2Int step in path)
        {
            targetCoords += step;

            // Contain in World Boundaries
            if (targetCoords.x >= world.worldSizeX || targetCoords.y >= world.worldSizeY || targetCoords.x < 0 || targetCoords.y < 0)
            {
                targetCoords = currentCoords;
                continue; // przeskakuje do nastepnej iteracji foreach. yield break; przerywa coroutine
            }

            float duration = render.tileSize / data.speed; //Gives constant time, from constant move speed
            yield return animator.MoveOneTile(step, render.tileSize, duration);
            currentCoords = targetCoords;
            data.MoveByStep(step);
        }

        
    }
}
