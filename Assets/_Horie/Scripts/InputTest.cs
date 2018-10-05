using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour {

    [SerializeField] private GameObject _ResultManager;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        // エンターを押してシーン遷移
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _ResultManager.GetComponent<ResultManager>().StartResult(702500);
        }
    }
}
