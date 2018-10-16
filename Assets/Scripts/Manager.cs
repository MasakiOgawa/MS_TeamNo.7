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
        PHASE_CHECK                ,   //次の遷移先を決める
        PHASE_BONUS                ,   //ボーナスタイム
        PHASE_END_PERFORMANCE      ,   //ゲーム終了時の演出
        PHASE_RESULT                   //リザルト
    };

    //ゲームの初期状態の設定
    public GAME_PHASE GamePhase;

    public GameObject CameraObj;               //カメラのオブジェクト
    public GameObject BGMObj;                  //BGMのオブジェクト
    public GameObject RhythmObj;               //リズムのオブジェクト
    public GameObject PerformanceManagerObj;   //パフォーマンスマネージャのオブジェクト
    public GameObject PlayerManagerObj;        //プレイヤーマネージャのオブジェクト
    public GameObject PlayersObj;              //プレイヤー達のオブジェクト
    public GameObject EnemyManagerObj;         //エネミーマネージャのオブジェクト
    public GameObject CountDownObj;            //カウントダウンのオブジェクト
    public GameObject ScoreManagerObj;         //スコアマネージャのオブジェクト
    public GameObject MapManagerObj;           //マップマネージャのオブジェクト
    public GameObject BonusObj;                //ボーナスのオブジェクト

    PerformanceManager PerformanceManagerClass;
    EnemyManager       EnemyManagerClass;
    CountDown          CountDownClass;

    void Start( )
    {
        PerformanceManagerClass = PerformanceManagerObj.GetComponent< PerformanceManager >( );
        EnemyManagerClass = EnemyManagerObj.GetComponent< EnemyManager >( );
        CountDownClass = CountDownObj.GetComponent< CountDown >( );
    }


	void Update( )
    {
        //ゲームの進行状態によって遷移
        switch( GamePhase )
        {
            //BGMの再生
            case GAME_PHASE.PHASE_BGM_START :
                //BGMObj.GetComponent< BGM >( ).EmitBGM( );
            break;

            //ゲーム開始時の演出
            case GAME_PHASE.PHASE_FIRST_PERFORMANCE :
                PerformanceManagerClass.FirstPerformance( );
            break;

            //敵の出現
            case GAME_PHASE.PHASE_ENEMY_APPEARANCE :
                EnemyManagerClass.Create( );
            break;

            //カウントダウン
            case GAME_PHASE.PHASE_COUNT_DOWN :
                CountDownClass.ChangeText( );
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

             //ボーナスタイム
            case GAME_PHASE.PHASE_BONUS :
                 BonusObj.GetComponent< Bonus >( ).BonusTime( );
            break;

            //遷移先をチェック
            case GAME_PHASE.PHASE_CHECK :
                 PerformanceManagerObj.GetComponent< PerformanceManager >( ).PhaseCheck( );
            break;
         
            //ゲーム終了時の演出(ここから遷移)
            case GAME_PHASE.PHASE_END_PERFORMANCE :  
                 PerformanceManagerObj.GetComponent< PerformanceManager >( ).FinalPerformance( );
            break;

            //リザルト
            case GAME_PHASE.PHASE_RESULT :
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


    //パフォーマンスマネージャのオブジェクトを取得
    public GameObject GetPerformanceManager( )
    {
       return PerformanceManagerObj;
    }
}
