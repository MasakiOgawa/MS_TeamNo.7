using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Manager : MonoBehaviour
{
    //列挙型定義
    public enum GAME_PHASE
    { 
        PHASE_BGM_START            ,   //BGMの再生
        PHASE_FIRST_PERFORMANCE    ,   //ゲーム開始時の演出
        PHASE_ENEMY_APPEARANCE     ,   //敵の出現
        PHASE_COUNT_DOWN           ,   //カウントダウン
        PHASE_PLAYER_DANCE         ,   //プレイヤーのダンス
        PHASE_AGGREGATE_SCORE      ,   //スコアの集計
        PHASE_CAMERA_PERFORMANCE   ,   //カメラのパフォーマンス
        PHASE_BGM_END_CHECK        ,   //BGMの終了状態をチェック(ゲーム終了に繋げる)
        PHASE_END_PERFORMANCE          //ゲーム終了時の演出
    };

    //ゲームの初期状態の設定
    public GAME_PHASE GamePhase = GAME_PHASE.PHASE_BGM_START;

    public GameObject CameraObj;          //カメラのオブジェクト
    public GameObject BGMObj;             //BGMのオブジェクト
    public GameObject RhythmObj;          //リズムのオブジェクト
    public GameObject PlayerManagerObj;   //プレイヤーマネージャのオブジェクト
    public GameObject EnemyManagerObj;    //エネミーマネージャのオブジェクト
    public GameObject CountDownObj;       //カウントダウンのオブジェクト
    public GameObject ScoreManagerObj;    //スコアマネージャのオブジェクト
    public GameObject MapManagerObj;      //マップマネージャのオブジェクト

    public GameObject PlayersObj;         //プレイヤー達のオブジェクト
     

	void Update( )
    {
        //ゲームの進行状態によって遷移
        switch( GamePhase )
        {
            //BGMの再生
            case GAME_PHASE.PHASE_BGM_START :
                BGMObj.GetComponent< BGM >( ).EmitBGM( );
            break;

            //ゲーム開始時の演出
            case GAME_PHASE.PHASE_FIRST_PERFORMANCE :
            break;

            //敵の出現
            case GAME_PHASE.PHASE_ENEMY_APPEARANCE :
                EnemyManagerObj.GetComponent< EnemyManager >( ).Create( );
            break;

            //カウントダウン
            case GAME_PHASE.PHASE_COUNT_DOWN :
                CountDownObj.GetComponent< CountDown >( ).ChangeText( );
            break;

            //プレイヤーのダンス
            case GAME_PHASE.PHASE_PLAYER_DANCE :
                PlayerManagerObj.GetComponent< PlayerManager >( ).Dance( );
            break;

             //スコアの集計
            case GAME_PHASE.PHASE_AGGREGATE_SCORE :
                 ScoreManagerObj.GetComponent< ScoreManager >( ).AggregateScore( );
            break;

            //カメラのパフォーマンス
            case GAME_PHASE.PHASE_CAMERA_PERFORMANCE :
                 CameraObj.GetComponent< CameraPerformance >( ).PerformanceMove( );
                 PlayerManagerObj.GetComponent< PlayerManager >( ).PlayersMove( );
            break;

            //BGMの終了状態をチェック
            case GAME_PHASE.PHASE_BGM_END_CHECK :
                 BGMObj.GetComponent< BGM >( ).CheckEndBGM( );
            break;
         
            //ゲーム終了時の演出(ここから遷移)
            case GAME_PHASE.PHASE_END_PERFORMANCE :
                 SceneManager.LoadScene( "Result" );
            break;
        }
	}


    //ゲームの進行状態を設定
    public void SetPhase( GAME_PHASE Phase )
    {
        GamePhase = Phase;
    }


    //ゲームの進行状態を取得
    public GAME_PHASE GetPhase( )
    {
       return GamePhase;
    }


    //BGMのオブジェクトを取得
    public GameObject GetBGM( )
    {
       return BGMObj;
    }


    //リズムのオブジェクトを取得
    public GameObject GetRhythm( )
    {
       return RhythmObj;
    }


    //プレイヤーマネージャのオブジェクトを取得
    public GameObject GetPlayerManager( )
    {
       return PlayerManagerObj;
    }


    //エネミーマネージャのオブジェクトを取得
    public GameObject GetEnemyManager( )
    {
       return EnemyManagerObj;
    }


    //カウントダウンのオブジェクトを取得
    public GameObject GetCountDown( )
    {
       return CountDownObj;
    }


    //スコアマネージャのオブジェクトを取得
    public GameObject GetScoreManager( )
    {
       return ScoreManagerObj;
    }


    //マップマネージャのオブジェクトを取得
    public GameObject GetMapManager( )
    {
       return MapManagerObj;
    }


    //プレイヤー達のオブジェクトを取得
    public GameObject GetPlayers( )
    {
       return PlayersObj;
    }
}
