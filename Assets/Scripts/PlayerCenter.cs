using System.Collections;
using UnityEngine;


public class PlayerCenter : MonoBehaviour
{
    public GameObject ManagerObj;         //マネージャのオブジェクト
           GameObject PlayerManagerObj;   //プレイヤーマネージャのオブジェクト
           GameObject EnemyManagerObj;    //エネミーマネージャのオブジェクト
           GameObject ScoreManagerObj;    //スコアマネージャのオブジェクト
    public bool       bPoseFlg;           //判定重複防止のフラグ
           GameObject TmpObj;             //保存用のオブジェクト変数
   
	void Start( )
    {
        //変数の初期化
		bPoseFlg = false;

        //プレイヤーマネージャのオブジェクトを取得
        PlayerManagerObj = ManagerObj.GetComponent< Manager >( ).GetPlayerManager( );

        //エネミーーマネージャのオブジェクトを取得
        EnemyManagerObj = ManagerObj.GetComponent< Manager >( ).GetEnemyManager( );

        //スコアマネージャのオブジェクトを取得
        ScoreManagerObj = ManagerObj.GetComponent< Manager >( ).GetScoreManager( );
	}
	

    //プレイヤーのダンス
    public void Pose( )
    {
        //まだ振られていなかったら
        if( bPoseFlg == false )
        {
            //上
            if( PlayerManagerObj.GetComponent< Example_gyro >( ).GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R1 ) == Example_gyro.JOYCON_STATE.STATE_UP_TRIGGER )
            {
                //敵の番号を取得
                TmpObj = EnemyManagerObj.GetComponent< EnemyManager >( ).GetTarget( );
     
                //一致していたら
                if( TmpObj.tag == "Up" )//現在エクセレント
                {
                   ScoreManagerObj.GetComponent< ScoreManager >( ).Create( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                }
                else
                {
                   ScoreManagerObj.GetComponent< ScoreManager >( ).Create(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                }

                bPoseFlg = true;
            }
            //下
            else if( PlayerManagerObj.GetComponent< Example_gyro >( ).GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R1 ) == Example_gyro.JOYCON_STATE.STATE_DOWN_TRIGGER )
            {
                //敵の番号を取得
                TmpObj = EnemyManagerObj.GetComponent< EnemyManager >( ).GetTarget( );
     
                //一致していたら
                if( TmpObj.tag == "Down" )//現在エクセレント
                {
                    ScoreManagerObj.GetComponent< ScoreManager >( ).Create( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                }
                else
                {
                    ScoreManagerObj.GetComponent< ScoreManager >( ).Create(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                }

                bPoseFlg = true;
            }
            //左
            else if( PlayerManagerObj.GetComponent< Example_gyro >( ).GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R1 ) == Example_gyro.JOYCON_STATE.STATE_LEFT_TRIGGER )
            {
                //敵の番号を取得
                TmpObj = EnemyManagerObj.GetComponent< EnemyManager >( ).GetTarget( );
     
                //一致していたら
                if( TmpObj.tag == "Left" )//現在エクセレント
                {
                   ScoreManagerObj.GetComponent< ScoreManager >( ).Create( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                }
                else
                {
                   ScoreManagerObj.GetComponent< ScoreManager >( ).Create(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                }

                bPoseFlg = true;
            }
            //右
            else if( PlayerManagerObj.GetComponent< Example_gyro >( ).GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R1 ) == Example_gyro.JOYCON_STATE.STATE_RIGHT_TRIGGER )
            {
                //敵の番号を取得
                TmpObj = EnemyManagerObj.GetComponent< EnemyManager >( ).GetTarget( );
     
                //一致していたら
                if( TmpObj.tag == "Right" )//現在エクセレント
                {
                   ScoreManagerObj.GetComponent< ScoreManager >( ).Create( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                }
                else
                {
                   ScoreManagerObj.GetComponent< ScoreManager >( ).Create(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
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
