using System.Collections;
using UnityEngine;

public class Bonus : MonoBehaviour {

    public GameObject ManagerObj;     //マネージャのオブジェクト
    public CountDown  CountDownObj;   //カウントダウンのオブジェクト
           float      fBPM;           //BPM
           float      fCntFrame;      //フレーム数のカウンタ

    Manager ManagerClass;

    bool bFlg;


	void Start( )
    {
        //変数の初期化
        fBPM = 60.0f / ManagerObj.GetComponent< Manager >( ).GetBGM( ).GetComponent< BGM >( ).GetBPM( );
        fCntFrame = 0.0f;
        CountDownObj = ManagerObj.GetComponent< Manager >( ).GetCountDown( ).GetComponent< CountDown >( );

        ManagerClass = ManagerObj.GetComponent< Manager >( );

        bFlg = false;
	}


    public void BonusTime( )
    {
        //フレーム数を計測
        // fCntFrame += Time.deltaTime;

      /*  if(bFlg == false)
        { 
            bFlg = true;

            ManagerClass.ResetlCntFrame( );
        }*/

         //33拍でボーナスの終了
        if( /*fCntFrame >= fBPM * 33.0f*/ ManagerClass.GetlCntFrame( ) > 1771 )
        {
         //   CountDownObj.SetText( );
            ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_CHECK );

          //  Debug.Log( "aaa" );

            ManagerClass.ResetlCntFrame( );
        }
    }
}
