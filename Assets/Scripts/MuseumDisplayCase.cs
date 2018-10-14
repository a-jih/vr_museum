using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumDisplayCase : MuseumPassage {

    private Texture2D paintingTexture;

    public void LoadTexture(string imagePath)
    {
        paintingTexture = Resources.Load(imagePath) as Texture2D;
        transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = paintingTexture;
    }
}
