using System.Collections;
using UnityEngine;


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
    }


    //最初のパフォーマンス
    public void FirstPerformance( )
    {
        //一度だけBGMの再生
        if( bFlg == false )
        {
            bFlg = true;
            BGMClass.EmitBGM( );
            CMCameraManagerObj.GetComponent< CMCameraManager >( ).SetCutScene( 0 );
        }

        //16拍でダンスの終了
        if( ManagerClass.GetdCntFrame( ) >= 14.765823f )
        {
            //パフォーマンスを終えたら敵の生成
            ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );

            BGMClass.SetBGM( 14.765823f );
            ManagerClass.SetFlg( );    
        }
    }


    //最後のパフォーマンス
    public void FinalPerformance( )
    {
        //16拍でダンスの終了
        if( ManagerClass.GetdCntFrame( ) >= 14.765823d )
        {
            //パフォーマンスを終えたらランキングの生成
            Instantiate( ResultManagerPrefab , new Vector3( 0.0f , 0.0f , 0.0f ) , Quaternion.identity );
            ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_RESULT );
        }
    }


    //遷移先の状態を決める
    public void PhaseCheck( )
    {
        nCntDance++;

        //パフォーマンスを挟む
        if( nCntDance == 9 )
        {
           nCntPerformance = 4; 
        }

        if( nCntDance == aPerformanceType[ nCntPerformance ].nCntTiming )
        {
            if( nCntPerformance == 0 )
            {
                CMCameraManagerObj.GetComponent< CMCameraManager >( ).SetCutScene( 1 );
            }
            else if( nCntPerformance == 1 )
            {
                CMCameraManagerObj.GetComponent< CMCameraManager >( ).SetCutScene( 2 );
            }
            else if( nCntPerformance == 2 )
            {
                CMCameraManagerObj.GetComponent< CMCameraManager >( ).SetCutScene( 3 );
            }
            else if( nCntPerformance == 4 )
            {
                CMCameraManagerObj.GetComponent< CMCameraManager >( ).SetCutScene( 4 );
            }
          
            PlayerManagerClass.SetnPerformanceBar( aPerformanceType[ nCntPerformance ].nBar );
            ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_CAMERA_PERFORMANCE );

            nCntPerformance++;

            //最後のパフォーマンスに遷移
            if( nCntPerformance == 6 )
            {
                 CMCameraManagerObj.GetComponent< CMCameraManager >( ).SetCutScene( 5 );
                 ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_END_PERFORMANCE );
            }
        }
        //敵を生成する
        else
        {
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
