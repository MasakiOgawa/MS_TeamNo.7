using System.Collections;
using UnityEngine;


public class Evaluation : MonoBehaviour
{
    public float fEvaluationEndFrame;   //判定描画終了のフレーム
           float fCntFrame;             //経過フレーム

    SpriteRenderer spriteRenderer;
    float fAlpha;


	void Start( )
    {
        //変数の初期化
	    fCntFrame = 0.0f;	
        fAlpha    = 1.0f;

        spriteRenderer = this.gameObject.GetComponent< SpriteRenderer >( );
	}
	

	void Update( )
    {
        fCntFrame += Time.deltaTime;
       
		//透過させて行く
        fAlpha    -= 1.0f / fEvaluationEndFrame;

        if( fAlpha < 0.0f )
        {
            fAlpha = 0.0f;
        }

        spriteRenderer.material.color = new Color( 1.0f , 1.0f , 1.0f , fAlpha );

        if( fCntFrame >= fEvaluationEndFrame )
        {
            fCntFrame = 0.0f;
            spriteRenderer.material.color = new Color( 1.0f , 1.0f , 1.0f , 1.0f );
            this.gameObject.SetActive( false );
        }
	}
}
