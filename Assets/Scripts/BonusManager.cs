using System.Collections;
using UnityEngine;
using Uniduino;
using System.Collections.Generic;


public class BonusManager : MonoBehaviour
{
           Manager       ManagerClass;   //マネージャのクラス
    public GameObject    ManagerObj;     //マネージャのオブジェクト
           PlayerLeft    PlayerLeftClass;           //左プレイヤーのクラス
           PlayerCenter  PlayerCenterClass;         //中央プレイヤーのクラス
           PlayerRight   PlayerRightClass;          //右プレイヤーのクラス
    public GameObject    PlayerLeftPrefab;          //左プレイヤーのプレハブ
    public GameObject    PlayerCenterPrefab;        //中央プレイヤーのプレハブ
    public GameObject    PlayerRightPrefab;         //右プレイヤーのプレハブ 
    public GameObject [ ]Enemy_Prefab;
    List<GameObject>     LeftEnemyList;
    List<GameObject>     CenterEnemyList;
    List<GameObject>     RightEnemyList;
    GameObject           LeftTarget;
    GameObject           CenterTarget;
    GameObject           RightTarget;
    bool                 bFlg;
    TextAsset            BonusText;           //ボーナス情報が格納されたファイルの情報
    int                  nCreateNo;
    float                fCntFrame;  
    int                  nLeftNo;
    int                  nCenterNo;
    int                  nRightNo;

           
    public SerialHandler SerialHandlerClass;
    public Arduino ArdiunoClass;
    ScoreManager ScoreClass;
    PerformanceManager PerformanceClass;
    public GameObject MirrorBallMateriaObj;
    MirrorBallMaterial MirrorBallMateriaClass;


	void Start( )
    { 
        //変数の初期化
        bFlg      = false;
        LeftTarget = null;
        CenterTarget = null;
        RightTarget = null;
        nCreateNo = 0;
        fCntFrame = 0.0f;
        nLeftNo = 0;
        nCenterNo = 0;
        nRightNo = 0;
      

        //各クラスの情報を取得
        ManagerClass = ManagerObj.GetComponent< Manager >( );

        //ボーナス情報をファイルから読み込み
        BonusText = Resources.Load( "bonus" ) as TextAsset;

        //アルディーノの設定
        ArdiunoClass = Arduino.global;
        ArdiunoClass.Setup( ConfigurePins );

        PlayerLeftClass         = PlayerLeftPrefab.GetComponent< PlayerLeft >( );
        PlayerCenterClass       = PlayerCenterPrefab.GetComponent< PlayerCenter >( );
        PlayerRightClass        = PlayerRightPrefab.GetComponent< PlayerRight >( );
        ScoreClass = ManagerClass.GetScoreManager( ).GetComponent< ScoreManager >( );
        PerformanceClass = ManagerClass.GetPerformanceManager( ).GetComponent< PerformanceManager >( );
        MirrorBallMateriaClass = MirrorBallMateriaObj.GetComponent< MirrorBallMaterial >( );

        LeftEnemyList = new List<GameObject>(); 
        CenterEnemyList = new List<GameObject>(); 
        RightEnemyList = new List<GameObject>(); 
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
            BounusArea.Create( new Vector3( -4.0f , 0.0f , 75.0f ) , new Vector3( 0.0f , 0.0f , 75.0f ) , new Vector3( 4.0f , 0.0f , 75.0f ) );
        }

        //一定フレーム毎に生成する
        fCntFrame += Time.deltaTime;

        if( fCntFrame >= 1.0f )
        {
            fCntFrame = 0.0f;

            //リモコンを振れる様にする
            PlayerLeftClass.ReleasebBonusFlg( );
            PlayerCenterClass.ReleasebBonusFlg( );  
            PlayerRightClass.ReleasebBonusFlg( );    

            for( int nCnt = nCreateNo , nCnt2 = 0; nCnt < nCreateNo + 3; nCnt++ , nCnt2++ )
            {
                switch( nCnt2 )
                { 
                    case 0:
                        if( BonusText.text[ nCreateNo + nCnt2 ] == '1' )
                        {
                            LeftEnemyList.Add( Instantiate( Enemy_Prefab[ Random.Range( 0 , 12 ) ] , new Vector3( -4.0f , 0.0f , 100.0f ) , Quaternion.identity ) );
                            LeftEnemyList[ nLeftNo ].transform.rotation = Quaternion.Euler( 0.0f , 180.0f , 0.0f );
                            LeftEnemyList[ nLeftNo ].GetComponent< PlayerAnim >( ).MotionChange( PlayerAnimDefine.Idx.HipHopDancing3 );
                            LeftEnemyList[ nLeftNo ].GetComponent< Bonus >( ).SetState( Bonus.BONUS_TYPE.LEFT );
                            nLeftNo++;
                        }
                    break;

                    case 1:
                        if( BonusText.text[ nCreateNo + nCnt2 ] == '1' )
                        {
                            CenterEnemyList.Add( Instantiate( Enemy_Prefab[ Random.Range( 0 , 12 ) ] , new Vector3( 0.0f , 0.0f , 100.0f ) , Quaternion.identity ) );
                            CenterEnemyList[ nCenterNo ].transform.rotation = Quaternion.Euler( 0.0f , 180.0f , 0.0f );
                            CenterEnemyList[ nCenterNo ].GetComponent< PlayerAnim >( ).MotionChange( PlayerAnimDefine.Idx.HipHopDancing3 );
                            CenterEnemyList[ nCenterNo ].GetComponent< Bonus >( ).SetState( Bonus.BONUS_TYPE.CENTER );
                            nCenterNo++;
                        }
                    break;

                    case 2:
                        if( BonusText.text[ nCreateNo + nCnt2 ] == '1' )
                        {
                            RightEnemyList.Add( Instantiate( Enemy_Prefab[ Random.Range( 0 , 12 ) ] , new Vector3( 4.0f , 0.0f , 100.0f ) , Quaternion.identity ) );
                            RightEnemyList[ nRightNo ].transform.rotation = Quaternion.Euler( 0.0f , 180.0f , 0.0f );
                            RightEnemyList[ nRightNo ].GetComponent< PlayerAnim >( ).MotionChange( PlayerAnimDefine.Idx.HipHopDancing3 );
                            RightEnemyList[ nRightNo ].GetComponent< Bonus >( ).SetState( Bonus.BONUS_TYPE.RIGHT );
                            nRightNo++;
                        }
                    break;
                }  
            }

             //文字列の添え字を進める(敵3体+改行コード2文字分)
             nCreateNo += 5;
        }
          //  ManagerClass.SetFlg2( );
     
         //33拍でボーナスの終了
        if( ManagerClass.GetdCntFrame( ) >= 0.895f * 33.0f )
        {
            ScoreClass.AggregateScore( );
            PerformanceClass.PhaseCheck( );
            ReleaseEnemy( );
            ManagerClass.SetFlg( );
            SerialHandlerClass.Write( "3" );
            MirrorBallMateriaClass.BonusAlDisable( );
            BounusArea.Delete( );
        }
    }


    public void SetBonusLeft( GameObject BonusLeft )
    {
        LeftTarget = BonusLeft;
    }


    public GameObject GetBonusLeft( )
    {
        return LeftTarget;
    }


    public void SetBonusCenter( GameObject BonusCenter )
    {
        CenterTarget = BonusCenter;
    }


    public GameObject GetBonusCenter( )
    {
        return CenterTarget;
    }


    public void SetBonusRight( GameObject BonusRight )
    {
        RightTarget = BonusRight;
    }


    public GameObject GetBonusRight( )
    {
        return RightTarget;
    }


    public void LeftAreaChange( bool bColor )
    {
        BounusArea.ChangeColor( 0 , bColor );
    }


    public void CenterAreaChange( bool bColor )
    {
        BounusArea.ChangeColor( 1 , bColor );
    }


    public void RightAreaChange( bool bColor )
    {
        BounusArea.ChangeColor( 2 , bColor );
    }


    void ReleaseEnemy( )
    {
        for( int nCnt = 0; nCnt < LeftEnemyList.Count; nCnt++ )
        {
            LeftEnemyList[ nCnt ].SetActive( false );
        }

        for( int nCnt = 0; nCnt < CenterEnemyList.Count; nCnt++ )
        {
            CenterEnemyList[ nCnt ].SetActive( false );
        }

        for( int nCnt = 0; nCnt < RightEnemyList.Count; nCnt++ )
        {
            RightEnemyList[ nCnt ].SetActive( false );
        }

        LeftEnemyList.Clear( );
        CenterEnemyList.Clear( );
        RightEnemyList.Clear( );
    }
}