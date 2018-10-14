using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MuseumCellEdge : MonoBehaviour {

    public MuseumCell cell, otherCell;

    public CellDirection direction;

    public void Initialize (MuseumCell cell, MuseumCell otherCell, CellDirection direction)
    {
        this.cell = cell;
        this.otherCell = otherCell;
        this.direction = direction;
        cell.SetEdge(direction, this);
        transform.parent = cell.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = direction.ToRotation();
    }
}
