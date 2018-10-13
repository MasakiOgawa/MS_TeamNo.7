using System.Collections;
using UnityEngine;


public class PerformanceManager : MonoBehaviour
{
    public GameObject    ManagerObj;            //マネージャのオブジェクト
    public GameObject    ResultManagerPrefab;   //リザルトマネージャのプレハブ
    public PlayerManager PlayerManagerObj;      //プレイヤーマネージャのオブジェクト
    public CountDown     CountDownObj;          //カウントダウンのオブジェクト
           float         fBPM;                  //BPM
           float         fCntFrame;             //フレーム数のカウンタ
           int           nCntPose;              //プレイヤーのダンス数のカウンタ
           int           nCntPerformance;       //パフォーマンス数のカウンタ

    struct PerformanceType
    {
        public int nPerformanceTiming;    //何回ダンスを行った後に、パフォーマンスに入るか？
        public int nPerformanceMeasure;   //何小節パフォーマンスを行うか？
    }

    PerformanceType[ ] aPerformanceType;   //パフォーマンスタイプの情報


    void Start( )
    {
        //変数の初期化
        fCntFrame        = 0.0f;
        nCntPose         = 0;
        nCntPerformance  = 0;
        fBPM             = 60.0f / ManagerObj.GetComponent< Manager >( ).GetBGM( ).GetComponent< BGM >( ).GetBPM( );
        PlayerManagerObj = ManagerObj.GetComponent< Manager >( ).GetPlayerManager( ).GetComponent< PlayerManager >( );
        CountDownObj     = ManagerObj.GetComponent< Manager >( ).GetCountDown( ).GetComponent< CountDown >( );
        aPerformanceType = new PerformanceType[ 10 ];
        aPerformanceType[ 0 ].nPerformanceTiming  = 4;
        aPerformanceType[ 0 ].nPerformanceMeasure = 32;
    }


    //最初のパフォーマンス
    public void FirstPerformance( )
    {
        //フレーム数を計測
        fCntFrame += Time.deltaTime;
        
        //16拍でダンスの終了
        if( fCntFrame >= fBPM * 16.0f )
        {
            //パフォーマンスを終えたら、敵の生成
            ManagerObj.GetComponent< Manager >( ).GetCountDown( ).GetComponent< CountDown >( ).SetText( );
            fCntFrame = 0.0f;
            ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
        }
    }


    //最後のパフォーマンス
    public void FinalPerformance( )
    {
        //パフォーマンスを終えたら、ランキングの生成
        Instantiate( ResultManagerPrefab , new Vector3( 0.0f , 0.0f , 0.0f ) , Quaternion.identity );
        ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_RESULT );
    }


    //遷移先の状態を決める
    public void PhaseCheck( )
    {
        nCntPose++;

        //パフォーマンスを挟む
        if( nCntPose == aPerformanceType[ 0 ].nPerformanceTiming )
        {
            PlayerManagerObj.SetnPerformanceTmp( aPerformanceType[ 0 ].nPerformanceMeasure );
            ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_CAMERA_PERFORMANCE );
        }
        //敵を生成する
        else
        {
            CountDownObj.SetText( );
            ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
        }
    }
}
