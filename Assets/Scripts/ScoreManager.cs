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

    public GameObject GameManagerPrefab;    //ゲームマネージャのプレハブ
    public GameObject EnemyManagerPrefab;   //エネミーマネージャのプレハブ


	void Start( )
    {
        //変数の初期化
		nCntBad       = 0;
        nCntFine      = 0;
        nCntExcellent = 0;
	}
	
	
	void Update( )
    {
		
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


    //スコアの加算
    public void AddScore( )
    {
        Score.nScore += ( nCntExcellent * 3 ) + ( nCntFine * 2 ) + ( nCntBad * 0 );

        //敵を追従させる
        EnemyManagerPrefab.GetComponent< EnemyManager >( ).TakeIn( ( nCntExcellent * 3 ) + ( nCntFine * 2 ) + ( nCntBad * 0 ) );

        //現在の敵を破棄
        EnemyManagerPrefab.GetComponent< EnemyManager >( ).Kill( );

        //各評価のリセット
        nCntBad       = 0;
        nCntFine      = 0;
        nCntExcellent = 0;

        //次の敵を生成
        GameManagerPrefab.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
    }
}
