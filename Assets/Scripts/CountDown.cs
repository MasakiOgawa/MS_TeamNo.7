using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public static float fCntFrame;       //経過フレーム
    public static Text  CountDownText;   //カウントダウンのテキスト情報


	void Start( )
    {
        //変数の初期化
		fCntFrame = 0.0f;

        //テキスト情報を取得
        CountDownText = GameObject.Find( "CountDown" ).GetComponent<Text>( );

        //テキストの設定
        SetText( );
	}
	

    public static void CountFrame( )
    {
        //フレーム数を計測
		fCntFrame += Time.deltaTime;

        //一定フレームが経過したらカウントダウン
        if( fCntFrame >= 60.0f / ( float )BGM.GetBPM( ) )
        {
            fCntFrame = 0.0f;
          
            if( CountDownText.text == "3" )
            {
                CountDownText.text = "2";
                Sample.Emit( );
            }
            else if( CountDownText.text == "2" )
            {
                CountDownText.text = "1";
                Sample.Emit( );
            }
            else if( CountDownText.text == "1" )
            {
                CountDownText.text = "GO!!";
                Sample.Emit( );
            }

            //カウントダウンの終了
            else if( CountDownText.text == "GO!!" )
            {
                CountDownText.text = " ";
         
                //プレイヤーのダンス
                Manager.SetPhase( Manager.GAME_PHASE.PHASE_PLAYER_DANCE );
            }
        }
    }


    public static void SetText( )
    {
        CountDownText.text = "3";

        Sample.Emit( );
    }
}
