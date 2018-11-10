using System.Collections;
using UnityEngine;
using Uniduino;


public class BonusManager : MonoBehaviour
{
           Manager    ManagerClass;   //マネージャのクラス
    public GameObject ManagerObj;     //マネージャのオブジェクト
           bool       bFlg;
    public GameObject    BonusPrefab;         //ボーナスのプレハブ
     int           nCntBeat;            //一拍のカウンタ


           GameObject[ ] aBonusPrefabArray;   //生成したボーナスのオブジェクト配列
           TextAsset     BonusText;           //ボーナス情報が格納されたファイルの情報
           string[ ]     aString;
           int[ ]        nTimingArray;        //タイミングフレームを格納した配列
           int           nCreateNo;           //生成する敵の番号
           GameObject    BonusTmp;
          

           GameObject[ ] aBonusPrefabArray2;   //生成したボーナスのオブジェクト配列
           TextAsset     BonusText2;           //ボーナス情報が格納されたファイルの情報
           string[ ]     aString2;
           int[ ]        nTimingArray2;        //タイミングフレームを格納した配列
           int           nCreateNo2;           //生成する敵の番号 
      GameObject    BonusTmp2;

           GameObject[ ] aBonusPrefabArray3;   //生成したボーナスのオブジェクト配列
           TextAsset     BonusText3;           //ボーナス情報が格納されたファイルの情報
           string[ ]     aString3;
           int[ ]        nTimingArray3;        //タイミングフレームを格納した配列
           int           nCreateNo3;           //生成する敵の番号 
      GameObject    BonusTmp3;
           
    public SerialHandler SerialHandlerClass;
    public Arduino ArdiunoClass;


     PlayerLeft         PlayerLeftClass;           //左プレイヤーのクラス
           PlayerCenter       PlayerCenterClass;         //中央プレイヤーのクラス
           PlayerRight        PlayerRightClass;          //右プレイヤーのクラス
     public GameObject         PlayerLeftPrefab;          //左プレイヤーのプレハブ
    public GameObject         PlayerCenterPrefab;        //中央プレイヤーのプレハブ
    public GameObject         PlayerRightPrefab;         //右プレイヤーのプレハブ 


     ScoreManager ScoreClass;
    PerformanceManager PerformanceClass;

    public GameObject MirrorBallMateriaObj;
    MirrorBallMaterial MirrorBallMateriaClass;


	void Start( )
    { 
        //変数の初期化
        bFlg      = false;
        nCreateNo = 0;
        nCreateNo2 = 0;
        nCreateNo3 = 0;
        nCntBeat  = 1;

        BonusTmp = null;
        BonusTmp2 = null;
        BonusTmp3 = null;

        //各クラスの情報を取得
        ManagerClass = ManagerObj.GetComponent< Manager >( );

        //ボーナス情報をファイルから読み込み
        BonusText = Resources.Load( "bonus_left" ) as TextAsset;
        aString = BonusText.text.Split('\n'); //
        BonusText2 = Resources.Load( "bonus_center" ) as TextAsset;
        aString2 = BonusText2.text.Split('\n'); //
        BonusText3 = Resources.Load( "bonus_right" ) as TextAsset;
        aString3 = BonusText3.text.Split('\n'); //

        //ボーナスオブジェクトを生成し、非表示にしておく
        aBonusPrefabArray = new GameObject[ 15 ];
        nTimingArray      = new int[ 16 ];

        for( int nCnt = 0 , nCnt2 = 0; nCnt < 15; nCnt++ , nCnt2 += 2 )
        {
            aBonusPrefabArray[ nCnt ] = Instantiate( BonusPrefab , new Vector3( -2.5f , 0.0f , 226.0f ) , Quaternion.identity );
            aBonusPrefabArray[ nCnt ].transform.parent = this.transform;
            aBonusPrefabArray[ nCnt ].gameObject.SetActive( false );
            nTimingArray[ nCnt ] = int.Parse( aString[ nCnt ] );   
        }
        nTimingArray[ 15 ] = 999;

        aBonusPrefabArray2 = new GameObject[ 16 ];
        nTimingArray2      = new int[ 17 ];

        for( int nCnt = 0 , nCnt2 = 0; nCnt < 16; nCnt++ , nCnt2 += 2 )
        {
            aBonusPrefabArray2[ nCnt ] = Instantiate( BonusPrefab , new Vector3( 0.0f , 0.0f , 226.0f ) , Quaternion.identity );
            aBonusPrefabArray2[ nCnt ].transform.parent = this.transform;
            aBonusPrefabArray2[ nCnt ].gameObject.SetActive( false );
            nTimingArray2[ nCnt ] = int.Parse( aString2[ nCnt ] );   
        }
        nTimingArray2[ 16 ] = 999;

        aBonusPrefabArray3 = new GameObject[ 15 ];
        nTimingArray3      = new int[ 16 ];

        for( int nCnt = 0 , nCnt2 = 0; nCnt < 15; nCnt++ , nCnt2 += 2 )
        {
            aBonusPrefabArray3[ nCnt ] = Instantiate( BonusPrefab , new Vector3( 2.5f , 0.0f , 226.0f ) , Quaternion.identity );
            aBonusPrefabArray3[ nCnt ].transform.parent = this.transform;
            aBonusPrefabArray3[ nCnt ].gameObject.SetActive( false );
            nTimingArray3[ nCnt ] = int.Parse( aString3[ nCnt ] );   
        }
        nTimingArray3[ 15 ] = 999;

        //アルディーノの設定
        ArdiunoClass = Arduino.global;
        ArdiunoClass.Setup( ConfigurePins );

        PlayerLeftClass         = PlayerLeftPrefab.GetComponent< PlayerLeft >( );
        PlayerCenterClass       = PlayerCenterPrefab.GetComponent< PlayerCenter >( );
        PlayerRightClass        = PlayerRightPrefab.GetComponent< PlayerRight >( );

          ScoreClass = ManagerClass.GetScoreManager( ).GetComponent< ScoreManager >( );
        PerformanceClass = ManagerClass.GetPerformanceManager( ).GetComponent< PerformanceManager >( );

        MirrorBallMateriaClass = MirrorBallMateriaObj.GetComponent< MirrorBallMaterial >( );
	}


    void ConfigurePins( )
    {
        ArdiunoClass.pinMode( 6 , PinMode.OUTPUT );
    }


    //ボーナスタイム
    public void BonusTime( )
    {
        if( bFlg == false )
        {
            bFlg = true;
            SerialHandlerClass.Write( "5" );
            MirrorBallMateriaClass.BonusAllEnabule( );
        }

        //一拍毎に生成するかをチェックする
        if( ManagerClass.GetdBonusFrame( ) >= 0.4475f )
        {
            nCntBeat++;

            //生成タイミングになったらボーナスを飛ばす
            if( nCntBeat == nTimingArray[ nCreateNo ] - 1 && nTimingArray[ nCreateNo ] != 999 )
            {
                aBonusPrefabArray[ nCreateNo ].gameObject.SetActive( true );
                aBonusPrefabArray[ nCreateNo ].gameObject.GetComponent< Bonus >( ).SetState( new Vector3( 0.0f , 0.0f , 177.101f - 226.0f ) ,
                                                                                             new Vector3( -2.5f , 0.0f , 177.101f ) , Bonus.BONUS_TYPE.LEFT );
               
                //ジョイコンを振れる様にする
                PlayerLeftClass.ReleasebBonusFlg( );
                nCreateNo++;
            }
            if( nCntBeat == nTimingArray2[ nCreateNo2 ] - 1 && nTimingArray2[ nCreateNo2 ] != 999 )
            {
                aBonusPrefabArray2[ nCreateNo2 ].gameObject.SetActive( true );
                aBonusPrefabArray2[ nCreateNo2 ].gameObject.GetComponent< Bonus >( ).SetState( new Vector3( 0.0f , 0.0f , 177.101f - 226.0f ) ,
                                                                                             new Vector3( 0.0f , 0.0f , 177.101f ) , Bonus.BONUS_TYPE.CENTER );
                 PlayerCenterClass.ReleasebBonusFlg( );   
                nCreateNo2++;
            }
            if( nCntBeat == nTimingArray3[ nCreateNo3 ] - 1 && nTimingArray3[ nCreateNo3 ] != 999 )
            {
                aBonusPrefabArray3[ nCreateNo3 ].gameObject.SetActive( true );
                aBonusPrefabArray3[ nCreateNo3 ].gameObject.GetComponent< Bonus >( ).SetState( new Vector3( 0.0f , 0.0f , 177.101f - 226.0f ) ,
                                                                                            new Vector3( 2.5f , 0.0f , 177.101f ) , Bonus.BONUS_TYPE.RIGHT );
                 PlayerRightClass.ReleasebBonusFlg( );    
                nCreateNo3++;
            }

            ManagerClass.SetFlg2( );
        }

         //33拍でボーナスの終了
        if( ManagerClass.GetdCntFrame( ) >= 0.895f * 33.0f )
        {
           // ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_CHECK );  
           ScoreClass.AggregateScore( );
            PerformanceClass.PhaseCheck( );
            BonusFalse( );
            ManagerClass.SetFlg( );
            SerialHandlerClass.Write( "3" );
            MirrorBallMateriaClass.BonusAlDisable( );
        }
    }

    public void SetBonusLeft( GameObject BonusLeft )
    {
        BonusTmp = BonusLeft;
    }


    public GameObject GetBonusLeft( )
    {
        return BonusTmp;
    }


    public void SetBonusCenter( GameObject BonusCenter )
    {
        BonusTmp2 = BonusCenter;
    }


    public GameObject GetBonusCenter( )
    {
        return BonusTmp2;
    }


    public void SetBonusRight( GameObject BonusRight )
    {
        BonusTmp3 = BonusRight;
    }


    public GameObject GetBonusRight( )
    {
        return BonusTmp3;
    }


    public void BonusFalse( )
    {
        for( int nCnt = 0; nCnt < 15; nCnt++ )
        {
            aBonusPrefabArray[ nCnt ].gameObject.SetActive( false );
        }
   
        for( int nCnt = 0; nCnt < 16; nCnt++ )
        {
            aBonusPrefabArray2[ nCnt ].gameObject.SetActive( false );
        }
     
        for( int nCnt = 0; nCnt < 15; nCnt++ )
        {
            aBonusPrefabArray3[ nCnt ].gameObject.SetActive( false );
        }
    }
}