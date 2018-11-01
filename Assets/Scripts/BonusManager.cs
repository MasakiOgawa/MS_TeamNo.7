using System.Collections;
using UnityEngine;
using Uniduino;


public class BonusManager : MonoBehaviour
{
           Manager    ManagerClass;   //マネージャのクラス
    public GameObject ManagerObj;     //マネージャのオブジェクト
           bool       bFlg;


    TextAsset BonusText;   //ボーナス情報が格納されたファイルの情報

    public SerialHandler SerialHandlerClass;
    public Arduino ArdiunoClass;


	void Start( )
    {  

        //各クラスの情報を取得
        ManagerClass = ManagerObj.GetComponent< Manager >( );

        bFlg = false;

        ArdiunoClass = Arduino.global;
        ArdiunoClass.Setup( ConfigurePins );
	}


    void ConfigurePins( )
    {
        ArdiunoClass.pinMode( 6 , PinMode.OUTPUT );
    }

    //ボーナスタイム
    public void BonusTime( )
    {
        if( bFlg == false )
        {
            bFlg = true;
            SerialHandlerClass.Write( "5" );
        }

         //33拍でボーナスの終了
        if( ManagerClass.GetdCntFrame( ) >= 0.895f * 33.0f )
        {
            Debug.Log( "aaa" );
            ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_CHECK );  
            ManagerClass.SetFlg( );
            SerialHandlerClass.Write( "3" );
        }
    }
}