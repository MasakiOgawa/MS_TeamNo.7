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


	void Start( )
    {
		
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
            break;

            case EVALUATION.EVALUATION_FINE :
                Instantiate( EvaluationFinePrefab , Pos , Quaternion.identity );
            break;

            case EVALUATION.EVALUATION_EXCELLENT :
                Instantiate( EvaluationExcellentPrefab , Pos , Quaternion.identity );
            break;
        }
    }
}
