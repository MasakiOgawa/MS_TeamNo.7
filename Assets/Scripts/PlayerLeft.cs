using System.Collections;
using UnityEngine;


public class PlayerLeft : MonoBehaviour
{
    public GameObject    ManagerObj;          //マネージャのオブジェクト
           Example_gyro  ControllerObj;       //コントローラのオブジェクト
           PlayerManager PlayerManagerObj;    //プレイヤーマネージャのオブジェクト
           EnemyManager  EnemyManagerObj;     //エネミーマネージャのオブジェクト
           ScoreManager  ScoreManagerObj;     //スコアマネージャのオブジェクト
    public bool          bPoseFlg;            //判定重複防止のフラグ
           GameObject    TmpObj;              //保存用のオブジェクト変数
           float         fTargetFrameConst;   //該当ターゲットのフレーム数の定数
    public float         fExcellentFrame;     //EXCELLENT判定のフレーム
   
	void Start( )
    {
        //BPMを取得
        float fBPM = ManagerObj.GetComponent< Manager >( ).GetBGM( ).GetComponent< BGM >( ).GetBPM( );

        //変数の初期化
		bPoseFlg = false;

        //コントローラのオブジェクトを取得
        ControllerObj = ManagerObj.GetComponent< Manager >( ).GetPlayerManager( ).GetComponent< Example_gyro >( );

        //プレイヤーマネージャのオブジェクトを取得
        PlayerManagerObj = ManagerObj.GetComponent< Manager >( ).GetPlayerManager( ).GetComponent< PlayerManager >( );

        //エネミーーマネージャのオブジェクトを取得
        EnemyManagerObj = ManagerObj.GetComponent< Manager >( ).GetEnemyManager( ).GetComponent< EnemyManager >( );

        //スコアマネージャのオブジェクトを取得
        ScoreManagerObj = ManagerObj.GetComponent< Manager >( ).GetScoreManager( ).GetComponent< ScoreManager >( );

        fTargetFrameConst = ( ( 60.0f / fBPM ) * 4.0f ) / 8.0f;
	}
	

    //プレイヤーのダンス
    public void Pose( )
    {
        //まだ振られていなかったら
        if( bPoseFlg == false )
        {
            //上
            if( ControllerObj.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_L1 ) == Example_gyro.JOYCON_STATE.STATE_UP_TRIGGER )
            { 
                //敵の番号を取得
                TmpObj = EnemyManagerObj.GetTarget( );
     
                //一致していたら
                if( TmpObj != null && TmpObj.tag == "Up" )//現在エクセレント
                {
                    //振った瞬間の経過フレームを取得
                    float fTmp = PlayerManagerObj.GetFourBeat( );

                    //現在の敵の該当フレームを求める
                    float fTargetFrame = fTargetFrameConst * ( PlayerManagerObj.GetTargetNo( ) - 1 );

                    if( Mathf.Abs( fTmp - fTargetFrame ) < fExcellentFrame )
                    {
                        ScoreManagerObj.Create( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                    }
                    else
                    {
                        ScoreManagerObj.Create( transform.position , ScoreManager.EVALUATION.EVALUATION_FINE );
                    }
                }
                else
                {
                   ScoreManagerObj.Create(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                }

                bPoseFlg = true;
            }
            //下
            else if( ControllerObj.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_L1 ) == Example_gyro.JOYCON_STATE.STATE_DOWN_TRIGGER )
            {   
                //敵の番号を取得
                TmpObj = EnemyManagerObj.GetTarget( );
     
                //一致していたら
                if( TmpObj != null && TmpObj.tag == "Down" )//現在エクセレント
                {
                    //振った瞬間の経過フレームを取得
                    float fTmp = PlayerManagerObj.GetFourBeat( );

                    //現在の敵の該当フレームを求める
                    float fTargetFrame = fTargetFrameConst * ( PlayerManagerObj.GetTargetNo( ) - 1 );

                    if( Mathf.Abs( fTmp - fTargetFrame ) < fExcellentFrame )
                    {
                        ScoreManagerObj.Create( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                    }
                    else
                    {
                        ScoreManagerObj.Create( transform.position , ScoreManager.EVALUATION.EVALUATION_FINE );
                    }
                }
                else
                {
                    ScoreManagerObj.Create(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                }

                bPoseFlg = true;
            }
            //左
            else if( ControllerObj.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_L1 ) == Example_gyro.JOYCON_STATE.STATE_LEFT_TRIGGER )
            {
                //敵の番号を取得
                TmpObj = EnemyManagerObj.GetTarget( );
     
                 //一致していたら
                if( TmpObj != null && TmpObj.tag == "Left" )//現在エクセレント
                {
                    //振った瞬間の経過フレームを取得
                    float fTmp = PlayerManagerObj.GetFourBeat( );

                    //現在の敵の該当フレームを求める
                    float fTargetFrame = fTargetFrameConst * ( PlayerManagerObj.GetTargetNo( ) - 1 );

                    if( Mathf.Abs( fTmp - fTargetFrame ) < fExcellentFrame )
                    {
                        ScoreManagerObj.Create( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                    }
                    else 
                    {
                        ScoreManagerObj.Create( transform.position , ScoreManager.EVALUATION.EVALUATION_FINE );
                    }
                }
                else
                {
                   ScoreManagerObj.Create(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                }

                bPoseFlg = true;
            }
            //右
            else if( ControllerObj.GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_L1 ) == Example_gyro.JOYCON_STATE.STATE_RIGHT_TRIGGER )
            {
                //敵の番号を取得
                TmpObj = EnemyManagerObj.GetTarget( );
     
                //一致していたら
                if( TmpObj != null && TmpObj.tag == "Right" )//現在エクセレント
                {
                    //振った瞬間の経過フレームを取得
                    float fTmp = PlayerManagerObj.GetFourBeat( );

                    //現在の敵の該当フレームを求める
                    float fTargetFrame = fTargetFrameConst * ( PlayerManagerObj.GetTargetNo( ) - 1 );

                    if( Mathf.Abs( fTmp - fTargetFrame ) < fExcellentFrame )
                    {
                        ScoreManagerObj.Create( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                    }
                    else
                    {
                        ScoreManagerObj.Create( transform.position , ScoreManager.EVALUATION.EVALUATION_FINE );
                    }
                }
                else
                {
                   ScoreManagerObj.Create(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                }

                //コントローラを触れない様にする
                bPoseFlg = true;
            }
        }
    }


    //コントローラの判定を開始
    public void ReleasePoseFlg( )
    {
        bPoseFlg = false;
    }
}
