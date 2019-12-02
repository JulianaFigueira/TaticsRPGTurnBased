using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateUnits : MonoBehaviour {

    private Unit[] units;

	// Use this for initialization
	void Start () {

        

	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void Generate()
    {
        int team = Random.Range(2, 4);
        int enemies = Random.Range(4, 8);


    }
}
