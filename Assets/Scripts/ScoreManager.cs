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

    //各評価のカウンタ
    int nCntBad;
    int nCntFine;
    int nCntExcellent;

    public GameObject   ManagerObj;         //ゲームマネージャのオブジェクト
           EnemyManager EnemyManagerObj;    //エネミーマネージャのオブジェクト
    public static int   nScore;             //スコア


	void Start( )
    {
        //変数の初期化
		nCntBad       = 0;
        nCntFine      = 0;
        nCntExcellent = 0;
        nScore        = 0;

        //エネミーーマネージャのオブジェクトを取得
        EnemyManagerObj = ManagerObj.GetComponent< Manager >( ).GetEnemyManager( ).GetComponent< EnemyManager >( );
	}
	

    //評価の生成
    public void Create( Vector3 Pos , EVALUATION Evaluation )
    {
        switch( Evaluation )
        {
            case EVALUATION.EVALUATION_BAD :
                Instantiate( EvaluationBadPrefab , Pos , Quaternion.identity );
                nCntBad++;
            break;

            case EVALUATION.EVALUATION_FINE :
                Instantiate( EvaluationFinePrefab , Pos , Quaternion.identity );
                nCntFine++;
            break;

            case EVALUATION.EVALUATION_EXCELLENT :
                Instantiate( EvaluationExcellentPrefab , Pos , Quaternion.identity );
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

        //BGMの状態をチェック
        ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_GAME_END_CHECK );
    }
}
