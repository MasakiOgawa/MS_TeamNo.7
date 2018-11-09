using System.Collections;
using UnityEngine;


public class Manager : MonoBehaviour
{
    //列挙型定義
    public enum GAME_PHASE
    { 
        PHASE_NONE ,
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
    public GameObject BonusObj;                //ボーナスのオブジェクト

    PerformanceManager PerformanceManagerClass;
    EnemyManager       EnemyManagerClass;
    CountDown          CountDownClass;
    PlayerManager      PlayerManagerClass;
    BonusManager       BonusClass;

    public float dCntFrame;       //フレーム数のカウンタ
    public float dCntHalfFrame;   //半拍分のカウンタ
    public float dPoseFrame;      //四拍分のカウンタ
    public float fBonusFrame;

    bool bFlg;
    bool bFlg2;

    public GameObject StartFilterObj;
    StartFilter StartFilterClass;
    public GameObject MirrorBallObj;
    MirrorBallMaterial MirrorBallClass;


    void Start( )
    {
        PerformanceManagerClass = PerformanceManagerObj.GetComponent< PerformanceManager >( );
        EnemyManagerClass = EnemyManagerObj.GetComponent< EnemyManager >( );
        CountDownClass = CountDownObj.GetComponent< CountDown >( );
        PlayerManagerClass = PlayerManagerObj.GetComponent< PlayerManager >( );
        BonusClass = BonusObj.GetComponent< BonusManager >( );

        //フレームカウンタの初期化
        dCntFrame     = 0;
        dCntHalfFrame = 0;
        dPoseFrame    = 0;
        fBonusFrame = 0.0f;

        bFlg = false;
        bFlg2 = false;
     
        StartFilterClass = StartFilterObj.GetComponent< StartFilter >( );

        MirrorBallClass = MirrorBallObj.GetComponent< MirrorBallMaterial >( );
    }


    //一定間隔で呼ばれる
	void FixedUpdate( )
    {
        //ゲームの進行状態によって遷移
        switch( GamePhase )
        {
            case GAME_PHASE.PHASE_NONE :
                if( dCntFrame >= 3.0d )
                {
                    bFlg = true;
                    GamePhase = GAME_PHASE.PHASE_FIRST_PERFORMANCE;
                }
            break;

            //ゲーム開始時の演出
            case GAME_PHASE.PHASE_FIRST_PERFORMANCE :
                PerformanceManagerClass.FirstPerformance( );
                StartFilterClass.AddAlpha( );
            break;

            //敵の出現
            case GAME_PHASE.PHASE_ENEMY_APPEARANCE :
                EnemyManagerClass.ActiveTrue( );
            break;

            //カウントダウン
            case GAME_PHASE.PHASE_COUNT_DOWN :
                CountDownClass.ChangeCount( );
            break;

            //プレイヤーのダンス
            case GAME_PHASE.PHASE_PLAYER_DANCE :
                dCntHalfFrame += 0.01666667f; ///???
                dPoseFrame    += 0.01666667f; ///??
                PlayerManagerClass.Dance( );
            break;

             //スコアの集計
            case GAME_PHASE.PHASE_AGGREGATE_SCORE :
                 //ScoreManagerObj.GetComponent< ScoreManager >( ).AggregateScore( );
            break;

            //カメラのパフォーマンス
            case GAME_PHASE.PHASE_CAMERA_PERFORMANCE :
                 CameraObj.GetComponent< CameraPerformance >( ).PerformanceMove( );
                 PlayerManagerClass.PlayersMove( );
            break;

             //ボーナスタイム
            case GAME_PHASE.PHASE_BONUS :
                MirrorBallClass.BonusMaterial( );
                 BonusClass.BonusTime( );
                 fBonusFrame += 0.01666667f;
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

        dCntFrame += 0.01666667f;

        if( bFlg == true )
        {
            bFlg = false;
            dCntFrame = 0.0f;
        }

        if( bFlg2 == true )
        {
            bFlg2 = false;
            fBonusFrame = 0.0f;
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

      public GameObject GetBonusManager( )
    {
       return BonusObj;
    }



    public float GetdCntFrame( )
    {
        return dCntFrame;
    }


    public void ResetdCntFrame( )
    {
        dCntFrame = 0;
    }


    public float GetdCntHalfFrame( )
    {
        return dCntHalfFrame;
    }


    public void ResetdCntHalfFrame( )
    {
        dCntHalfFrame = 0;
    }


    public float GetdPoseFrame( )
    {
        return dPoseFrame;
    }


    public void  ResetdPoseFrame( )
    {
        dPoseFrame = 0.0f;
    }


    public float GetdBonusFrame( )
    {
        return fBonusFrame;
    }


    public void SetFlg( )
    {
        bFlg = true;
    }


    public void SetFlg2( )
    {
        bFlg2 = true;
    }
}
