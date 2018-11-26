using System.Collections;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public GameObject         ManagerObj;                //マネージャのオブジェクト
           Manager            ManagerClass;              //マネージャのクラス
           GameObject         PlayersObj;                //プレイヤー達のオブジェクト
           PlayerLeft         PlayerLeftClass;           //左プレイヤーのクラス
           PlayerCenter       PlayerCenterClass;         //中央プレイヤーのクラス
           PlayerRight        PlayerRightClass;          //右プレイヤーのクラス
    public GameObject         PlayerLeftPrefab;          //左プレイヤーのプレハブ
    public GameObject         PlayerCenterPrefab;        //中央プレイヤーのプレハブ
    public GameObject         PlayerRightPrefab;         //右プレイヤーのプレハブ 
           EnemyManager       EnemyManagerClass;         //エネミーマネージャのクラス
           Rhythm             RhythmClass;               //リズムのクラス
           PerformanceManager PerformanceManagerClass;   //パフォーマンスマネージャのクラス
           bool               bTargetChangeFlg;          //ターゲット切り替えのフラグ
           int                nTargetNo;                 //現在のターゲット
    public float              fMove;                     //プレイヤー達の移動量
    public float              fDist;                     //プレイヤー達の移動距離
           int                nCntRhythm;                //リズムが鳴った回数
           int                nPerformanceBar;           //パフォーマンスの小節数
    public float              fHalfBar;                  //半拍分のリズム

    ScoreManager ScoreClass;
    PerformanceManager PerformanceClass;
    
    BGM BGMClass;

    public float OneBeat;

    bool bFlg;
    bool bFlg2;

    public GameObject MotionManagerObj;
    MotionManager MotionManagerClass;

    public GameObject CameraObj;
    CameraPerformance CameraPerformanceClass;


	void Start( )
    {
        //変数の初期化
        bTargetChangeFlg = false;  
        nTargetNo        = 0;
        fDist            = 0.0f;
        nCntRhythm       = 0;
        bFlg = false;
        bFlg2 = false;

        //各クラスの情報を取得
        ManagerClass            = ManagerObj.GetComponent< Manager >( );
        EnemyManagerClass       = ManagerClass.GetEnemyManager( ).GetComponent< EnemyManager >( );
        RhythmClass             = ManagerClass.GetRhythm( ).GetComponent< Rhythm >( );
        PlayerLeftClass         = PlayerLeftPrefab.GetComponent< PlayerLeft >( );
        PlayerCenterClass       = PlayerCenterPrefab.GetComponent< PlayerCenter >( );
        PlayerRightClass        = PlayerRightPrefab.GetComponent< PlayerRight >( );
        PerformanceManagerClass =  ManagerClass.GetPerformanceManager( ).GetComponent< PerformanceManager >( );
        
        //プレイヤー達のオブジェクトを取得
        PlayersObj = ManagerClass.GetPlayers( );   

        ScoreClass = ManagerClass.GetScoreManager( ).GetComponent< ScoreManager >( );
        PerformanceClass = ManagerClass.GetPerformanceManager( ).GetComponent< PerformanceManager >( );

        BGMClass = ManagerClass.GetBGM( ).GetComponent< BGM >( );

        MotionManagerClass = MotionManagerObj.GetComponent< MotionManager >( );

        CameraPerformanceClass = CameraObj.GetComponent< CameraPerformance >( );
	}
	

    //プレイヤー達の入力処理
    void Update( )
    {
        //ダンス状態ならプレイヤーの入力を検出
        if( ManagerClass.GetPhase( ) == Manager.GAME_PHASE.PHASE_PLAYER_DANCE )
        {
            PlayerLeftClass.Pose( );
            PlayerCenterClass.Pose( );
            PlayerRightClass.Pose( );
        }
        //ボーナス状態ならプレイヤーの入力を検出
        else if( ManagerClass.GetPhase( ) == Manager.GAME_PHASE.PHASE_BONUS )
        {
            PlayerLeftClass.BonusPlay( );
            PlayerCenterClass.BonusPlay( );
            PlayerRightClass.BonusPlay( );
        }
    }


    //プレイヤーのダンス
    public void Dance( )
    {
         //半拍毎にターゲットを切り替える
        if( ManagerClass.GetdCntHalfFrame( ) > fHalfBar || bTargetChangeFlg == false )
        {
            bTargetChangeFlg = true;

            ManagerClass.ResetdCntHalfFrame( );//???
            
            //次のターゲットを設定
            EnemyManagerClass.SetTarget( nTargetNo );
            nTargetNo++;

            //ジョイコンを振れる様にする
            PlayerLeftClass.ReleasebPoseFlg( );
            PlayerCenterClass.ReleasebPoseFlg( );
            PlayerRightClass.ReleasebPoseFlg( );

            ScoreClass.ResetnBeatEvaluation( );
        }

        //1拍毎にリズムを鳴らす
        if( ManagerClass.GetdCntFrame( ) >= OneBeat )
        {
            nCntRhythm++;
            ManagerClass.SetFlg( );
            
            if( nCntRhythm < 4 )
            {
               RhythmClass.Emit( );
            }  
        }

        //四拍でダンスの終了
        if( nCntRhythm >= 4 )
        {
            nCntRhythm       = 0;
            nTargetNo        = 0;
            bTargetChangeFlg = false;

            ManagerClass.ResetdPoseFrame( );//???

            //スコアの集計
            //ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_AGGREGATE_SCORE );

            if( ManagerClass.GetPhase( ) != Manager.GAME_PHASE.PHASE_END_PERFORMANCE )
            {
                MotionManagerClass.ChangeAllMotion(PlayerAnimDefine.Idx.Idle);
            }
          
            ScoreClass.AggregateScore( );
            PerformanceClass.PhaseCheck( );
         
        }
    }


    //プレイヤー達の移動
    public void PlayersMove( )
    {
        if( CameraPerformanceClass.GetbMoveFlg( ) == false )
        {
            PlayersObj.transform.position += new Vector3( 0.0f , 0.0f , fMove );
            fDist                         += fMove;
        }
        
        if( bFlg == false )
        {
          /*  if( PerformanceManagerClass.GetnCntPerformance( ) == 1 && ManagerClass.GetdCntFrame( ) >= 8.28f )
            {
                bFlg = true;
                MotionManagerClass.ChangeAllMotion( PlayerAnimDefine.Idx.HipHopDancing1 );
                EnemyManagerClass.TakeInEnemyMotion( PlayerAnimDefine.Idx.HipHopDancing1 );
            }
            else */if( PerformanceManagerClass.GetnCntPerformance( ) == 5 && ManagerClass.GetdCntFrame( ) >= 7.36 )
            {
                CameraPerformanceClass.TruebMoveFlg( );

                bFlg = true;
                bFlg2 = true;
                MotionManagerClass.ChangeAllMotion( PlayerAnimDefine.Idx.HeadSpinning );
                EnemyManagerClass.TakeInEnemyMotion( PlayerAnimDefine.Idx.HeadSpinning );


                //Y座標の調整
                PlayerLeftPrefab.transform.position += new Vector3( 0.0f , -0.59f , 0.0f );
                PlayerCenterPrefab.transform.position += new Vector3( 0.0f , -0.64f , 0.0f );
                PlayerRightPrefab.transform.position += new Vector3( 0.0f , -0.59f , 0.0f );
                EnemyManagerClass.TakeInEnemySetPosY( -0.64f , -0.59f );
            }
        }

    /*    if( bFlg2 ==true )
        {
            MotionManagerClass.HeadDanceRotate( );
            EnemyManagerClass.TakeInEnemyRotate( );
        }*/


        //パフォーマンスが終了したら
        if( ManagerClass.GetdCntFrame( ) >= ( float )nPerformanceBar * /*OneBeat*/0.92f )
        {
           MotionManagerClass.ApplyFalse( );
            EnemyManagerClass.ApplyFalse( );

            if( PerformanceManagerClass.GetnCntPerformance( ) == 5 )
            {
                PlayerLeftPrefab.transform.position += new Vector3( 0.0f , 0.59f , 0.0f );
                PlayerCenterPrefab.transform.position += new Vector3( 0.0f , 0.64f , 0.0f );
                PlayerRightPrefab.transform.position += new Vector3( 0.0f , 0.59f , 0.0f );
                EnemyManagerClass.TakeInEnemySetPosY( 0.64f , 0.59f );
            }

            bFlg = false;
            MotionManagerClass.ChangeAllMotion( PlayerAnimDefine.Idx.Idle );
            EnemyManagerClass.TakeInEnemyMotion( PlayerAnimDefine.Idx.Idle );

            //現在のパフォーマンスによって遷移先を決める
            if( PerformanceManagerClass.GetnCntPerformance( ) == 3 )
            {
                //ボーナス
                //虹色の何か？？？
                ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_BONUS );
            }
            else if( PerformanceManagerClass.GetnCntPerformance( ) == 6 )
            {
                //最後のパフォーマンス
                // ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_END_PERFORMANCE );
                Debug.Log("PlayerManager通過");
            }
            else
            {
                //敵を出現させる
                ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
                ManagerClass.GetCountDown( ).GetComponent< CountDown >( ).ActiveCountDown( );
            }

            ManagerClass.SetFlg( );
        }
    }


    //ターゲットの番号を取得
    public int GetnTargetNo( )
    {
        return nTargetNo;
    }


    //パフォーマンスの小節数を設定
    public void SetnPerformanceBar( int nSetPerformanceBar )
    {
        nPerformanceBar = nSetPerformanceBar;
    }


    public float GetfDist( )
    {
        return fDist;
    }
}
