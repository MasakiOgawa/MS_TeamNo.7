using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeUIScale : MonoBehaviour {

    [SerializeField] private RectTransform rect;
    [SerializeField] private float DeltaScale;

    private float originalScale;

	// Use this for initialization
	void Start () {



        originalScale = rect.transform.localScale.x;

        rect.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

    }
	
	// Update is called once per frame
	void Update () {
		
        if ( rect.transform.localScale.x < originalScale)
        {
            rect.transform.localScale = new Vector3(rect.transform.localScale.x + DeltaScale,
                rect.transform.localScale.y + DeltaScale,
                rect.transform.localScale.z + DeltaScale);

            if (rect.transform.localScale.x > originalScale)
            {
                rect.transform.localScale = new Vector3(originalScale, originalScale, originalScale);
            }
        }



	}
}
