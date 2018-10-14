using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : MonoBehaviour {

    public MuseumCell cell;
    public CellDirection direction;

    private Texture2D paintingTexture;

    public void Initialize(MuseumCell cell, CellDirection direction)
    {
        this.cell = cell;
        this.direction = direction;
        transform.parent = cell.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = direction.ToRotation();
    }

    public void LoadTexture(string imagePath)
    {
        paintingTexture = Resources.Load(imagePath) as Texture2D;
        transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = paintingTexture;
    }
}
