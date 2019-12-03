using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaticsManager : MonoBehaviour {

    public CreateMapScript MapScript;
    public CreateUnitsScript UnitsScript;


	// Use this for initialization
	void Start () {
        MapScript.Generate();
        UnitsScript.Generate();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
