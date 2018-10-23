using System.Collections;
using UnityEngine;


public class PlayerCenter : MonoBehaviour
{
           Manager       ManagerClass;         //マネージャのクラス
    public GameObject    ManagerObj;           //マネージャのオブジェクト
           Example_gyro  ControllerClass;      //コントローラのクラス
           PlayerManager PlayerManagerClass;   //プレイヤーマネージャのクラス
           EnemyManager  EnemyManagerClass;    //エネミーマネージャのクラス
           ScoreManager  ScoreManagerClass;    //スコアマネージャのクラス
           GameObject    EnemyObj;             //敵オブジェクト保存用の変数
    public bool          bPoseFlg;             //判定重複防止のフラグ
        

	void Start( )
    {
        //変数の初期化
		bPoseFlg = false;

        //各クラスの情報を取得
        ManagerClass       = ManagerObj.GetComponent< Manager>( );
        ControllerClass    = ManagerClass.GetPlayerManager( ).GetComponent< Example_gyro >( );
        PlayerManagerClass = ManagerClass.GetPlayerManager( ).GetComponent< PlayerManager >( );
        EnemyManagerClass  = ManagerClass.GetEnemyManager( ).GetComponent< EnemyManager >( );
        ScoreManagerClass  = ManagerClass.GetScoreManager( ).GetComponent< ScoreManager >( ); 
	}
	

    //プレイヤーのダンス
    public void Pose( )
    {
        //まだ振られていなかったら
        if( bPoseFlg == false )
        {
            //上
            if( ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R1 ) == Example_gyro.JOYCON_STATE.STATE_UP_TRIGGER ||
                Input.GetKeyDown( KeyCode.UpArrow ) )
            { 
                //現在の敵の情報を取得
                EnemyObj = EnemyManagerClass.GetTarget( );
     
                //一致していたら
                if( EnemyObj != null && EnemyObj.tag == "Up" )
                {
                    //振った瞬間の経過フレームを取得
                    float fTmp = ManagerClass.GetPoseFrame( );

                    //現在の敵の該当フレームを求める
                    float fTargetFrame = 0.461432f * ( PlayerManagerClass.GetnTargetNo( ) - 1 );

                    if( Mathf.Abs( fTmp - fTargetFrame ) < 7 )
                    {
                        ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                    }
                    else
                    {
                        ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_FINE );
                    }
                }
                else
                {
                   ScoreManagerClass.ActiveTrue(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                }

                bPoseFlg = true;
            }
            //下
            else if( ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R1 ) == Example_gyro.JOYCON_STATE.STATE_DOWN_TRIGGER ||
                     Input.GetKeyDown( KeyCode.DownArrow ) )
            {   
                //現在の敵の情報を取得
                EnemyObj = EnemyManagerClass.GetTarget( );

                //一致していたら
                if( EnemyObj != null && EnemyObj.tag == "Down" )
                {
                    //振った瞬間の経過フレームを取得
                   float fTmp = ManagerClass.GetPoseFrame( );

                    //現在の敵の該当フレームを求める
                    float fTargetFrame = 0.461432f * ( PlayerManagerClass.GetnTargetNo( ) - 1 );

                    if( Mathf.Abs( fTmp - fTargetFrame ) < 7 )
                    {
                        ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                    }
                    else
                    {
                        ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_FINE );
                    }
                }
                else
                {
                    ScoreManagerClass.ActiveTrue(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                }

                bPoseFlg = true;
            }
            //左
            else if( ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R1 ) == Example_gyro.JOYCON_STATE.STATE_LEFT_TRIGGER ||
                     Input.GetKeyDown( KeyCode.LeftArrow ) )
            {
                //現在の敵の情報を取得
                EnemyObj = EnemyManagerClass.GetTarget( );
     
                 //一致していたら
                if( EnemyObj != null && EnemyObj.tag == "Left" )
                {
                    //振った瞬間の経過フレームを取得
                    float fTmp = ManagerClass.GetPoseFrame( );

                    //現在の敵の該当フレームを求める
                    float fTargetFrame = 0.461432f * ( PlayerManagerClass.GetnTargetNo( ) - 1 );

                    if( Mathf.Abs( fTmp - fTargetFrame ) < 7 )
                    {
                        ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                    }
                    else 
                    {
                        ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_FINE );
                    }
                }
                else
                {
                   ScoreManagerClass.ActiveTrue(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                }

                bPoseFlg = true;
            }
            //右
            else if( ControllerClass.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R1 ) == Example_gyro.JOYCON_STATE.STATE_RIGHT_TRIGGER ||
                     Input.GetKeyDown( KeyCode.RightArrow ) )
            {
                //現在の敵の情報を取得
                EnemyObj = EnemyManagerClass.GetTarget( );
     
                //一致していたら
                if( EnemyObj != null && EnemyObj.tag == "Right" )
                {
                    //振った瞬間の経過フレームを取得
                    float fTmp = ManagerClass.GetPoseFrame( );

                    //現在の敵の該当フレームを求める
                    float fTargetFrame =0.461432f * ( PlayerManagerClass.GetnTargetNo( ) - 1 );

                    if( Mathf.Abs( fTmp - fTargetFrame ) < 7 )
                    {
                        ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                    }
                    else
                    {
                        ScoreManagerClass.ActiveTrue( transform.position , ScoreManager.EVALUATION.EVALUATION_FINE );
                    }
                }
                else
                {
                   ScoreManagerClass.ActiveTrue(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                }

                //コントローラを触れない様にする
                bPoseFlg = true;
            }
        }
    }


    //コントローラの判定を開始
    public void ReleasebPoseFlg( )
    {
        bPoseFlg = false;
    }
}
