﻿using System.Collections;
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

    public GameObject [ ]Enemy_Prefab;
    GameObject[ ] aEnemyPrefabArray2;
    GameObject[ ] aEnemyPrefabArray3;

    List<GameObject> TakeInEnemyList;  //追従する敵の作業用変数
    List<PlayerAnim> PlayerAnimList; 
    List<Animator>   AnimatorList;
    List<float>      PosXList;

    public float fAddRotateY;
    float fRotY;

    public Canvas CanvasObj;
    public Vector2 LeftEndPosCanvas;           //左端の敵の座標(キャンバス)
    public float  fWidthCanvas;

    int nTargetNoTmp;

    public enum EnemyPhase
    {
        ENEMY_DIST = 0 ,
        ENEMY_STOP ,
        ENEMY_JUMP ,
        ENEMY_ANGRY
    };

    EnemyPhase[ ] aEnemyPhase;
    float[ ] fCntFrame;
     Animator[ ] aAnimator;

    public GameObject ShakeUIPrefab;
    shakeUICanvas shakeUICanvasClass;

    public GameObject GetEnemyPrefab;
    EnemyGetEffect GetEnemyClass;

   
	void Start( )
    {
        //変数の初期化
        aEnemyPrefabArray = new GameObject[ 8 , 4 ];
        aEnemyPrefabArray2 = new GameObject[ 12 ];
        aEnemyPrefabArray3 = new GameObject[ 8 ];
        TakeInEnemyList = new List<GameObject>();  
        PlayerAnimList  = new List<PlayerAnim>();
        AnimatorList = new List<Animator>();
        PosXList = new List<float>();
        nCreateNo         = 0;
        nCntLength        = 0;
         fRotY = 0.0f;

        nTargetNoTmp = 0;
         aAnimator = new Animator[ 12 ];

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
        }

        for( int nCnt = 0; nCnt < 12; nCnt++ )
        {
            aEnemyPrefabArray2[ nCnt ] = Instantiate( Enemy_Prefab[ nCnt ] , new Vector3( 0.0f, 0.0f , 0.0f ) , Quaternion.Euler( 0.0f , 180.0f , 0.0f ) );
            aEnemyPrefabArray2[ nCnt ].transform.parent = this.transform;

            aAnimator[ nCnt ] = aEnemyPrefabArray2[ nCnt ].gameObject.GetComponent< Animator >( );
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
        afStartArray[ 7 ] = 147.72f;  //
        afStartArray[ 8 ] = 155.117f;  //
        afStartArray[ 9 ] = 162.514f;  //
        afStartArray[ 10 ] = 169.911f;  //
        afStartArray[ 11 ] = 177.25f;  //
        afStartArray[ 12 ] = 184.647f;  //


        nCnt = -1;
        BGMClass =  ManagerClass.GetBGM( ).GetComponent< BGM >( );

        //モーション周り
        aEnemyPhase = new EnemyPhase[ 12 ];
        fCntFrame = new float[ 12 ];

        for( int nCnt = 0; nCnt < 12; nCnt++ )
        {
            aEnemyPhase[ nCnt ] = EnemyPhase.ENEMY_DIST;
            fCntFrame[ nCnt ] = 0.0f;
        }

        shakeUICanvasClass = ShakeUIPrefab.GetComponent< shakeUICanvas >( );

        GetEnemyClass = GetEnemyPrefab.GetComponent< EnemyGetEffect >( );
	}

    void Update( )
    {
        /* if( bFlg == false )
         {
             for( int nCnt = 0; nCnt < 12; nCnt++ )
             {
                 if( aEnemyPrefabArray2[ nCnt ].gameObject.activeSelf == true )
                 {
                     if( aEnemyPrefabArray2[ nCnt ].gameObject.GetComponent< Animator >( ).GetCurrentAnimatorStateInfo( 0 ).normalizedTime >= 1.0f && bFlg == false )
                     {
                         bFlg = true;
                     }
                 }
             }
         }


         if( bFlg == true )
         {
             fCntFrame += Time.deltaTime;

             if( fCntFrame >= 0.5f )
             {
                 bFlg = false;
                 fCntFrame = 0.0f;

                 for( int nCnt = 0; nCnt < 12; nCnt++ )
                 {
                     if( aEnemyPrefabArray2[ nCnt ].activeSelf == true )
                     {
                         aEnemyPrefabArray2[ nCnt ].gameObject.GetComponent< Animator >( ).ForceStateNormalizedTime( 0.0f );
                     }
                 }
             }
         }*/

        for( int nCnt = 0; nCnt < 12; nCnt++)
        {
            //敵が生成されていたら
            if( aEnemyPrefabArray2[ nCnt ].gameObject.activeSelf == true )
            {
                //矢印モーションでなければ
                if( aAnimator[ nCnt ].GetCurrentAnimatorStateInfo( 0 ).nameHash != Animator.StringToHash("Base Layer.VictoryIdle") &&
                    aAnimator[ nCnt ].GetCurrentAnimatorStateInfo( 0 ).nameHash != Animator.StringToHash("Base Layer.Angry"))
                {
                    //矢印モーションが終了していたらフレームのカウント
                    if( aAnimator[ nCnt ].GetCurrentAnimatorStateInfo( 0 ).normalizedTime >= 1.0f )
                    {
                        aEnemyPhase[ nCnt ] = EnemyPhase.ENEMY_STOP;
                    }

                    //一定間隔で矢印モーションの再開
                    if( aEnemyPhase[ nCnt ] == EnemyPhase.ENEMY_STOP )
                    {
                        fCntFrame[ nCnt ] += Time.deltaTime;

                        if( fCntFrame[ nCnt ] >= 0.5f )
                        {
                            fCntFrame[ nCnt ] = 0.0f;
                            aEnemyPhase[ nCnt ] = EnemyPhase.ENEMY_DIST;
                            aAnimator[ nCnt ].ForceStateNormalizedTime( 0.0f );
                        }
                    }
                } 
            }
        }
    }


    //敵のアクティブ化
    public void ActiveTrue( )
    {  
        float fPosZ = PlayersObj.transform.position.z;
        int [ ] nArray =  new int[ 12 ];
        int nRand = 0;

        for( int nCnt = 0; nCnt < 12; nCnt++ )
        {
            nArray[ nCnt ] = 0;
        }

        for( int nCnt = nCreateNo , nCnt2 = 0; nCnt < nCreateNo + 8; nCnt++ , nCnt2++ )
        {
            if( EnemyText.text[ nCnt ] == '1' )
            {
                shakeUICanvasClass.CreateShakeUI( shakeUICanvas.SHAKE_TYPE.UP , nCnt2 );

                do
                {
                    nRand = Random.Range( 0 , 12 );
                }while( nArray[ nRand ] != 0 );
                nArray[ nRand ] = 999;

                aEnemyPrefabArray[ nCnt2 , 0 ].gameObject.SetActive( true );   
                aEnemyPrefabArray[ nCnt2 , 0 ].transform.position = new Vector3( LeftEndPos.x + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );

                aEnemyPrefabArray2[ nRand ].gameObject.SetActive( true );
               
                aEnemyPrefabArray2[ nRand ].transform.position = new Vector3( LeftEndPos.x + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );
                aEnemyPrefabArray2[ nRand ].transform.rotation = Quaternion.Euler( 0.0f , 180.0f , 0.0f );
                aEnemyPrefabArray2[ nRand ].gameObject.GetComponent< PlayerAnim >( ).MotionChange( PlayerAnimDefine.Idx.Up );

                aEnemyPrefabArray3[ nCnt2 ] = aEnemyPrefabArray2[ nRand ];

                SwapEffect.Create( SwapEffect.SWAP_TYPE.TYPE_UP , aEnemyPrefabArray2[ nRand ].transform.position );
            }
            else if( EnemyText.text[ nCnt ] == '2' )
            {
                shakeUICanvasClass.CreateShakeUI( shakeUICanvas.SHAKE_TYPE.DOWN , nCnt2 );

                do
                {
                    nRand = Random.Range( 0 , 12 );
                }while( nArray[ nRand ] != 0 );
                nArray[ nRand ] = 999;

                aEnemyPrefabArray[ nCnt2 , 1 ].gameObject.SetActive( true );
                aEnemyPrefabArray[ nCnt2 , 1 ].transform.position = new Vector3( LeftEndPos.x + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );

                aEnemyPrefabArray2[ nRand ].gameObject.SetActive( true );
                aEnemyPrefabArray2[ nRand ].transform.position = new Vector3( LeftEndPos.x + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );
                aEnemyPrefabArray2[ nRand ].transform.rotation = Quaternion.Euler( 0.0f , 180.0f , 0.0f );
                aEnemyPrefabArray2[ nRand ].gameObject.GetComponent< PlayerAnim >( ).MotionChange( PlayerAnimDefine.Idx.Down );

                aEnemyPrefabArray3[ nCnt2 ] = aEnemyPrefabArray2[ nRand ];

                SwapEffect.Create( SwapEffect.SWAP_TYPE.TYPE_DOWN , aEnemyPrefabArray2[ nRand ].transform.position );
            }
            else if( EnemyText.text[ nCnt ] == '3' )
            {
                shakeUICanvasClass.CreateShakeUI( shakeUICanvas.SHAKE_TYPE.LEFT , nCnt2 );

                do
                {
                    nRand = Random.Range( 0 , 12 );
                }while( nArray[ nRand ] != 0 );
                nArray[ nRand ] = 999;

                aEnemyPrefabArray[ nCnt2 , 2 ].gameObject.SetActive( true );
                aEnemyPrefabArray[ nCnt2 , 2 ].transform.position = new Vector3( LeftEndPos.x + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );

                aEnemyPrefabArray2[ nRand ].gameObject.SetActive( true );
                aEnemyPrefabArray2[ nRand ].transform.position = new Vector3( LeftEndPos.x + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );
                aEnemyPrefabArray2[ nRand ].transform.rotation = Quaternion.Euler( 0.0f , 180.0f , 0.0f );
                aEnemyPrefabArray2[ nRand ].gameObject.GetComponent< PlayerAnim >( ).MotionChange( PlayerAnimDefine.Idx.Right );

                aEnemyPrefabArray3[ nCnt2 ] = aEnemyPrefabArray2[ nRand ];

                SwapEffect.Create( SwapEffect.SWAP_TYPE.TYPE_LEFT , aEnemyPrefabArray2[ nRand ].transform.position );
            }
            else if( EnemyText.text[ nCnt ] == '4' )
            {
                shakeUICanvasClass.CreateShakeUI( shakeUICanvas.SHAKE_TYPE.RIGHT , nCnt2 );

                 do
                {
                    nRand = Random.Range( 0 , 12 );
                }while( nArray[ nRand ] != 0 );
                nArray[ nRand ] = 999;

                aEnemyPrefabArray[ nCnt2 , 3 ].gameObject.SetActive( true );
                aEnemyPrefabArray[ nCnt2 , 3 ].transform.position = new Vector3( LeftEndPos.x + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );

                aEnemyPrefabArray2[ nRand ].gameObject.SetActive( true );
                aEnemyPrefabArray2[ nRand ].transform.position = new Vector3( LeftEndPos.x + ( fWidth * nCnt2 ) , LeftEndPos.y , fPosZ + fPlayerToEnemyDist );
                aEnemyPrefabArray2[ nRand ].transform.rotation = Quaternion.Euler( 0.0f , 180.0f , 0.0f );
                aEnemyPrefabArray2[ nRand ].gameObject.GetComponent< PlayerAnim >( ).MotionChange( PlayerAnimDefine.Idx.Left );

                aEnemyPrefabArray3[ nCnt2 ] = aEnemyPrefabArray2[ nRand ];

                SwapEffect.Create( SwapEffect.SWAP_TYPE.TYPE_RIGHT , aEnemyPrefabArray2[ nRand ].transform.position );
            }
            else
            {
                aEnemyPrefabArray3[ nCnt2 ] = null;
            }
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
        }

        shakeUICanvasClass.ResetShakeUI( );
        AuraSpotObj.SetActive( false );

        for( int nCnt = 0; nCnt < 12; nCnt++ )
        {
             if( aEnemyPrefabArray2[ nCnt ].activeSelf == true )
            {
                if (aAnimator[nCnt].GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.VictoryIdle"))
                {
                   GetEnemyClass.SetEnemyEffect( nCnt , aEnemyPrefabArray2[ nCnt ].transform.position , true );
                }
                else
                {
                    GetEnemyClass.SetEnemyEffect( nCnt , aEnemyPrefabArray2[ nCnt ].transform.position , false );
                }

                aEnemyPrefabArray2[ nCnt ].SetActive( false );
            }
        }

        for( int nCnt = 0; nCnt < 8; nCnt++ )
        {
            aEnemyPrefabArray3[ nCnt ] = null;
        }

        //モーション周り
        for( int nCnt = 0; nCnt < 12; nCnt++ )
        {
            aEnemyPhase[ nCnt ] = EnemyPhase.ENEMY_DIST;
            fCntFrame[ nCnt ] = 0.0f;
            aAnimator[ nCnt ].applyRootMotion = false;
        }
    }


    //ターゲットを設定
    public void SetTarget( int nTargetNo )
    {
        nTargetNoTmp = nTargetNo;
        int nCnt2 = 0;

        for( int nCnt = 0; nCnt < 4; nCnt++ )
        {
            if( aEnemyPrefabArray[ nTargetNo , nCnt ].activeSelf == true )
            {
                TargetEnemy = aEnemyPrefabArray[ nTargetNo , nCnt ];
                
                AuraSpotObj.transform.position = new Vector3( TargetEnemy.transform.position.x , TargetEnemy.transform.position.y + 5.0f , TargetEnemy.transform.position.z );
                AuraSpotObj.SetActive( true );
                break;
            }
            else
            { 
                nCnt2++;
                TargetEnemy = null;   
            }
        }

        //一つもついていなければ
        if( nCnt2 == 4 )
        {
            //場所を指定
            if( nTargetNo == 0 )
            {
                AuraSpotObj.transform.position = new Vector3( -5.43f , 5.0f , PlayersObj.transform.position.z + fPlayerToEnemyDist );
            }
            else
            {
                AuraSpotObj.transform.position += new Vector3( 1.6f , 0.0f , 0.0f );
            }

            AuraSpotObj.SetActive( true );
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


    public GameObject GetBeatTarget( )
    {
        if( aEnemyPrefabArray3[ nTargetNoTmp ] != null )
        {
            return aEnemyPrefabArray3[ nTargetNoTmp ] ;
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
        nEvaluation /= 2;   //適当
        //プレイヤーの移動距離を取得
        float fMoveZ = ManagerClass.GetPlayerManager( ).GetComponent< PlayerManager >( ).fDist;

        //敵の生成数までループ
        for( int nCnt = 0; nCnt < nEvaluation; )
        {
            //横に生成する数を決める(最大7体)
           // do
           // {
                nCreateSide = ( int )Random.Range( 3, 5 ); 
         //   }while( nCreateSide == 0 );

            //左端のX座標を決める
            fPosX = -( fRoadSide * 0.5f );

            //敵を横に生成する
            for( int nCnt2 = 0; nCnt2 < nCreateSide; nCnt2++ )
            {
                nTmp   = ( int )Random.Range( 2 , nTakeInEnemyRange );
                nTmp  -= ( nTakeInEnemyRange - 1 ) / 2;
                TakeInEnemyList.Add( Instantiate( Enemy_Prefab[ Random.Range( 0 , 12 ) ] , new Vector3( fPosX + nTmp , 0.0f , -fTakeInEnemyDist * nCntLength + fMoveZ + fLength ) , Quaternion.identity ) );
                PlayerAnimList.Add( TakeInEnemyList[ TakeInEnemyList.Count - 1 ].GetComponent< PlayerAnim >());
                AnimatorList.Add( TakeInEnemyList[ TakeInEnemyList.Count - 1 ].GetComponent<Animator>());
                PosXList.Add( TakeInEnemyList[ TakeInEnemyList.Count - 1 ].transform.position.x );

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
            TakeInEnemyList[ nCnt ].gameObject.transform.position = new Vector3( PosXList[ nCnt ] , 0.0f , TakeInEnemyList[ nCnt ].gameObject.transform.position.z);
            TakeInEnemyList[ nCnt ].gameObject.transform.rotation = Quaternion.Euler( 0.0f , 0.0f , 0.0f );
            PlayerAnimList[ nCnt ].MotionChange( idx );
        }
    }


    public void  ApplyFalse( )
    {
        for( int nCnt = 0; nCnt < TakeInEnemyList.Count; nCnt++ )
        {
            AnimatorList[ nCnt ].applyRootMotion = false;
        }
    }


    public void  ApplyTrue( )
    {
        for( int nCnt = 0; nCnt < TakeInEnemyList.Count; nCnt++ )
        {
            AnimatorList[ nCnt ].applyRootMotion = true;
        }
    }


    public void TakeInEnemyRotate( )
    {
        fRotY += fAddRotateY;

        for( int nCnt = 0; nCnt < TakeInEnemyList.Count; nCnt++ )
        {
            TakeInEnemyList[ nCnt ].gameObject.transform.rotation = Quaternion.Euler( 0.0f , fRotY , 0.0f );
        }
    }


    public void TakeInEnemySetPosY( float fManY , float fWomanY )
    {
        for( int nCnt = 0; nCnt < TakeInEnemyList.Count; nCnt++ )
        {
            if( TakeInEnemyList[ nCnt ].gameObject.GetComponent< EnemySex >( ).GetSexType( ) == EnemySex.EnemySexType.TYPE_MAN )
            {
                 TakeInEnemyList[ nCnt ].gameObject.transform.position += new Vector3( 0.0f , fManY , 0.0f );
            }
            else
            {
                TakeInEnemyList[ nCnt ].gameObject.transform.position += new Vector3( 0.0f , fWomanY , 0.0f );
            }
           
        }
    }
}
