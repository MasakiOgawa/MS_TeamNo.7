using System.Collections;
using UnityEngine;


public class BGM : MonoBehaviour
{
    public GameObject  ManagerObj;   //マネージャのオブジェクト
    public AudioClip   BGMData;      //BGM情報
           AudioSource audioSource; 
           int         nCntCollect;
           float       fAllFrame;
           float       fAllFrameTmp;


	void Start( )
    {
        audioSource      = gameObject.GetComponent< AudioSource >( );
        audioSource.clip = BGMData;

        nCntCollect = 0;
        fAllFrame   = 0.0f;
        fAllFrameTmp = 0.0f;
	}


    //BGMを再生
    public void EmitBGM( )
    {
        //BGMを再生
       // audioSource.Play( );

        //最初のパフォーマンスに遷移
       // ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_FIRST_PERFORMANCE );
    }


    //BPMの取得
    public float GetBPM( )
    {
        return 65.5f;
    }


    //BGMのずれを補正
    public void SetTime( int nType , float fFrame )
    {
        nCntCollect++;

        if( nType == 0 || nCntCollect % 20 == 0 )   //%は可変
        {
            nCntCollect = 0;
            audioSource.time = fFrame + fAllFrame;
            fAllFrameTmp = fFrame;
        }
    }


    public void SetAllFrame( )
    {
        fAllFrame += fAllFrameTmp;
        nCntCollect = 0;
     //   Debug.Log( fAllFrame.ToString( ));
    }


    public void InitCollect( )
    {
        fAllFrame += ( audioSource.time - fAllFrame );
       // Debug.Log( fAllFrame.ToString( ));
    }
}
