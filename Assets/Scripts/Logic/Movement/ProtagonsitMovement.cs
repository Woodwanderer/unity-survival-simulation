using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ProtagonsitMovement : MonoBehaviour
{
    private ProtagonistData data;
    private MovementAnimator animator;
    private RenderWorld render;
    private World world;
    public void Initialise(ProtagonistData dataIn, RenderWorld renderIn)
    {
        this.data = dataIn;
        render = renderIn;
        this.world = render.world;
        animator = GetComponent<MovementAnimator>();
    }
    public IEnumerator MoveAlong()
    {
        List<Vector2Int> path = data.pathSteps;
        Vector2Int currentCoords = data.mapCoords;
        Vector2Int targetCoords = currentCoords;

        foreach (Vector2Int step in path)
        {
            targetCoords += step;

            render.TilePath(targetCoords, false);

            float duration = render.tileSize / data.speed; 

            yield return animator.MoveOneTile(step, render.tileSize, duration);
            currentCoords = targetCoords;
            data.MoveByStep(step);   
        }
        EventBus.MovementAnimationComplete();
    }
}
