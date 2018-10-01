using System.Collections;
using UnityEngine;

public class Evaluation : MonoBehaviour
{
    public float fEvaluationEndFrame;   //判定描画終了のフレーム
           float fCntFrame;             //経過フレーム


	void Start( )
    {
	    fCntFrame = 0.0f;	
	}
	

	void Update( )
    {
        fCntFrame += Time.deltaTime;

		this.transform.position += new Vector3 ( 0.0f , 0.05f , 0.0f );

        if( fCntFrame >= fEvaluationEndFrame )
        {
            Destroy( this.gameObject );
        }
	}
}
