using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class CountDown : MonoBehaviour
{
           Manager    ManagerClass;   //マネージャのクラス
    public GameObject ManagerObj;     //マネージャのオブジェクト
    public GameObject CountNo3Obj;    //3のオブジェクト
    public GameObject CountNo2Obj;    //2のオブジェクト
    public GameObject CountNo1Obj;    //1のオブジェクト
    public GameObject CountGoObj;     //GOのオブジェクト
           BGM        BGMClass;       //BGMのクラス
           Rhythm     RhythmClass;    //リズムのクラス
           int        nCountDown;     //カウントダウンのカウンタ

    public float OneBeat;


	void Start( )
    {
        //変数の初期化
        nCountDown = 3;

        //各クラスの情報を取得
        ManagerClass = ManagerObj.GetComponent< Manager >( );
        BGMClass     = ManagerClass.GetBGM( ).GetComponent< BGM >( );
        RhythmClass  = ManagerClass.GetRhythm( ).GetComponent< Rhythm >( );
	}
	

    //カウントダウン
    public void ChangeCount( )
    {
        //1拍毎にカウントダウン
        if( ManagerClass.GetdCntFrame( ) >= OneBeat )
        {
            ManagerClass.SetFlg( );
            
            if( nCountDown == 3 )
            {
                nCountDown = 2;
                CountNo3Obj.SetActive( false );
                CountNo2Obj.SetActive( true );
                RhythmClass.Emit( );
            }
            else if( nCountDown == 2 )
            {
                nCountDown = 1;
                CountNo2Obj.SetActive( false );
                CountNo1Obj.SetActive( true );
                RhythmClass.Emit( );
            }
            else if( nCountDown == 1 )
            {
                nCountDown = 0;
                CountNo1Obj.SetActive( false );
                CountGoObj.SetActive( true );
                RhythmClass.Emit( );
            }
            //カウントダウンの終了
            else if( nCountDown == 0 )
            {
                nCountDown = 0;
                CountGoObj.SetActive( false );
                RhythmClass.Emit( );
         
                //プレイヤーのダンス
                ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_PLAYER_DANCE );
            }
        }
    }


    //カウントダウンを可視化
    public void ActiveCountDown( )
    {
        if( ManagerClass.GetPhase( ) != Manager.GAME_PHASE.PHASE_END_PERFORMANCE )
        {
            nCountDown = 3;
            CountNo3Obj.SetActive( true );
          //  RhythmClass.Emit( );
        }
    }
}
