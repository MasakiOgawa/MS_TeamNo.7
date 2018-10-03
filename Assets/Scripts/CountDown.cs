using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public GameObject ManagerPrefab;   //マネージャのプレハブ
    public GameObject BGMPrefab;       //BGMのプレハブ
    public GameObject SamplePrefab;   
           float      fCntFrame;       //経過フレーム
           Text       CountDownText;   //カウントダウンのテキスト情報


	void Start( )
    {
        //変数の初期化
		fCntFrame = 0.0f;

        //テキスト情報を取得
        CountDownText = GameObject.Find( "CountDown" ).GetComponent< Text >( );

        //テキストの設定
        SetText( );
	}
	

    public void CountFrame( )
    {
        //フレーム数を計測
		fCntFrame += Time.deltaTime;

        //一定フレームが経過したらカウントダウン
        if( fCntFrame >= 60.0f / ( float )BGMPrefab.GetComponent< BGM >( ).GetBPM( ) )
        {
            fCntFrame = 0.0f;
          
            if( CountDownText.text == "3" )
            {
                CountDownText.text = "2";
                SamplePrefab.GetComponent< Sample >( ).Emit( );
            }
            else if( CountDownText.text == "2" )
            {
                CountDownText.text = "1";
                SamplePrefab.GetComponent< Sample >( ).Emit( );
            }
            else if( CountDownText.text == "1" )
            {
                CountDownText.text = "GO!!";
                SamplePrefab.GetComponent< Sample >( ).Emit( );
            }

            //カウントダウンの終了
            else if( CountDownText.text == "GO!!" )
            {
                CountDownText.text = " ";
         
                //プレイヤーのダンス
                ManagerPrefab.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_PLAYER_DANCE );
            }
        }
    }


    public void SetText( )
    {
        if( ManagerPrefab.GetComponent< Manager >( ).GetPhase( ) != Manager.GAME_PHASE.PHASE_END_PERFORMANCE )
        {
            CountDownText.text = "3";

            SamplePrefab.GetComponent< Sample >( ).Emit( );
        }
    }
}
