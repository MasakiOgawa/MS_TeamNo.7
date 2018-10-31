using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEffectTest : MonoBehaviour {

    public float TargetPosY;
    public float speedY;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        this.transform.position = new Vector3(0, this.transform.position.y + speedY, 0); 

        if (this.transform.position.y > TargetPosY)
        {
            this.transform.position = new Vector3(0, 1, 0);

        }


    }
}
