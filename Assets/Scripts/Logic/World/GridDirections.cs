using UnityEngine;

public static class GridDirections
{
    public static readonly Vector2Int[] Cardinal =
    {
        new Vector2Int( 1, 0 ),
        new Vector2Int( 0,-1 ),
        new Vector2Int(-1, 0 ),
        new Vector2Int( 0, 1 ),
    };

    public static readonly Vector2Int[] CardinalAndDiagonal =
    {
        new Vector2Int( 1, 0 ),
        new Vector2Int( 0,-1 ),
        new Vector2Int(-1, 0 ),
        new Vector2Int( 0, 1 ),
        new Vector2Int( 1, 1 ),
        new Vector2Int( 1,-1 ),
        new Vector2Int(-1, 1 ),
        new Vector2Int(-1,-1 ),
    };
}