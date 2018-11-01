using System.Collections;
using UnityEngine;
using Uniduino;


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
    public int                nScore;                    //スコア

    public SerialHandler SerialHandlerClass;
    public Arduino ArdiunoClass;
    bool bFlg;
    float fCntFrame;


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

        ArdiunoClass = Arduino.global;
        ArdiunoClass.Setup( ConfigurePins );
        bFlg = false;
        fCntFrame = 0.0f;
	}


    void ConfigurePins( )
    {
        ArdiunoClass.pinMode( 6 , PinMode.OUTPUT );
    }


    void Update( )
    {
        if ( bFlg == true )
        {
            fCntFrame += Time.deltaTime;

            if( fCntFrame >= 3.0f )
            {
                bFlg = false;
                fCntFrame = 0.0f;
                SerialHandlerClass.Write( "3" );
            }
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
                         aEvaluationBadArray[ nCnt ].gameObject.transform.position = new Vector3( Pos.x , Pos.y + 4.0f , Pos.z + 3.0f );    
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
                          aEvaluationFineArray[ nCnt ].gameObject.transform.position = new Vector3( Pos.x , Pos.y + 4.0f , Pos.z + 3.0f );
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
                        aEvaluationExcellentArray[ nCnt ].gameObject.transform.position = new Vector3( Pos.x , Pos.y + 4.0f , Pos.z + 3.0f );

                        if( nCnt == 2 && bFlg == false )
                        {
                            bFlg = true;
                            SerialHandlerClass.Write( "4" );
                        }
                      
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
        nScore += ( nCntExcellent * 2 ) + ( nCntFine * 1 );

        //敵を追従させる
        EnemyManagerClass.TakeIn( ( nCntExcellent * 2 ) + ( nCntFine * 1 ) );

        //各評価のリセット
        nCntBad       = 0;
        nCntFine      = 0;
        nCntExcellent = 0;

        //現在の敵を非アクティブ化
        EnemyManagerClass.ActiveFalse( );

        //遷移先をチェック
        ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_CHECK );
    }


    //
    public int GetnScore( )
    {
        return nScore;
    }

    public int ResetnScore( )
    {
        return nScore = 0;
    }
}
