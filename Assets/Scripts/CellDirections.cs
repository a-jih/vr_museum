using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CellDirections {

    public const int Count = 4;

    private static Vector2Int[] dirVectors =
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    };
    private static CellDirection[] opposteDirs =
    {
        CellDirection.SOUTH,
        CellDirection.WEST,
        CellDirection.NORTH,
        CellDirection.EAST
    };
    private static Quaternion[] rotations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 90f, 0f),
        Quaternion.Euler(0f, 180f, 0f),
        Quaternion.Euler(0f, 270f, 0f)
    };

    public static CellDirection RandomValue
    {
        get { return (CellDirection)Random.Range(0, Count); }
    }

    public static Vector2Int ToVector2Int(this CellDirection direction)
    {
        return dirVectors[(int)direction];
    }

    public static CellDirection GetOpposite(this CellDirection direction)
    {
        return opposteDirs[(int)direction];
    }

    public static Quaternion ToRotation (this CellDirection direction)
    {
        return rotations[(int)direction];
    }
}
