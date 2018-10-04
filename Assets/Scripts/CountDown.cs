using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class CountDown : MonoBehaviour
{
    public GameObject ManagerObj;      //マネージャのオブジェクト
           float      fBPM;            //BPM
           float      fCntFrame;       //経過フレーム
           Text       CountDownText;   //カウントダウンのテキスト情報


	void Start( )
    {
        //変数の初期化
		fCntFrame = 0.0f;
        fBPM      = ManagerObj.GetComponent< Manager >( ).GetBGM( ).GetComponent< BGM >( ).GetBPM( );

        //テキスト情報を取得
        CountDownText = GameObject.Find( "CountDown" ).GetComponent< Text >( );

        //テキストの設定
        SetText( );
	}
	

    //テキストの内容をカウントダウン
    public void ChangeText( )
    {
        //フレーム数を計測
		fCntFrame += Time.deltaTime;

        //1拍毎にカウントダウン
        if( fCntFrame >= 60.0f / ( float )fBPM )
        {
            fCntFrame = 0.0f;
          
            if( CountDownText.text == "3" )
            {
                CountDownText.text = "2";
                ManagerObj.GetComponent< Manager >( ).GetRhythm( ).GetComponent< Rhythm >( ).Emit( );
            }
            else if( CountDownText.text == "2" )
            {
                CountDownText.text = "1";
                ManagerObj.GetComponent< Manager >( ).GetRhythm( ).GetComponent< Rhythm >( ).Emit( );
            }
            else if( CountDownText.text == "1" )
            {
                CountDownText.text = "GO!!";
                ManagerObj.GetComponent< Manager >( ).GetRhythm( ).GetComponent< Rhythm >( ).Emit( );
            }

            //カウントダウンの終了
            else if( CountDownText.text == "GO!!" )
            {
                CountDownText.text = " ";
         
                //プレイヤーのダンス
                ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_PLAYER_DANCE );
            }
        }
    }


    //カウントダウンテキストを可視化
    public void SetText( )
    {
        if( ManagerObj.GetComponent< Manager >( ).GetPhase( ) != Manager.GAME_PHASE.PHASE_END_PERFORMANCE )
        {
            CountDownText.text = "3";

            ManagerObj.GetComponent< Manager >( ).GetRhythm( ).GetComponent< Rhythm >( ).Emit( );
        }
    }
}
