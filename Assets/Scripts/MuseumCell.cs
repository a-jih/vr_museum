using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumCell : MonoBehaviour {

    public Vector2Int gridCoordinate;
    public MuseumRoom room;

    private int initializedEdgeCount;
    private MuseumCellEdge[] edges = new MuseumCellEdge[CellDirections.Count];

    public void Initialize (MuseumRoom room)
    {
        room.Add(this);
    }

    public bool IsFullyInitialized
    {
        get { return initializedEdgeCount == CellDirections.Count; }
    }

    /*
    public bool HasWallInDirection(CellDirection direction)
    {
        for (int i = 0; i < edges.Length; i++)
        {
            if (edges[i] == null) continue;
            if (edges[i].direction == direction) return true;
        }
        return false;
    }
    */

    public MuseumCellEdge GetEdge(CellDirection direction)
    {
        return edges[(int)direction];
    }

    public void SetEdge(CellDirection direction, MuseumCellEdge edge)
    {
        edges[(int)direction] = edge;
        initializedEdgeCount++;
    }

    public CellDirection RandomUninitializedDirection
    {
        get
        {
            int skips = Random.Range(0, CellDirections.Count - initializedEdgeCount);
            for (int i = 0; i < CellDirections.Count; i++)
            {
                if (edges[i] == null)
                {
                    if (skips == 0)
                    {
                        return (CellDirection)i;
                    }
                    skips -= 1;
                }
            }
            throw new System.InvalidOperationException("MuseumCell has no unintialized directions left.");
        }
    }
}
