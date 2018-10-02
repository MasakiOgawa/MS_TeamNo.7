using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject    ManagerPrefab;       //マネージャのプレハブ
    public GameObject    CountDownPrefab;     //カウントダウンのプレハブ
           TextAsset     EnemyText;           //敵情報が格納されたファイルの情報
    public GameObject    EnemyUp;             //上方向の敵のプレハブ
    public GameObject    EnemyDown;           //下方向の敵のプレハブ
    public GameObject    EnemyRight;          //右方向の敵のプレハブ
    public GameObject    EnemyLeft;           //左方向の敵のプレハブ
    public GameObject    TakeInEnemyPrefab;   //追従する敵のプレハブ
           GameObject[ ] EnemyPrefabTmp;      //生成した敵情報を保存
           GameObject    TargetEnemy;         //現在ターゲットにされている敵情報
    public Vector2       LeftEndPos;          //左端の敵の座標
    public float         fDist;               //敵同士の距離
           int           nCreateNo;           //ファイルの文字列の添え字


     int nCreateSide = 0;   //横の生成数
        float fRoadSide   = 16.0f;  //道の幅
        int nCntLength  = 0;   //生成した段の数
        float fPosX = 0.0f;
        float fhaba = 1.5f;
   

	void Start( )
    {
        //変数の初期化
        EnemyPrefabTmp = new GameObject[ 8 ];
        nCreateNo = 0;

        for( int nCnt = 0; nCnt < 8; nCnt++ )
        {
            EnemyPrefabTmp[ nCnt ] = null;
        }

        //敵情報をファイルから読み込み
        EnemyText = Resources.Load( "enemy" ) as TextAsset;
	}


    //敵の生成
    public void Create( )
    {  
        //各方向の敵を生成
        for( int nCnt = nCreateNo , nCnt2 = 0; nCnt < nCreateNo + 8; nCnt++ , nCnt2++ )
        {
            if( EnemyText.text[ nCnt ] == '1' )
            {
                EnemyPrefabTmp[ nCnt2 ] = Instantiate( EnemyUp , new Vector3( LeftEndPos.x - ( fDist * nCnt2 ), LeftEndPos.y , 0.0f ) , Quaternion.identity );
            }
            else if( EnemyText.text[ nCnt ] == '2' )
            {
                EnemyPrefabTmp[ nCnt2 ] = Instantiate( EnemyDown , new Vector3( LeftEndPos.x - ( fDist * nCnt2 ) , LeftEndPos.y , 0.0f ) , Quaternion.identity );
            }
            else if( EnemyText.text[ nCnt ] == '3' )
            {
                EnemyPrefabTmp[ nCnt2 ] = Instantiate( EnemyLeft , new Vector3( LeftEndPos.x - ( fDist * nCnt2 ) , LeftEndPos.y , 0.0f ) , Quaternion.identity );
            }
            else if( EnemyText.text[ nCnt ] == '4' )
            {
                EnemyPrefabTmp[ nCnt2 ] = Instantiate( EnemyRight , new Vector3( LeftEndPos.x - ( fDist * nCnt2 ) , LeftEndPos.y , 0.0f ) , Quaternion.identity );
            }
        }

        //文字列の添え字を進める(敵8体+改行コード2文字分)
        nCreateNo += 10;

        //カウントダウンの開始
        ManagerPrefab.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_COUNT_DOWN );
    }

    
    //敵の破棄
    public void Kill( )
    {
        for( int nCnt = 0; nCnt < 8; nCnt++ )
        {
            if( EnemyPrefabTmp[ nCnt ] != null )
            {
                Destroy( EnemyPrefabTmp[ nCnt ].gameObject );
                EnemyPrefabTmp[ nCnt ] = null;
            }
        }

        //次の敵を生成
        ManagerPrefab.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
        CountDownPrefab.GetComponent< CountDown >( ).SetText( );
    }


    //ターゲットを設定
    public void SetTarget( int nTarget )
    {
        TargetEnemy = EnemyPrefabTmp[ nTarget ];
    }


    //ターゲットを取得
    public GameObject GetTarget( )
    {
        return TargetEnemy;
    }


    //敵をプレイヤーの後ろに生成
    public void TakeIn( int nEvaluation )
    {
        //敵の数までループ
        for( int nCnt = 0; nCnt < nEvaluation; )
        {
            //横に生成する数を決める(最大7体)
            do
            {
                nCreateSide = ( int )Random.Range( 4, 8 ); 
            }while( nCreateSide == 0 );

            //左端のX座標を決める
            fPosX = -( fRoadSide * 0.5f );

            //敵を横に生成する
            for( int nCnt2 = 0; nCnt2 < nCreateSide; nCnt2++ )
            {
                Instantiate( TakeInEnemyPrefab , new Vector3( fPosX , -2.0f , 0.0f + fhaba * ( nCntLength + 1 ) ) , Quaternion.identity );

                //X座標をずらす
                fPosX += fRoadSide / ( nCreateSide - 1 );
            }

            //縦の段数をふやす
            nCntLength++;

            //生成数をカウンタ
            nCnt += nCreateSide;
        }
    }
}
