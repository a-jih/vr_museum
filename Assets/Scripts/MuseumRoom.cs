using System.Collections.Generic;
using UnityEngine;

public class MuseumRoom : ScriptableObject
{

    private List<MuseumCell> cells = new List<MuseumCell>();

    public void Add(MuseumCell cell)
    {
        cell.room = this;
        cells.Add(cell);
    }

}
