using System.Collections;
using UnityEngine;
using System.Collections.Generic;


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

    float [ ]afStartArray;
    int nCnt;
    BGM BGMClass;

    public GameObject Enemy_01Prefab;
    GameObject[ ] aEnemyPrefabArray2;

    List<GameObject> TakeInEnemyList;  //追従する敵の作業用変数

   
	void Start( )
    {
        //変数の初期化
        aEnemyPrefabArray = new GameObject[ 8 , 4 ];
        aEnemyPrefabArray2 = new GameObject[ 8 ];
        TakeInEnemyList = new List<GameObject>();  
        nCreateNo         = 0;
        nCntLength        = 0;

        //敵オブジェクトを生成し、非表示にしておく
        for( int nCnt = 0; nCnt < 8; nCnt++ )
        {
           /* aEnemyPrefabArray[ nCnt , 0 ] = Instantiate( EnemyUpPrefab , new Vector3( 0.0f , 0.0f , 0.0f ) , Quaternion.identity );
            aEnemyPrefabArray[ nCnt , 1 ] = Instantiate( EnemyDownPrefab , new Vector3( 0.0f, 0.0f, 0.0f ) , Quaternion.identity );
            aEnemyPrefabArray[ nCnt , 2 ] = Instantiate( EnemyLeftPrefab , new Vector3( 0.0f , 0.0f, 0.0f ) , Quaternion.identity );
            aEnemyPrefabArray[ nCnt , 3 ] = Instantiate( EnemyRightPrefab , new Vector3( 0.0f, 0.0f , 0.0f ) , Quaternion.identity );*/

            aEnemyPrefabArray[ nCnt , 0 ] = Instantiate( EnemyUpPrefab , new Vector3( 0.0f , 0.0f , 0.0f ) , Quaternion.Euler( 0.0f , 0.0f , 0.0f ) );
            aEnemyPrefabArray[ nCnt , 1 ] = Instantiate( EnemyDownPrefab , new Vector3( 0.0f, 0.0f, 0.0f ) , Quaternion.Euler( 0.0f , 0.0f , 0.0f ) );
            aEnemyPrefabArray[ nCnt , 2 ] = Instantiate( EnemyLeftPrefab , new Vector3( 0.0f , 0.0f, 0.0f ) , Quaternion.Euler( 0.0f , 0.0f , 0.0f ) );
            aEnemyPrefabArray[ nCnt , 3 ] = Instantiate( EnemyRightPrefab , new Vector3( 0.0f, 0.0f , 0.0f ) , Quaternion.Euler( 0.0f , 0.0f , 0.0f ) );

            aEnemyPrefabArray[ nCnt , 0 ].transform.parent = this.transform;
            aEnemyPrefabArray[ nCnt , 1 ].transform.parent = this.transform;
            aEnemyPrefabArray[ nCnt , 2 ].transform.parent = this.transform;
            aEnemyPrefabArray[ nCnt , 3 ].transform.parent = this.transform;

            aEnemyPrefabArray[ nCnt , 0 ].gameObject.SetActive( false );
            aEnemyPrefabArray[ nCnt , 1 ].gameObject.SetActive( false );
            aEnemyPrefabArray[ nCnt , 2 ].gameObject.SetActive( false );
            aEnemyPrefabArray[ nCnt , 3 ].gameObject.SetActive( false );

            aEnemyPrefabArray2[ nCnt ] = Instantiate( Enemy_01Prefab , new Vector3( 0.0f, 0.0f , 0.0f ) , Quaternion.Euler( 0.0f , 180.0f , 0.0f ) );
            aEnemyPrefabArray2[ nCnt ].transform.parent = this.transform;
            aEnemyPrefabArray2[ nCnt ].gameObject.SetActive( false );
        }

        //敵情報をファイルから読み込み
        EnemyText = Resources.Load( "enemy" ) as TextAsset;

        //各クラスの情報を取得
        ManagerClass   = ManagerObj.GetComponent< Manager >( );
        CountDownClass =  ManagerClass.GetCountDown( ).GetComponent< CountDown >( );

        //プレイヤー達のオブジェクトを取得
        PlayersObj = ManagerClass.GetPlayers( );  

        afStartArray = new float[ 13 ];
      /*  afStartArray[ 0 ] = 22.21f;   //
        afStartArray[ 1 ] = 29.1f;  //
        afStartArray[ 2 ] = 37.0f;  //
        afStartArray[ 3 ] = 59.1f;  //
        afStartArray[ 4 ] = 66.482f;  //
        afStartArray[ 5 ] = 81.2f;  //
        afStartArray[ 6 ] = 88.582f;  //
        afStartArray[ 7 ] = 147.65f;  //
        afStartArray[ 8 ] = 155.1f;  //
        afStartArray[ 9 ] = 162.45f;  //
        afStartArray[ 10 ] = 169.8f;  //
        afStartArray[ 11 ] = 177.2f;  //
        afStartArray[ 12 ] = 184.6f;  //*/

        afStartArray[ 0 ] = 22.20f;   //OK!!
        afStartArray[ 1 ] = 29.597f;  //OK!!
        afStartArray[ 2 ] = 36.96f;  //OK!!
        afStartArray[ 3 ] = 59.11f;  //OK!!
        afStartArray[ 4 ] = 66.50f;  //OK!!
        afStartArray[ 5 ] = 81.28f;  //OK!!
        afStartArray[ 6 ] = 88.63f;  //OK!!
        afStartArray[ 7 ] = 147.7f;  //OK!!
        afStartArray[ 8 ] = 155.05f;  //OK!!
        afStartArray[ 9 ] = 162.45f;  //OK!!
        afStartArray[ 10 ] = 169.82f;  //OK!!
        afStartArray[ 11 ] = 177.17f;  //
        afStartArray[ 12 ] = 184.57f;  //


        nCnt = -1;
        BGMClass =  ManagerClass.GetBGM( ).GetComponent< BGM >( );
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
                aEnemyPrefabArray[ nCnt2 , 0 ].transform.position = new Vector3( LeftEndPos.x + ( fWidth * nCnt2 ) , 0.5f , fPosZ + fPlayerToEnemyDist );

                aEnemyPrefabArray2[ nCnt2 ].gameObject.SetActive( true );
                aEnemyPrefabArray2[ nCnt2 ].transform.position = new Vector3( LeftEndPos.x + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );
            }
            else if( EnemyText.text[ nCnt ] == '2' )
            {
                aEnemyPrefabArray[ nCnt2 , 1 ].gameObject.SetActive( true );
                aEnemyPrefabArray[ nCnt2 , 1 ].transform.position = new Vector3( LeftEndPos.x  + ( fWidth * nCnt2 ) , 0.5f , fPosZ + fPlayerToEnemyDist );

                aEnemyPrefabArray2[ nCnt2 ].gameObject.SetActive( true );
                aEnemyPrefabArray2[ nCnt2 ].transform.position = new Vector3( LeftEndPos.x + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );
            }
            else if( EnemyText.text[ nCnt ] == '3' )
            {
                aEnemyPrefabArray[ nCnt2 , 2 ].gameObject.SetActive( true );
                aEnemyPrefabArray[ nCnt2 , 2 ].transform.position = new Vector3( LeftEndPos.x  + ( fWidth * nCnt2 ) , 0.5f , fPosZ + fPlayerToEnemyDist );

                aEnemyPrefabArray2[ nCnt2 ].gameObject.SetActive( true );
                aEnemyPrefabArray2[ nCnt2 ].transform.position = new Vector3( LeftEndPos.x + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );
            }
            else if( EnemyText.text[ nCnt ] == '4' )
            {
                aEnemyPrefabArray[ nCnt2 , 3 ].gameObject.SetActive( true );
                aEnemyPrefabArray[ nCnt2 , 3 ].transform.position = new Vector3( LeftEndPos.x  + ( fWidth * nCnt2 ) , 0.5f , fPosZ + fPlayerToEnemyDist );

                aEnemyPrefabArray2[ nCnt2 ].gameObject.SetActive( true );
                aEnemyPrefabArray2[ nCnt2 ].transform.position = new Vector3( LeftEndPos.x + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );
            }

            //  AuraSpotObj.SetActive( false );
        }

        //文字列の添え字を進める(敵8体+改行コード2文字分)
        nCreateNo += 10;

        //カウントダウンの開始
        CountDownClass.ActiveCountDown( );
        ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_COUNT_DOWN ); 

        if( nCnt != -1 )
        {
            BGMClass.SetBGM( afStartArray[ nCnt ] );
        }
        
        nCnt++;

       // ManagerClass.ResetdCntFrame( );//??
      //  ManagerClass.SetFlg( );
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

            if( aEnemyPrefabArray2[ nCnt ].activeSelf == true )
            {
                aEnemyPrefabArray2[ nCnt ].SetActive( false );
            }
        }

         AuraSpotObj.SetActive( false );
    }


    //ターゲットを設定
    public void SetTarget( int nTargetNo )
    {
        for( int nCnt = 0; nCnt < 4; nCnt++ )
        {
            if( aEnemyPrefabArray[ nTargetNo , nCnt ].activeSelf == true )
            {
                TargetEnemy = aEnemyPrefabArray[ nTargetNo , nCnt ];
                  AuraSpotObj.SetActive( true );
                AuraSpotObj.transform.position = TargetEnemy.transform.position;
                break;
            }
            else
            {
                AuraSpotObj.SetActive( false );
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
                TakeInEnemyList.Add( Instantiate( TakeInEnemyPrefab , new Vector3( fPosX + nTmp , 0.0f , -fTakeInEnemyDist * nCntLength + fMoveZ + fLength ) , Quaternion.identity ) );

                //プレイヤー達の子要素にする
                TakeInEnemyList[ TakeInEnemyList.Count - 1 ].transform.parent = PlayersObj.GetComponent< Transform >( ).transform;

                //X座標をずらす
                fPosX += fRoadSide / ( nCreateSide - 1 );
            }

            //縦の段数を増やす
            nCntLength++;

            //生成数をカウンタ
            nCnt += nCreateSide;
        }
    }


    public void TakeInEnemyMotion( PlayerAnimDefine.Idx idx )
    {
        for( int nCnt = 0; nCnt < TakeInEnemyList.Count; nCnt++ )
        {
            TakeInEnemyList[ nCnt ].gameObject.GetComponent< PlayerAnim >( ).MotionChange( idx );
        }
    }
}
