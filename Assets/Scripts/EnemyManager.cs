using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject       ManagerObj;            //マネージャのオブジェクト
           GameObject       PlayersObj;            //プレイヤー達のオブジェクト
    public GameObject       EnemyUpPrefab;         //上方向の敵のプレハブ
    public GameObject       EnemyDownPrefab;       //下方向の敵のプレハブ
    public GameObject       EnemyRightPrefab;      //右方向の敵のプレハブ
    public GameObject       EnemyLeftPrefab;       //左方向の敵のプレハブ
    public GameObject       TakeInEnemyPrefab;     //追従する敵のプレハブ
           TextAsset        EnemyText;             //敵情報が格納されたファイルの情報
           GameObject[ , ]  EnemyPrefabTmp;        //生成した敵のオブジェクト保存用配列
           GameObject       TargetEnemy;           //現在ターゲットにされている敵のオブジェクト
    public float            fRoadSide;             //道の幅16
    public float            fPlayerToEnemyDist;    //プレイヤーと敵の距離11
    public Vector2          LeftEndPos;            //左端の敵の座標
    public float            fWidth;                //敵同士の横幅2.2??
    public float            fLength;               //敵同士の縦幅13
    public float            fTakeInEnemyDist;      //追従する敵同士の距離1.5
    public int              nTakeInEnemyRange;     //敵同士の距離に乱数を追加
           int              nCreateNo;             //生成する敵の番号
           int              nCntLength;            //生成した段の数


    
  

	void Start( )
    {
        //変数の初期化
        EnemyPrefabTmp = new GameObject[ 8 , 4 ];
        nCreateNo      = 0;
        nCntLength     = 0;

        //敵オブジェクトを生成し、非表示にしておく
        for( int nCnt = 0; nCnt < 8; nCnt++ )
        {
            EnemyPrefabTmp[ nCnt , 0 ] = Instantiate( EnemyUpPrefab , new Vector3( 0.0f , 0.0f , 0.0f ) , Quaternion.identity );
            EnemyPrefabTmp[ nCnt , 1 ] = Instantiate( EnemyDownPrefab , new Vector3( 0.0f, 0.0f, 0.0f ) , Quaternion.identity );
            EnemyPrefabTmp[ nCnt , 2 ] = Instantiate( EnemyLeftPrefab , new Vector3( 0.0f , 0.0f, 0.0f ) , Quaternion.identity );
            EnemyPrefabTmp[ nCnt , 3 ] = Instantiate( EnemyRightPrefab , new Vector3( 0.0f, 0.0f , 0.0f ) , Quaternion.identity );

            EnemyPrefabTmp[ nCnt , 0 ].gameObject.SetActive( false );
            EnemyPrefabTmp[ nCnt , 1 ].gameObject.SetActive( false );
            EnemyPrefabTmp[ nCnt , 2 ].gameObject.SetActive( false );
            EnemyPrefabTmp[ nCnt , 3 ].gameObject.SetActive( false );
        }

        //敵情報をファイルから読み込み
        EnemyText = Resources.Load( "enemy" ) as TextAsset;

        //プレイヤー達のオブジェクトを取得
        PlayersObj = ManagerObj.GetComponent< Manager >( ).GetPlayers( );
	}


    //敵の生成
    public void Create( )
    {  
        float fPosZ = PlayersObj.transform.position.z;

        for( int nCnt = nCreateNo , nCnt2 = 0; nCnt < nCreateNo + 8; nCnt++ , nCnt2++ )
        {
            if( EnemyText.text[ nCnt ] == '1' )
            {
                EnemyPrefabTmp[ nCnt2 , 0 ].gameObject.SetActive( true );
                EnemyPrefabTmp[ nCnt2 , 0 ].transform.position = new Vector3( LeftEndPos.x + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );
            }
            else if( EnemyText.text[ nCnt ] == '2' )
            {
                EnemyPrefabTmp[ nCnt2 , 1 ].gameObject.SetActive( true );
                EnemyPrefabTmp[ nCnt2 , 1 ].transform.position = new Vector3( LeftEndPos.x  + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );
            }
            else if( EnemyText.text[ nCnt ] == '3' )
            {
                EnemyPrefabTmp[ nCnt2 , 2 ].gameObject.SetActive( true );
                EnemyPrefabTmp[ nCnt2 , 2 ].transform.position = new Vector3( LeftEndPos.x  + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );
            }
            else if( EnemyText.text[ nCnt ] == '4' )
            {
               EnemyPrefabTmp[ nCnt2 , 3 ].gameObject.SetActive( true );
               EnemyPrefabTmp[ nCnt2 , 3 ].transform.position = new Vector3( LeftEndPos.x  + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );
            }
            else
            {

            }
        }

        //文字列の添え字を進める(敵8体+改行コード2文字分)
        nCreateNo += 10;

        //カウントダウンの開始
        ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_COUNT_DOWN ); 
    }

    
    //敵の破棄
    public void Kill( )
    {
        for( int nCnt = 0; nCnt < 8; nCnt++ )
        {
            for( int nCnt2 = 0; nCnt2 < 4; nCnt2++ )
            {   
                if( EnemyPrefabTmp[ nCnt , nCnt2 ].activeSelf == true )
                {
                    EnemyPrefabTmp[ nCnt , nCnt2 ].SetActive( false );
                }
            }
        }
    }


    //ターゲットを設定
    public void SetTarget( int nTarget )
    {
        for( int nCnt = 0; nCnt < 4; nCnt++ )
        {
            if( EnemyPrefabTmp[ nTarget , nCnt ].activeSelf == true )
            {
                TargetEnemy = EnemyPrefabTmp[ nTarget , nCnt ];

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
        float fMoveZ = ManagerObj.GetComponent< Manager >( ).GetPlayerManager( ).GetComponent< PlayerManager >( ).fDist;

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
                nTmp = ( int )Random.Range( 0 , nTakeInEnemyRange );
                nTmp -= ( nTakeInEnemyRange - 1 ) / 2;
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
