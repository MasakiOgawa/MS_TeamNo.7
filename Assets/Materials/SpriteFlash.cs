using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour{

    [SerializeField] 
    private float FlashColor;
    
    [SerializeField]
    private float AddColor;
    
    [SerializeField]
    private float MaxColor;

    [SerializeField]
    private float MinColor;

    bool Flg = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<SpriteRenderer>().color = new Color(FlashColor, FlashColor, FlashColor, 1.0f);


        if (FlashColor >= MaxColor || FlashColor <= MinColor)
            Flg = true;

        if (Flg)
        {
            AddColor *= -1;
            Flg = false;
        }

        FlashColor += AddColor;
	}
}
