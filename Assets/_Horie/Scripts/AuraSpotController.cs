using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraSpotController : MonoBehaviour {

    [SerializeField] private float RotateSpeedY;
    [SerializeField] private GameObject[] AuraPointLight;


	// Use this for initialization
	void Start ()
    { 
        // アタッチされていない場合
        for ( int n =0; n < AuraPointLight.Length; n ++)
        {
            if (AuraPointLight[n] == null)
                Debug.Log("AuraPointLight not attach : " + n);

        }

        // すべてのAuraを消灯する
        for ( int n = 0; n < AuraPointLight.Length; n ++)
        {
            //AuraPointLight[n].SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(new Vector3(0, RotateSpeedY, 0));

	}
}
