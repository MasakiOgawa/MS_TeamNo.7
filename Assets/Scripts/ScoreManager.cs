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
    public GameObject    EvaluationBadPrefab;
    public GameObject    EvaluationFinePrefab;
    public GameObject    EvaluationExcellentPrefab;
    public GameObject[ ] aEvaluationBadArray;
    public GameObject[ ] aEvaluationFineArray;
    public GameObject[ ] aEvaluationExcellentArray;

    //各評価のカウンタ
    int nCntBad;
    int nCntFine;
    int nCntExcellent;

           Manager            ManagerClass;              //マネージャのクラス
    public GameObject         ManagerObj;                //マネージャのオブジェクト
           EnemyManager       EnemyManagerClass;         //エネミーマネージャのクラス
           PerformanceManager PerformanceManagerClass;   //パフォーマンスマネージャのクラス
    public static int         nScore;                    //スコア


	void Start( )
    {
        //各クラスの情報を取得
        ManagerClass            = ManagerObj.GetComponent< Manager >( );
        EnemyManagerClass       = ManagerClass.GetEnemyManager( ).GetComponent< EnemyManager >( );
        PerformanceManagerClass = ManagerClass.GetPerformanceManager( ).GetComponent< PerformanceManager >( );

        //変数の初期化
		nCntBad       = 0;
        nCntFine      = 0;
        nCntExcellent = 0;
        nScore        = 0;

        //各評価を生成し、非アクティブにしておく
        aEvaluationBadArray       = new GameObject[ 3 ];
        aEvaluationFineArray      = new GameObject[ 3 ];
        aEvaluationExcellentArray = new GameObject[ 3 ];

        for( int nCnt = 0; nCnt < 3; nCnt++ )
        {
            aEvaluationBadArray[ nCnt ]       = Instantiate( EvaluationBadPrefab , new Vector3( 0.0f , 0.0f , 0.0f ) , Quaternion.identity );
            aEvaluationFineArray[ nCnt ]      = Instantiate( EvaluationFinePrefab , new Vector3( 0.0f , 0.0f , 0.0f ) , Quaternion.identity );
            aEvaluationExcellentArray[ nCnt ] = Instantiate( EvaluationExcellentPrefab , new Vector3( 0.0f , 0.0f , 0.0f ) , Quaternion.identity );

            aEvaluationBadArray[ nCnt ].gameObject.SetActive( false );
            aEvaluationFineArray[ nCnt ].gameObject.SetActive( false );
            aEvaluationExcellentArray[ nCnt ].gameObject.SetActive( false );
        }
	}
	

    //評価のアクティブ化
    public void ActiveTrue( Vector3 Pos , EVALUATION Evaluation )
    {
        switch( Evaluation )
        {
            case EVALUATION.EVALUATION_BAD :
                
                for( int nCnt = 0; nCnt < 3; nCnt++ )
                {
                    if( aEvaluationBadArray[ nCnt ].gameObject.activeSelf == false )
                    {
                         aEvaluationBadArray[ nCnt ].gameObject.SetActive( true );
                         aEvaluationBadArray[ nCnt ].gameObject.transform.position = new Vector3( Pos.x , Pos.y , Pos.z - 0.5f );
                         break;
                    }
                }

                nCntBad++;
            break;

            case EVALUATION.EVALUATION_FINE :

                 for( int nCnt = 0; nCnt < 3; nCnt++ )
                 {
                     if( aEvaluationFineArray[ nCnt ].gameObject.activeSelf == false )
                     {
                          aEvaluationFineArray[ nCnt ].gameObject.SetActive( true );
                          aEvaluationFineArray[ nCnt ].gameObject.transform.position = new Vector3( Pos.x , Pos.y , Pos.z - 0.5f );
                          break;
                     }
                 }
                
                nCntFine++;
            break;

            case EVALUATION.EVALUATION_EXCELLENT :

                 for( int nCnt = 0; nCnt < 3; nCnt++ )
                 {
                     if( aEvaluationExcellentArray[ nCnt ].gameObject.activeSelf == false )
                     {
                          aEvaluationExcellentArray[ nCnt ].gameObject.SetActive( true );
                          aEvaluationExcellentArray[ nCnt ].gameObject.transform.position = new Vector3( Pos.x , Pos.y , Pos.z - 0.5f );
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
        nScore += ( nCntExcellent * 3 ) + ( nCntFine * 2 );

        //敵を追従させる
        EnemyManagerClass.TakeIn( ( nCntExcellent * 3 ) + ( nCntFine * 2 ) );

        //各評価のリセット
        nCntBad       = 0;
        nCntFine      = 0;
        nCntExcellent = 0;

        //現在の敵を非アクティブ化
        EnemyManagerClass.ActiveFalse( );

        //遷移先をチェック
        ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_CHECK );
    }
}
