using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour {

    [SerializeField] private GameObject _ResultManager;
    [SerializeField] private GameObject Woman;
    [SerializeField] private GameObject ThunderTaerget;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            EffectManager.CreateSphereEffect(new Vector3(1, 2, 3), new Vector3(10, 20, 100), 100);
        }


        if (Input.GetKeyDown(KeyCode.W))
        {
            WomanController womanScript = Woman.GetComponent<WomanController>();
            womanScript.SetThunder(ThunderTaerget);
        }

        // エンターを押してシーン遷移
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _ResultManager.GetComponent<ResultManager>().StartResult(702500);
        }
    }
}
