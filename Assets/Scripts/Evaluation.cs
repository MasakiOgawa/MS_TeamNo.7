using System.Collections;
using UnityEngine;

public class Evaluation : MonoBehaviour
{
    float fCntFrame;   //経過フレーム


	void Start( )
    {
	    fCntFrame = 0.0f;	
	}
	

	void Update( )
    {
        fCntFrame += Time.deltaTime;

		this.transform.position += new Vector3 ( 0.0f , 0.05f , 0.0f );

        if( fCntFrame >= 1.0f )
        {
            Destroy( this.gameObject );
        }
	}
}
