using System.Collections;
using UnityEngine;


public class Evaluation : MonoBehaviour
{
    public float fEvaluationEndFrame;   //判定描画終了のフレーム
    public float fMoveY;                //Y移動量
           float fCntFrame;             //経過フレーム


	void Start( )
    {
        //変数の初期化
        fMoveY    = 0.0f;
	    fCntFrame = 0.0f;	
	}
	

	void Update( )
    {
        fCntFrame += Time.deltaTime;

		this.transform.position += new Vector3 ( 0.0f , fMoveY , 0.0f );

        if( fCntFrame >= fEvaluationEndFrame )
        {
            Destroy( this.gameObject );
        }
	}
}
