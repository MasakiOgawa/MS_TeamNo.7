using System.Collections;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    //列挙型定義
    public enum EVALUATION
    { 
       EVALUATION_BAD = 0   ,   //悪い
       EVALUATION_FINE      ,   //惜しい
       EVALUATION_EXCELLENT     //良い
    };

    //評価テキストのプレハブ
    public GameObject EvaluationBadPrefab;
    public GameObject EvaluationFinePrefab;
    public GameObject EvaluationExcellentPrefab;

    public GameObject[ ] EvaluationBadPrefabTmp;
    public GameObject[ ] EvaluationFinePrefabTmp;
    public GameObject[ ] EvaluationExcellentPrefabTmp;

    //各評価のカウンタ
    int nCntBad;
    int nCntFine;
    int nCntExcellent;

    public GameObject         ManagerObj;              //ゲームマネージャのオブジェクト
           EnemyManager       EnemyManagerObj;         //エネミーマネージャのオブジェクト
           PerformanceManager PerformanceManagerObj;   //パフォーマンスマネージャのオブジェクト
    public static int         nScore;                  //スコア


	void Start( )
    {
        //変数の初期化
		nCntBad       = 0;
        nCntFine      = 0;
        nCntExcellent = 0;
        nScore        = 0;

        //エネミーマネージャのオブジェクトを取得
        EnemyManagerObj = ManagerObj.GetComponent< Manager >( ).GetEnemyManager( ).GetComponent< EnemyManager >( );

        //パフォーマンスマネージャのオブジェクトを取得
        PerformanceManagerObj = ManagerObj.GetComponent< Manager >( ).GetPerformanceManager( ).GetComponent< PerformanceManager >( );

        EvaluationBadPrefabTmp       = new GameObject[ 3 ];
        EvaluationFinePrefabTmp      = new GameObject[ 3 ];
        EvaluationExcellentPrefabTmp = new GameObject[ 3 ];

        for( int nCnt = 0; nCnt < 3; nCnt++ )
        {
            EvaluationBadPrefabTmp[ nCnt ]       = Instantiate( EvaluationBadPrefab , new Vector3( 0.0f , 0.0f , 0.0f ) , Quaternion.identity );
            EvaluationFinePrefabTmp[ nCnt ]      = Instantiate( EvaluationFinePrefab , new Vector3( 0.0f , 0.0f , 0.0f ) , Quaternion.identity );
            EvaluationExcellentPrefabTmp[ nCnt ] = Instantiate( EvaluationExcellentPrefab , new Vector3( 0.0f , 0.0f , 0.0f ) , Quaternion.identity );

            EvaluationBadPrefabTmp[ nCnt ].gameObject.SetActive( false );
            EvaluationFinePrefabTmp[ nCnt ].gameObject.SetActive( false );
            EvaluationExcellentPrefabTmp[ nCnt ].gameObject.SetActive( false );
        }
	}
	

    //評価の生成
    public void Create( Vector3 Pos , EVALUATION Evaluation )
    {
        switch( Evaluation )
        {
            case EVALUATION.EVALUATION_BAD :
                
                for( int nCnt = 0; nCnt < 3; nCnt++ )
                {
                    if( EvaluationBadPrefabTmp[ nCnt ].gameObject.activeSelf == false )
                    {
                         EvaluationBadPrefabTmp[ nCnt ].gameObject.SetActive( true );
                         EvaluationBadPrefabTmp[ nCnt ].gameObject.transform.position = new Vector3( Pos.x , Pos.y , Pos.z );
                         break;
                    }
                }

                nCntBad++;
            break;

            case EVALUATION.EVALUATION_FINE :

                 for( int nCnt = 0; nCnt < 3; nCnt++ )
                 {
                     if( EvaluationFinePrefabTmp[ nCnt ].gameObject.activeSelf == false )
                     {
                          EvaluationFinePrefabTmp[ nCnt ].gameObject.SetActive( true );
                          EvaluationFinePrefabTmp[ nCnt ].gameObject.transform.position = new Vector3( Pos.x , Pos.y , Pos.z );
                          break;
                     }
                 }
                
                nCntFine++;
            break;

            case EVALUATION.EVALUATION_EXCELLENT :

                 for( int nCnt = 0; nCnt < 3; nCnt++ )
                 {
                     if( EvaluationExcellentPrefabTmp[ nCnt ].gameObject.activeSelf == false )
                     {
                          EvaluationExcellentPrefabTmp[ nCnt ].gameObject.SetActive( true );
                          EvaluationExcellentPrefabTmp[ nCnt ].gameObject.transform.position = new Vector3( Pos.x , Pos.y , Pos.z );
                          break;
                     }
                 }
              
                nCntExcellent++;
            break;
        }
    }


    //スコアの集計
    public void AggregateScore( )
    {
        ScoreManager.nScore += ( nCntExcellent * 3 ) + ( nCntFine * 2 );

        //敵を追従させる
        EnemyManagerObj.TakeIn( ( nCntExcellent * 3 ) + ( nCntFine * 2 ) );

        //各評価のリセット
        nCntBad       = 0;
        nCntFine      = 0;
        nCntExcellent = 0;

        //現在の敵を破棄
        EnemyManagerObj.Kill( );

        //ダンスの回数をカウントしチェック
        ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_CHECK );
    }
}
