﻿using System.Collections;
using UnityEngine;


public class PlayerLeft : MonoBehaviour
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
	}
	

    //プレイヤーのダンス
    public void Pose( )
    {
        //まだ振られていなかったら
        if( bPoseFlg == false )
        {
            //上
            if( ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_L1 ) == Example_gyro.JOYCON_STATE.STATE_UP_TRIGGER ||
                Input.GetKeyDown( KeyCode.A ) )
            { 
                //現在の敵の情報を取得
                EnemyObj = EnemyManagerClass.GetTarget( );

                //一致していたら
                if( EnemyObj != null && EnemyObj.tag == "Up" )
                {
                    //振った瞬間の経過フレームを取得
                    float fTmp = ( float )ManagerClass.GetdPoseFrame( );

                    //現在の敵の該当フレームを求める
                    float fTargetFrame = 0.455f * ( PlayerManagerClass.GetnTargetNo( ) - 1 );

                    if( Mathf.Abs( fTmp - fTargetFrame ) < 0.3f )
                    {
                        ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                     //   ExplodeController.Create(this.gameObject, 0, EnemyObj, 0, MirrorBall, 2,
                      //                           ExplodeController.EXPLODE_TYPE.TYPE_EXCELLENT, 0.5f, 1.25f);
                    }
                    else if( Mathf.Abs( fTmp - fTargetFrame ) < 0.35f )
                    {
                        ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_FINE );
                    //    ExplodeController.Create(this.gameObject, 0, EnemyObj, 0, MirrorBall, 2,
                     //                            ExplodeController.EXPLODE_TYPE.TYPE_FINE, 0.5f, 1.25f);
                    }
                }
                else
                {
                   ScoreManagerClass.ActiveTrue(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                 //  ExplodeController.Create(this.gameObject, 0, EnemyObj, 0, MirrorBall, 2,
                  //                          ExplodeController.EXPLODE_TYPE.TYPE_BAD, 0.5f, 1.25f);
                }

                bPoseFlg = true;
            }
            //下
            else if( ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_L1 ) == Example_gyro.JOYCON_STATE.STATE_DOWN_TRIGGER ||
                     Input.GetKeyDown( KeyCode.DownArrow ) )
            {   
                //現在の敵の情報を取得
                EnemyObj = EnemyManagerClass.GetTarget( );
               
                //一致していたら
                if( EnemyObj != null && EnemyObj.tag == "Down" )
                {
                    //振った瞬間の経過フレームを取得
                   float fTmp = ( float )ManagerClass.GetdPoseFrame( );

                    //現在の敵の該当フレームを求める
                    float fTargetFrame = 0.455f * ( PlayerManagerClass.GetnTargetNo( ) - 1 );

                    if( Mathf.Abs( fTmp - fTargetFrame ) < 0.3f )
                    {
                        ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                   //     ExplodeController.Create(this.gameObject, 0, EnemyObj, 0, MirrorBall, 2,
                   //                              ExplodeController.EXPLODE_TYPE.TYPE_EXCELLENT, 0.5f, 1.25f);
                    }
                    else if( Mathf.Abs( fTmp - fTargetFrame ) < 0.35f )
                    {
                        ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_FINE );
                     //   ExplodeController.Create(this.gameObject, 0, EnemyObj, 0, MirrorBall, 2,
                     //                            ExplodeController.EXPLODE_TYPE.TYPE_FINE, 0.5f, 1.25f);
                    }
                }
                else
                {
                    ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                 //   ExplodeController.Create(this.gameObject, 0, EnemyObj, 0, MirrorBall, 2,
                  //                               ExplodeController.EXPLODE_TYPE.TYPE_BAD, 0.5f, 1.25f);
                }

                bPoseFlg = true;
            }
            //左
            else if( ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_L1 ) == Example_gyro.JOYCON_STATE.STATE_LEFT_TRIGGER ||
                     Input.GetKeyDown( KeyCode.LeftArrow ) )
            {
                //現在の敵の情報を取得
                EnemyObj = EnemyManagerClass.GetTarget( );
           
                 //一致していたら
                if( EnemyObj != null && EnemyObj.tag == "Left" )
                {
                    //振った瞬間の経過フレームを取得
                    float fTmp = ( float )ManagerClass.GetdPoseFrame( );

                    //現在の敵の該当フレームを求める
                    float fTargetFrame = 0.455f * ( PlayerManagerClass.GetnTargetNo( ) - 1 );

                    if( Mathf.Abs( fTmp - fTargetFrame ) < 0.3f )
                    {
                        ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                     //   ExplodeController.Create(this.gameObject, 0, EnemyObj, 0, MirrorBall, 2,
                      //                           ExplodeController.EXPLODE_TYPE.TYPE_EXCELLENT, 0.5f, 1.25f);
                    }
                    else if( Mathf.Abs( fTmp - fTargetFrame ) < 0.35f )
                    {
                        ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_FINE );
                       // ExplodeController.Create(this.gameObject, 0, EnemyObj, 0, MirrorBall, 2,
                       //                          ExplodeController.EXPLODE_TYPE.TYPE_FINE, 0.5f, 1.25f);
                    }
                }
                else
                {
                   ScoreManagerClass.ActiveTrue(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                 //  ExplodeController.Create(this.gameObject, 0, EnemyObj, 0, MirrorBall, 2,
                  //                               ExplodeController.EXPLODE_TYPE.TYPE_BAD, 0.5f, 1.25f);
                }

                bPoseFlg = true;
            }
            //右
            else if( ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_L1 ) == Example_gyro.JOYCON_STATE.STATE_RIGHT_TRIGGER ||
                     Input.GetKeyDown( KeyCode.RightArrow ) )
            {  
                //現在の敵の情報を取得
                EnemyObj = EnemyManagerClass.GetTarget( );           
     
                //一致していたら
                if( EnemyObj != null && EnemyObj.tag == "Right" )
                {
                    //振った瞬間の経過フレームを取得
                    float fTmp = ( float )ManagerClass.GetdPoseFrame( );

                    //現在の敵の該当フレームを求める
                    float fTargetFrame =0.455f * ( PlayerManagerClass.GetnTargetNo( ) - 1 ); 

                    if( Mathf.Abs( fTmp - fTargetFrame ) < 0.3f )
                    {
                        ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                  //      ExplodeController.Create(this.gameObject, 0, EnemyObj, 0, MirrorBall, 2,
                   //                              ExplodeController.EXPLODE_TYPE.TYPE_EXCELLENT, 0.5f, 1.25f);
                    }
                    else if( Mathf.Abs( fTmp - fTargetFrame ) < 0.35f )
                    {
                        ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_FINE );
                   //     ExplodeController.Create(this.gameObject, 0, EnemyObj, 0, MirrorBall, 2,
                   //                              ExplodeController.EXPLODE_TYPE.TYPE_FINE, 0.5f, 1.25f);
                    }
                }
                else
                {
                   ScoreManagerClass.ActiveTrue(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                 //  ExplodeController.Create(this.gameObject, 0, EnemyObj, 0, MirrorBall, 2,
                 //                                ExplodeController.EXPLODE_TYPE.TYPE_BAD, 0.5f, 1.25f);
                }

                //コントローラを触れない様にする
                bPoseFlg = true;
            }
        }
    }


    //ボーナス
    public void Bonus( )
    {
        //まだ振られていなかったら
        if( bBonusFlg == false )
        {
            if( ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_L1 ) == Example_gyro.JOYCON_STATE.STATE_UP_TRIGGER ||
                ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_L1 ) == Example_gyro.JOYCON_STATE.STATE_DOWN_TRIGGER ||
                ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_L1 ) == Example_gyro.JOYCON_STATE.STATE_LEFT_TRIGGER ||
                ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_L1 ) == Example_gyro.JOYCON_STATE.STATE_RIGHT_TRIGGER ||
                Input.GetKeyDown( KeyCode.UpArrow ) ||
                Input.GetKeyDown( KeyCode.DownArrow ) ||
                Input.GetKeyDown( KeyCode.LeftArrow ) ||
                Input.GetKeyDown( KeyCode.RightArrow ) )
            {
                GameObject Tmp = BonusManagerClass.GetBonusLeft( );
                Vector3 Pos = new Vector3( -2.5f , 0.0f , 17.0f );
              
              
                if( Tmp != null && Vector3.Distance( Tmp.gameObject.transform.position , Pos ) <= 20.0f )
                { 
                    ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                    Tmp.GetComponent< Bonus >( ).SetBiriBiri( );
                    LightningManager.Create( MirrorBall, 2.67f, Tmp, 0);
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
