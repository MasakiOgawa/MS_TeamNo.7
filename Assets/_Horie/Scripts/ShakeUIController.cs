using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeUIController : MonoBehaviour {

    [SerializeField] private GameObject ShakeUI1;
    [SerializeField] private GameObject ShakeUI2;

    [SerializeField] private int ChangeFrame;



    private int nFrame;

	// Use this for initialization
	void Start () {
        nFrame = 0;

        ShakeUI1.SetActive(true);
        ShakeUI2.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if ( nFrame > ChangeFrame)
        {
            // 1が有効な場合
            if ( ShakeUI1.activeSelf )
            {
                ShakeUI1.SetActive(false);
                ShakeUI2.SetActive(true);
                nFrame = 0;
                return;
            }
            // 2が有効な場合
            else if (ShakeUI2.activeSelf)
            {
                ShakeUI1.SetActive(true);
                ShakeUI2.SetActive(false);
                nFrame = 0;
                return;
            }
        }


        nFrame++;
	}
}
