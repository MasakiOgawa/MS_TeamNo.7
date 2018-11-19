using System.Collections;
using UnityEngine;


public class PlayerRight : MonoBehaviour
{
           Manager       ManagerClass;         //マネージャのクラス
    public GameObject    ManagerObj;           //マネージャのオブジェクト
           Example_gyro  ControllerClass;      //コントローラのクラス
           PlayerManager PlayerManagerClass;   //プレイヤーマネージャのクラス
           EnemyManager  EnemyManagerClass;    //エネミーマネージャのクラス
           ScoreManager  ScoreManagerClass;    //スコアマネージャのクラス
           GameObject    EnemyObj;             //敵オブジェクト保存用の変数
    public bool          bPoseFlg;             //判定重複防止のフラグ

    public bool          bBonusFlg;          
    BonusManager BonusManagerClass;

    public GameObject MirrorBall;

    public GameObject Tmp;

    public float PlayerToBallSpeed;
    public float BallToEnemySpeed;

     public GameObject MotionManagerObj;
    MotionManager MotionManagerClass;

    Animator Child;

     public AudioClip   AudioClip;
           AudioSource AudioSource;

        
	void Start( )
    {
        //変数の初期化
		bPoseFlg = false;

        bBonusFlg = false;

        //各クラスの情報を取得
        ManagerClass       = ManagerObj.GetComponent< Manager>( );
        ControllerClass    = ManagerClass.GetPlayerManager( ).GetComponent< Example_gyro >( );
        PlayerManagerClass = ManagerClass.GetPlayerManager( ).GetComponent< PlayerManager >( );
        EnemyManagerClass  = ManagerClass.GetEnemyManager( ).GetComponent< EnemyManager >( );
        ScoreManagerClass  = ManagerClass.GetScoreManager( ).GetComponent< ScoreManager >( ); 

        BonusManagerClass = ManagerClass.GetBonusManager( ).GetComponent< BonusManager >( );

        MotionManagerClass = MotionManagerObj.GetComponent< MotionManager >( );

        Child = transform.GetChild( 0 ).GetComponent< Animator >( );

         AudioSource      = gameObject.GetComponent< AudioSource >( );
        AudioSource.clip = AudioClip;   
	}


    void Update( )
    {
         if( ( ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R2 ) == Example_gyro.JOYCON_STATE.STATE_UP_TRIGGER ||
                ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R2 ) == Example_gyro.JOYCON_STATE.STATE_DOWN_TRIGGER ||
                ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R2 ) == Example_gyro.JOYCON_STATE.STATE_LEFT_TRIGGER ||
                ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R2 ) == Example_gyro.JOYCON_STATE.STATE_RIGHT_TRIGGER ||
                Input.GetKeyDown( KeyCode.UpArrow ) ||
                Input.GetKeyDown( KeyCode.DownArrow ) ||
                Input.GetKeyDown( KeyCode.LeftArrow ) ||
                Input.GetKeyDown( KeyCode.RightArrow ) )  &&
             EnemyManagerClass.GetTarget( ) == null )
        {
            int Rand = Random.RandomRange(0, 10);

            OneShot.Create( ( OneShot.ONESHOT_TYPE ) Rand, new Vector3 ( 3.0f , 2.0f , PlayerManagerClass.GetfDist( ) + 17.0f ) );
        }

       if( ManagerClass.GetPhase( ) == Manager.GAME_PHASE.PHASE_PLAYER_DANCE &&
            Child.GetCurrentAnimatorStateInfo( 0 ).normalizedTime >= 1.0f )
        {
          
                  MotionManagerClass.ChangeRightMotion( PlayerAnimDefine.Idx.Idle );
            Child.ForceStateNormalizedTime( 0.0f );
        }
    }
	

    //プレイヤーのダンス
    public void Pose( )
    {
        //まだ振られていなかったら
        if( bPoseFlg == false )
        {
            //上
            if( ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R2 ) == Example_gyro.JOYCON_STATE.STATE_UP_TRIGGER ||
                Input.GetKeyDown( KeyCode.UpArrow ) )
            { 
                
                 MotionManagerClass.ChangeRightMotion( PlayerAnimDefine.Idx.Player_Up );

                //現在の敵の情報を取得
                EnemyObj = EnemyManagerClass.GetTarget( );

                Tmp.transform.position = new Vector3( 3.0f , 0.0f , PlayerManagerClass.GetfDist( ) + 17.0f );

                //一致していたら
                if( EnemyObj != null && EnemyObj.tag == "Up" )
                {
                    //振った瞬間の経過フレームを取得
                    float fTmp = ( float )ManagerClass.GetdPoseFrame( );

                    //現在の敵の該当フレームを求める
                    float fTargetFrame = 0.455f * ( PlayerManagerClass.GetnTargetNo( ) - 1 );

                    if( Mathf.Abs( fTmp - fTargetFrame ) < 1.0f )
                    {
                        ScoreManagerClass.ActiveTrue( new Vector2( 240.0f , -180.0f ) , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                        ExplodeController.Create( Tmp.transform.position , EnemyObj.transform.position , MirrorBall.transform.position ,
                                                  ExplodeController.EXPLODE_TYPE.TYPE_EXCELLENT , PlayerToBallSpeed, BallToEnemySpeed);
                    }
                    else if( Mathf.Abs( fTmp - fTargetFrame ) < 0.5f )
                    {
                        ScoreManagerClass.ActiveTrue( new Vector2( 290.0f , -180.0f ) , ScoreManager.EVALUATION.EVALUATION_FINE );
                        ExplodeController.Create( Tmp.transform.position , EnemyObj.transform.position , MirrorBall.transform.position ,
                                                  ExplodeController.EXPLODE_TYPE.TYPE_FINE , PlayerToBallSpeed, BallToEnemySpeed);
                    }
                    else
                    {
                          ScoreManagerClass.ActiveTrue(new Vector2( 310.0f , -180.0f ) , ScoreManager.EVALUATION.EVALUATION_BAD );
                         ExplodeController.Create( Tmp.transform.position , EnemyObj.transform.position , MirrorBall.transform.position ,
                                                  ExplodeController.EXPLODE_TYPE.TYPE_BAD , PlayerToBallSpeed, BallToEnemySpeed);
                    }
                    
                }
                

                bPoseFlg = true;
            }
            //下
            else if( ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R2 ) == Example_gyro.JOYCON_STATE.STATE_DOWN_TRIGGER ||
                     Input.GetKeyDown( KeyCode.DownArrow ) )
            {   
                MotionManagerClass.ChangeRightMotion( PlayerAnimDefine.Idx.Player_Down );

                //現在の敵の情報を取得
                EnemyObj = EnemyManagerClass.GetTarget( );

                 Tmp.transform.position = new Vector3( 3.0f , 0.0f , PlayerManagerClass.GetfDist( ) + 17.0f );
               
                //一致していたら
                if( EnemyObj != null && EnemyObj.tag == "Down" )
                {
                    //振った瞬間の経過フレームを取得
                   float fTmp = ( float )ManagerClass.GetdPoseFrame( );

                    //現在の敵の該当フレームを求める
                    float fTargetFrame = 0.455f * ( PlayerManagerClass.GetnTargetNo( ) - 1 );

                    if( Mathf.Abs( fTmp - fTargetFrame ) < 1.0f )
                    {
                        ScoreManagerClass.ActiveTrue( new Vector2( 240.0f , -180.0f ) , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                        ExplodeController.Create( Tmp.transform.position , EnemyObj.transform.position , MirrorBall.transform.position ,
                                                  ExplodeController.EXPLODE_TYPE.TYPE_EXCELLENT , PlayerToBallSpeed, BallToEnemySpeed);
                    }
                    else if( Mathf.Abs( fTmp - fTargetFrame ) < 0.5f )
                    {
                        ScoreManagerClass.ActiveTrue( new Vector2( 290.0f , -180.0f ) , ScoreManager.EVALUATION.EVALUATION_FINE );
                        ExplodeController.Create( Tmp.transform.position , EnemyObj.transform.position , MirrorBall.transform.position ,
                                                  ExplodeController.EXPLODE_TYPE.TYPE_FINE , PlayerToBallSpeed, BallToEnemySpeed);
                    }
                     else
                    {
                          ScoreManagerClass.ActiveTrue(new Vector2( 310.0f , -180.0f ) , ScoreManager.EVALUATION.EVALUATION_BAD );
                         ExplodeController.Create( Tmp.transform.position , EnemyObj.transform.position , MirrorBall.transform.position ,
                                                  ExplodeController.EXPLODE_TYPE.TYPE_BAD , PlayerToBallSpeed, BallToEnemySpeed);
                    }
                    
                }
               
                
                bPoseFlg = true;
            }
            //左
            else if( ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R2 ) == Example_gyro.JOYCON_STATE.STATE_LEFT_TRIGGER ||
                     Input.GetKeyDown( KeyCode.LeftArrow ) )
            {
                MotionManagerClass.ChangeRightMotion( PlayerAnimDefine.Idx.Player_Left );

                //現在の敵の情報を取得
                EnemyObj = EnemyManagerClass.GetTarget( );

                 Tmp.transform.position = new Vector3( 3.0f , 0.0f , PlayerManagerClass.GetfDist( ) + 17.0f );
           
                 //一致していたら
                if( EnemyObj != null && EnemyObj.tag == "Left" )
                {
                    //振った瞬間の経過フレームを取得
                    float fTmp = ( float )ManagerClass.GetdPoseFrame( );

                    //現在の敵の該当フレームを求める
                    float fTargetFrame = 0.455f * ( PlayerManagerClass.GetnTargetNo( ) - 1 );

                    if( Mathf.Abs( fTmp - fTargetFrame ) < 1.0f )
                    {
                        ScoreManagerClass.ActiveTrue( new Vector2( 240.0f , -180.0f ) , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                        ExplodeController.Create( Tmp.transform.position , EnemyObj.transform.position , MirrorBall.transform.position ,
                                                  ExplodeController.EXPLODE_TYPE.TYPE_EXCELLENT , PlayerToBallSpeed, BallToEnemySpeed);
                    }
                    else if( Mathf.Abs( fTmp - fTargetFrame ) < 0.5f )
                    {
                        ScoreManagerClass.ActiveTrue( new Vector2( 290.0f , -180.0f ) , ScoreManager.EVALUATION.EVALUATION_FINE );
                        ExplodeController.Create( Tmp.transform.position , EnemyObj.transform.position , MirrorBall.transform.position ,
                                                  ExplodeController.EXPLODE_TYPE.TYPE_FINE , PlayerToBallSpeed, BallToEnemySpeed);
                    }
                    else
                    {
                          ScoreManagerClass.ActiveTrue(new Vector2( 310.0f , -180.0f ) , ScoreManager.EVALUATION.EVALUATION_BAD );
                         ExplodeController.Create( Tmp.transform.position , EnemyObj.transform.position , MirrorBall.transform.position ,
                                                  ExplodeController.EXPLODE_TYPE.TYPE_BAD , PlayerToBallSpeed, BallToEnemySpeed);
                    }
                    
                }
                

                bPoseFlg = true;
            }
            //右
            else if( ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R2 ) == Example_gyro.JOYCON_STATE.STATE_RIGHT_TRIGGER ||
                     Input.GetKeyDown( KeyCode.RightArrow ) )
            {  
                MotionManagerClass.ChangeRightMotion( PlayerAnimDefine.Idx.Player_Right );

                //現在の敵の情報を取得
                EnemyObj = EnemyManagerClass.GetTarget( );
                
                Tmp.transform.position = new Vector3( 3.0f , 0.0f , PlayerManagerClass.GetfDist( ) + 17.0f );
     
                //一致していたら
                if( EnemyObj != null && EnemyObj.tag == "Right" )
                {
                    //振った瞬間の経過フレームを取得
                    float fTmp = ( float )ManagerClass.GetdPoseFrame( );

                    //現在の敵の該当フレームを求める
                    float fTargetFrame =0.455f * ( PlayerManagerClass.GetnTargetNo( ) - 1 ); 

                    if( Mathf.Abs( fTmp - fTargetFrame ) < 1.0f )
                    {
                        ScoreManagerClass.ActiveTrue( new Vector2( 240.0f , -180.0f ) , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                        ExplodeController.Create( Tmp.transform.position , EnemyObj.transform.position , MirrorBall.transform.position ,
                                                  ExplodeController.EXPLODE_TYPE.TYPE_EXCELLENT , PlayerToBallSpeed, BallToEnemySpeed);
                    }
                    else if( Mathf.Abs( fTmp - fTargetFrame ) < 0.5f )
                    {
                        ScoreManagerClass.ActiveTrue( new Vector2( 290.0f , -180.0f ) , ScoreManager.EVALUATION.EVALUATION_FINE );
                        ExplodeController.Create( Tmp.transform.position , EnemyObj.transform.position , MirrorBall.transform.position ,
                                                  ExplodeController.EXPLODE_TYPE.TYPE_FINE , PlayerToBallSpeed, BallToEnemySpeed);
                    }
                    else
                    {
                          ScoreManagerClass.ActiveTrue(new Vector2( 310.0f , -180.0f ) , ScoreManager.EVALUATION.EVALUATION_BAD );
                         ExplodeController.Create( Tmp.transform.position , EnemyObj.transform.position , MirrorBall.transform.position ,
                                                  ExplodeController.EXPLODE_TYPE.TYPE_BAD , PlayerToBallSpeed, BallToEnemySpeed);
                    }
                    
                }
                
               
                //コントローラを触れない様にする
                bPoseFlg = true;
            }
        }
    }


    //ボーナス
    public void BonusPlay( )
    {
        //まだ振られていなかったら
        if( bBonusFlg == false )
        {
            if( ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R2 ) == Example_gyro.JOYCON_STATE.STATE_UP_TRIGGER ||
                ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R2 ) == Example_gyro.JOYCON_STATE.STATE_DOWN_TRIGGER ||
                ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R2 ) == Example_gyro.JOYCON_STATE.STATE_LEFT_TRIGGER ||
                ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R2 ) == Example_gyro.JOYCON_STATE.STATE_RIGHT_TRIGGER ||
                Input.GetKeyDown( KeyCode.UpArrow ) ||
                Input.GetKeyDown( KeyCode.DownArrow ) ||
                Input.GetKeyDown( KeyCode.LeftArrow ) ||
                Input.GetKeyDown( KeyCode.RightArrow ) )
            {
                MotionManagerClass.ChangeRightMotion( PlayerAnimDefine.Idx.Player_Right );

                GameObject Tmp = BonusManagerClass.GetBonusRight( );
              
               
               if( Tmp != null && Tmp.GetComponent< Bonus >( ).BonusState == Bonus.BONUS_STATE.TARGET )
                { 
                  
                     AudioSource.PlayOneShot( AudioClip );
                    Tmp.GetComponent< Bonus >( ).SetBiriBiri( );
               //     LightningManager.Create( MirrorBall, 2.67f, Tmp, 0);
                    BounusEffect.Create( Tmp );
                }

                bBonusFlg = true;
            }   
        } 
    }


    //コントローラの判定を開始
    public void ReleasebPoseFlg( )
    {
        bPoseFlg = false;
    }


 
    public void ReleasebBonusFlg( )
    {
        bBonusFlg = false;
    }
}
