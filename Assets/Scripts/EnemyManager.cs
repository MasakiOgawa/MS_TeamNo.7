using System.Collections;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
           Manager         ManagerClass;         //マネージャのクラス
    public GameObject      ManagerObj;           //マネージャのオブジェクト
    public GameObject      EnemyUpPrefab;        //上方向の敵のプレハブ
    public GameObject      EnemyDownPrefab;      //下方向の敵のプレハブ
    public GameObject      EnemyRightPrefab;     //右方向の敵のプレハブ
    public GameObject      EnemyLeftPrefab;      //左方向の敵のプレハブ
    public GameObject      TakeInEnemyPrefab;    //追従する敵のプレハブ
           GameObject[ , ] aEnemyPrefabArray;    //生成した敵のオブジェクト配列
           GameObject      TargetEnemy;          //現在ターゲットにされている敵のオブジェクト
           TextAsset       EnemyText;            //敵情報が格納されたファイルの情報
    public float           fRoadSide;            //道の幅
    public Vector2         LeftEndPos;           //左端の敵の座標
    public float           fPlayerToEnemyDist;   //プレイヤーと敵の距離
    public float           fWidth;               //追従する敵同士の横幅
    public float           fLength;              //追従する敵同士の縦幅
    public float           fTakeInEnemyDist;     //追従する敵同士の距離
    public int             nTakeInEnemyRange;    //敵同士の距離に乱数を追加
           int             nCreateNo;            //アクティブ化する敵の番号
           int             nCntLength;           //生成した段の数
           GameObject      PlayersObj;           //プレイヤー達のオブジェクト
           CountDown       CountDownClass;       //カウントダウンのクラス
    public GameObject      AuraSpotObj;

   
	void Start( )
    {
        //変数の初期化
        aEnemyPrefabArray = new GameObject[ 8 , 4 ];
        nCreateNo         = 0;
        nCntLength        = 0;

        //敵オブジェクトを生成し、非表示にしておく
        for( int nCnt = 0; nCnt < 8; nCnt++ )
        {
            aEnemyPrefabArray[ nCnt , 0 ] = Instantiate( EnemyUpPrefab , new Vector3( 0.0f , 0.0f , 0.0f ) , Quaternion.identity );
            aEnemyPrefabArray[ nCnt , 1 ] = Instantiate( EnemyDownPrefab , new Vector3( 0.0f, 0.0f, 0.0f ) , Quaternion.identity );
            aEnemyPrefabArray[ nCnt , 2 ] = Instantiate( EnemyLeftPrefab , new Vector3( 0.0f , 0.0f, 0.0f ) , Quaternion.identity );
            aEnemyPrefabArray[ nCnt , 3 ] = Instantiate( EnemyRightPrefab , new Vector3( 0.0f, 0.0f , 0.0f ) , Quaternion.identity );

            aEnemyPrefabArray[ nCnt , 0 ].gameObject.SetActive( false );
            aEnemyPrefabArray[ nCnt , 1 ].gameObject.SetActive( false );
            aEnemyPrefabArray[ nCnt , 2 ].gameObject.SetActive( false );
            aEnemyPrefabArray[ nCnt , 3 ].gameObject.SetActive( false );
        }

        //敵情報をファイルから読み込み
        EnemyText = Resources.Load( "enemy" ) as TextAsset;

        //各クラスの情報を取得
        ManagerClass   = ManagerObj.GetComponent< Manager >( );
        CountDownClass =  ManagerClass.GetCountDown( ).GetComponent< CountDown >( );

        //プレイヤー達のオブジェクトを取得
        PlayersObj = ManagerClass.GetPlayers( );  
	}


    //敵のアクティブ化
    public void ActiveTrue( )
    {  
        float fPosZ = PlayersObj.transform.position.z;

        for( int nCnt = nCreateNo , nCnt2 = 0; nCnt < nCreateNo + 8; nCnt++ , nCnt2++ )
        {
            if( EnemyText.text[ nCnt ] == '1' )
            {
                aEnemyPrefabArray[ nCnt2 , 0 ].gameObject.SetActive( true );
                aEnemyPrefabArray[ nCnt2 , 0 ].transform.position = new Vector3( LeftEndPos.x + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );
            }
            else if( EnemyText.text[ nCnt ] == '2' )
            {
                aEnemyPrefabArray[ nCnt2 , 1 ].gameObject.SetActive( true );
                aEnemyPrefabArray[ nCnt2 , 1 ].transform.position = new Vector3( LeftEndPos.x  + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );
            }
            else if( EnemyText.text[ nCnt ] == '3' )
            {
                aEnemyPrefabArray[ nCnt2 , 2 ].gameObject.SetActive( true );
                aEnemyPrefabArray[ nCnt2 , 2 ].transform.position = new Vector3( LeftEndPos.x  + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );
            }
            else if( EnemyText.text[ nCnt ] == '4' )
            {
               aEnemyPrefabArray[ nCnt2 , 3 ].gameObject.SetActive( true );
               aEnemyPrefabArray[ nCnt2 , 3 ].transform.position = new Vector3( LeftEndPos.x  + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );
            }
        }

        //文字列の添え字を進める(敵8体+改行コード2文字分)
        nCreateNo += 10;

        //カウントダウンの開始
        CountDownClass.ActiveCountDown( );
        ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_COUNT_DOWN ); 

      //  ManagerClass.ResetfCntFrame( );//??
    }

    
    //敵の非アクティブ化
    public void ActiveFalse( )
    {
        for( int nCnt = 0; nCnt < 8; nCnt++ )
        {
            for( int nCnt2 = 0; nCnt2 < 4; nCnt2++ )
            {   
                if( aEnemyPrefabArray[ nCnt , nCnt2 ].activeSelf == true )
                {
                    aEnemyPrefabArray[ nCnt , nCnt2 ].SetActive( false );
                }
            }
        }
    }


    //ターゲットを設定
    public void SetTarget( int nTargetNo )
    {
        for( int nCnt = 0; nCnt < 4; nCnt++ )
        {
            if( aEnemyPrefabArray[ nTargetNo , nCnt ].activeSelf == true )
            {
                TargetEnemy = aEnemyPrefabArray[ nTargetNo , nCnt ];
                break;
            }
            else
            {
                TargetEnemy = null;   
            }
        }
    }


    //ターゲットを取得
    public GameObject GetTarget( )
    {
        if( TargetEnemy != null )
        {
            return TargetEnemy;
        }
        else
        {
            return null;
        }
    }


    //評価を基に敵をプレイヤーの後ろに生成
    public void TakeIn( int nEvaluation )
    {
        GameObject TmpObj;        //オブジェクトの作業用変数
        int        nTmp;          //作業用変数
        int        nCreateSide;   //横の生成数
        float      fPosX;         //敵のX座標

        //プレイヤーの移動距離を取得
        float fMoveZ = ManagerClass.GetPlayerManager( ).GetComponent< PlayerManager >( ).fDist;

        //敵の生成数までループ
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
                nTmp   = ( int )Random.Range( 0 , nTakeInEnemyRange );
                nTmp  -= ( nTakeInEnemyRange - 1 ) / 2;
                TmpObj = Instantiate( TakeInEnemyPrefab , new Vector3( fPosX + nTmp , 4.0f , -fTakeInEnemyDist * nCntLength + fMoveZ + fLength ) , Quaternion.identity );

                //プレイヤー達の子要素にする
                TmpObj.transform.parent = PlayersObj.GetComponent< Transform >( ).transform;

                //X座標をずらす
                fPosX += fRoadSide / ( nCreateSide - 1 );
            }

            //縦の段数を増やす
            nCntLength++;

            //生成数をカウンタ
            nCnt += nCreateSide;
        }
    }
}
