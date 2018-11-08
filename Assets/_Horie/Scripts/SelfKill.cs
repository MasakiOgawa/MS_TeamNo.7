using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfKill : MonoBehaviour {

    [SerializeField] private int KillFrame;

    private int nFrame;


	// Use this for initialization
	void Start () {
        nFrame = 0;

	}
	
	// Update is called once per frame
	void Update () {

        if (KillFrame < nFrame)
            Destroy(this.gameObject);


	}
}
