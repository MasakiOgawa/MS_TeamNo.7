using System.Collections;
using UnityEngine;
using Uniduino;


public class PerformanceManager : MonoBehaviour
{
    //列挙型定義
    struct PerformanceType
    {
        public int nCntTiming;   //何回ダンスを行った後にパフォーマンスに入るか？
        public int nBar;         //何小節パフォーマンスを行うか？
    }

           Manager            ManagerClass;           //マネージャのクラス
    public GameObject         ManagerObj;             //マネージャのオブジェクト
           BGM                BGMClass;               //BGmのクラス
           PlayerManager      PlayerManagerClass;     //プレイヤーマネージャのクラス
           CountDown          CountDownClass;         //カウントダウンのクラス
    public GameObject         ResultManagerPrefab;    //リザルトマネージャのプレハブ
    public GameObject         CMCameraManagerObj;
           PerformanceType[ ] aPerformanceType;       //パフォーマンス情報の配列
           int                nCntDance;              //ダンス数のカウンタ
           int                nCntPerformance;        //パフォーマンス数のカウンタ
           bool               bFlg;

    ScoreManager ScoreManagerClass;

    public SerialHandler SerialHandlerClass;
    public Arduino ArdiunoClass;

    public GameObject MirrorBallColorObj;
    MirrorBallMaterial MirrorBallMaterialClass;

    public GameObject MotionManagerObj;
    MotionManager MotionManagerClass;

    public GameObject EnemyManagerObj;
    EnemyManager EnemyManagerClass;

    public GameObject AuraObj;

     public AudioClip   Audiotutorial;
            AudioSource AudioSource;
   

    void Start( )
    {
        //変数の初期化
        nCntDance       = 0;
        nCntPerformance = 0;
        bFlg            = false;

        aPerformanceType = new PerformanceType[ 10 ];
        aPerformanceType[ 0 ].nCntTiming = 4;
        aPerformanceType[ 0 ].nBar       = 16;
        aPerformanceType[ 1 ].nCntTiming = 6;
        aPerformanceType[ 1 ].nBar       = 8;
        aPerformanceType[ 2 ].nCntTiming = 8;
        aPerformanceType[ 2 ].nBar       = 8; 
        aPerformanceType[ 3 ].nCntTiming = 0;
        aPerformanceType[ 3 ].nBar       = 0; 
        aPerformanceType[ 4 ].nCntTiming = 9;
        aPerformanceType[ 4 ].nBar       = 16; 
        aPerformanceType[ 5 ].nCntTiming = 15;
        aPerformanceType[ 5 ].nBar       = 16;   

        //各クラスの情報を取得
        ManagerClass       = ManagerObj.GetComponent< Manager >( );
        BGMClass           = ManagerClass.GetBGM( ).GetComponent< BGM >( );
        PlayerManagerClass = ManagerClass.GetPlayerManager( ).GetComponent< PlayerManager >( );
        CountDownClass     = ManagerClass.GetCountDown( ).GetComponent< CountDown >( );

        ScoreManagerClass = ManagerClass.GetScoreManager( ).GetComponent< ScoreManager >( );

        ArdiunoClass = Arduino.global;
        ArdiunoClass.Setup( ConfigurePins );

        MirrorBallMaterialClass = MirrorBallColorObj.GetComponent< MirrorBallMaterial >( );

        MotionManagerClass = MotionManagerObj.GetComponent< MotionManager >( );

        EnemyManagerClass = EnemyManagerObj.GetComponent< EnemyManager >( );

        AudioSource      = gameObject.GetComponent< AudioSource >( );
        AudioSource.clip = Audiotutorial;
    }


    void ConfigurePins( )
    {
        ArdiunoClass.pinMode( 6 , PinMode.OUTPUT );
    }


    //最初のパフォーマンス
    public void FirstPerformance( )
    {
        //一度だけBGMの再生
        if( bFlg == false )
        {
            bFlg = true;
            SerialHandlerClass.Write( "3" );
            MotionManagerClass.ChangeAllMotion( PlayerAnimDefine.Idx.Idle );
            BGMClass.EmitBGM( );
            CMCameraManagerObj.GetComponent< CMCameraManager >( ).SetCutScene( 0 );
        }

        //16拍でダンスの終了
        if( ManagerClass.GetdCntFrame( ) >= 14.765823f )
        {
            //パフォーマンスを終えたら敵の生成
            ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
            MotionManagerClass.ChangeAllMotion( PlayerAnimDefine.Idx.Idle );
            BGMClass.SetBGM( 14.78f );//OK!!
            ManagerClass.SetFlg( );    
         //   TutorialManagerClass.TrueTutorial01( );
            AuraObj.GetComponent< AuraSpotController >( ).IncreaseType( AuraSpotController.AURA_TYPE.TYPE_1 );
        }
    }


    //最後のパフォーマンス
    public void FinalPerformance( )
    {
        //16拍でダンスの終了
        if( ManagerClass.GetdCntFrame( ) >= 14.765823d )
        {
            //パフォーマンスを終えたらランキングの生成
           // ResultManagerPrefab = Instantiate( ResultManagerPrefab , new Vector3( 0.0f , 0.0f , 0.0f ) , Quaternion.identity );
            ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_RESULT );
            MotionManagerClass.ChangeAllMotion( PlayerAnimDefine.Idx.Idle );
            SerialHandlerClass.Write( "6" );
            ResultManagerPrefab.GetComponent< ResultManager >( ).StartResult( ScoreManagerClass.GetnScore( ) );
            ScoreManagerClass.ResetnScore( );
        }
    }


    //遷移先の状態を決める
    public void PhaseCheck( )
    {
        nCntDance++;

       
        if( nCntDance == 9 )
        {
           nCntPerformance = 4; 
        }

        if( nCntDance == aPerformanceType[ nCntPerformance ].nCntTiming )
        {
            if( nCntPerformance == 0 )
            {
                CMCameraManagerObj.GetComponent< CMCameraManager >( ).SetCutScene( 1 );
                MotionManagerClass.ChangeAllMotion( PlayerAnimDefine.Idx.HipHopDancing1 );
                EnemyManagerClass.TakeInEnemyMotion( PlayerAnimDefine.Idx.HipHopDancing1 );
                AuraObj.GetComponent< AuraSpotController >( ).IncreaseType( AuraSpotController.AURA_TYPE.TYPE_2 );
            }
            else if( nCntPerformance == 1 )
            {
                CMCameraManagerObj.GetComponent< CMCameraManager >( ).SetCutScene( 2 );
                MotionManagerClass.ChangeAllMotion( PlayerAnimDefine.Idx.Walk );
                EnemyManagerClass.TakeInEnemyMotion( PlayerAnimDefine.Idx.Walk );
                AuraObj.GetComponent< AuraSpotController >( ).IncreaseType( AuraSpotController.AURA_TYPE.TYPE_3 );
            }
            else if( nCntPerformance == 2 )
            {
                BonusTutorial.Create( 6.0f );
                AudioSource.PlayOneShot( Audiotutorial );
                CMCameraManagerObj.GetComponent< CMCameraManager >( ).SetCutScene( 3 );
                MotionManagerClass.ChangeAllMotion( PlayerAnimDefine.Idx.BboyHipHopMove );
                EnemyManagerClass.TakeInEnemyMotion( PlayerAnimDefine.Idx.BboyHipHopMove );
                AuraObj.GetComponent< AuraSpotController >( ).IncreaseType( AuraSpotController.AURA_TYPE.TYPE_4 );
            }
            else if( nCntPerformance == 4 )
            {
                CMCameraManagerObj.GetComponent< CMCameraManager >( ).SetCutScene( 4 );
                MotionManagerClass.ChangeAllMotion( PlayerAnimDefine.Idx.GangnamStyle );
                EnemyManagerClass.TakeInEnemyMotion( PlayerAnimDefine.Idx.GangnamStyle );
            }
          
            PlayerManagerClass.SetnPerformanceBar( aPerformanceType[ nCntPerformance ].nBar );
            ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_CAMERA_PERFORMANCE );

            nCntPerformance++;

            //最後のパフォーマンスに遷移
            if( nCntPerformance == 6 )
            {
                 CMCameraManagerObj.GetComponent< CMCameraManager >( ).SetCutScene( 5 );
                 MotionManagerClass.ChangeAllMotion( PlayerAnimDefine.Idx.Idle );
                 ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_END_PERFORMANCE );
            }

             MirrorBallMaterialClass.SetColor( 4 );
        }
        //敵を生成する
        else
        {
            MotionManagerClass.ChangeAllMotion( PlayerAnimDefine.Idx.Idle );
            ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
        }

       // ManagerClass.ResetfCntFrame( );//??
       //ManagerClass.SetFlg( );
    }


    //何回目のパフォーマンスかを取得
    public int GetnCntPerformance( )
    {
        return nCntPerformance;
    }
}
