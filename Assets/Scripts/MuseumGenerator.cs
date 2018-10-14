using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumGenerator : MonoBehaviour {

    public Museum museumPrefab;

    private Museum museumInstance;

	void Start () {
        museumInstance = Instantiate(museumPrefab) as Museum;
        museumInstance.Generate();
	}
}
