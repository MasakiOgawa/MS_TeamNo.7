using System.Collections;
using UnityEngine;


public class PerformanceManager : MonoBehaviour
{
    public GameObject ManagerObj;            //マネージャのオブジェクト
    public GameObject ResultManagerPrefab;   //リザルトマネージャのプレハブ
           TextAsset  PerformanceText;       //パフォーマンス情報が格納されたファイルの情報

    struct PerformanceType
    {
        char aPerformanceTiming;    //何回ダンスを行った後に、パフォーマンスに入るか？
        char aPerformanceMeasure;   //何小節パフォーマンスを行うか？
    }

    PerformanceType[ ] aPerformanceType;   //パフォーマンス構造体


    void Start( )
    {
        //変数の初期化
      /*  aPerformanceType = new PerformanceType[ 10 ];

        //パフォーマンス情報をファイルから読み込み
        PerformanceText = Resources.Load( "performance" ) as TextAsset;

        for( int nCnt = 0 , nCnt2 = 0; PerformanceText.Text[ nCnt ] != null; nCnt2++ )
        {
            aPerformanceType[ nCnt2 ].aPerformanceTiming  = PerformanceText.Text[ nCnt ];
            aPerformanceType[ nCnt2 ].aPerformanceMeasure = PerformanceText.Text[ nCnt + 1 ];

            nCnt += 2;
        }*/
    }
   

    //最初のパフォーマンス
    public void FirstPerformance( )
    {
        //パフォーマンスを終えたら、敵の生成
        ManagerObj.GetComponent< Manager >( ).GetCountDown( ).GetComponent< CountDown >( ).SetText( );
        ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
    }


    //最後のパフォーマンス
    public void FinalPerformance( )
    {
        //パフォーマンスを終えたら、ランキングの生成
        Instantiate( ResultManagerPrefab , new Vector3( 0.0f , 0.0f , 0.0f ) , Quaternion.identity );
        ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_RESULT );
    }
}
