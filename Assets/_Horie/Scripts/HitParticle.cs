using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticle : MonoBehaviour {

    private int DeleteFrame;
    private int FrameCounter;
	// Use this for initialization
	void Start () {
        FrameCounter = 0;
        DeleteFrame = 20;
    }
	
	// Update is called once per frame
	void Update () {
        FrameCounter++;

        if (FrameCounter > DeleteFrame)
            Destroy(this.gameObject);
	}
}
