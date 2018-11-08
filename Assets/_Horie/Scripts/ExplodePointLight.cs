using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodePointLight : MonoBehaviour {

    public enum LIGHT_TYPE
    {
        TYPE_BLUE,
        TYPE_GREEN,
        TYPE_ORANGE,
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetColor ( LIGHT_TYPE type)
    {
        if ( type == LIGHT_TYPE.TYPE_BLUE)
        {
            this.GetComponent<Light>().color = new Color((float)55 / (float)255,
                (float)98 / (float)255,
                (float)255 / (float)255,
                (float)255 / (float)255);
        }
        else if ( type == LIGHT_TYPE.TYPE_GREEN)
        {
            this.GetComponent<Light>().color = new Color((float)38 / (float)255,
                (float)255 / (float)255,
                (float)71 / (float)255,
                (float)255 / (float)255);
        }
        else if ( type == LIGHT_TYPE.TYPE_ORANGE)
        {
            this.GetComponent<Light>().color = new Color((float)255 / (float)255,
                (float)152 / (float)255,
                (float)74 / (float)255,
                (float)255 / (float)255);
        }



    }
}
