using System.Collections;
using UnityEngine;

public class PlayerRight : MonoBehaviour
{
    public GameObject PlayerManagerPrefab;   //プレイヤーマネージャのプレハブ
    public GameObject EnemyManagerPrefab;    //エネミーマネージャのプレハブ
    public GameObject ScoreManagerPrefab;    //スコアマネージャのプレハブ
           GameObject ObjectTmp;
    public bool       bPoseFlg;              //判定重複防止のフラグ
  

	void Start( )
    {
		bPoseFlg = false;
	}
	

	void Update( )
    {
	}


    public void Pose( )
    {
        //まだ振られていなかったら
        if( bPoseFlg == false )
        {
            //上
            if( PlayerManagerPrefab.GetComponent< Example_gyro >( ).GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R2 ) == Example_gyro.JOYCON_STATE.STATE_UP_TRIGGER )
            {
                //敵の番号を取得
                ObjectTmp = EnemyManagerPrefab.GetComponent< EnemyManager >( ).GetTarget( );
     
                //一致していたら
                if( ObjectTmp.tag == "Up" )//現在エクセレント
                {
                   ScoreManagerPrefab.GetComponent< ScoreManager >( ).Create( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                }
                else
                {
                   ScoreManagerPrefab.GetComponent< ScoreManager >( ).Create(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                }

                bPoseFlg = true;
            }
            //下
            else if( PlayerManagerPrefab.GetComponent< Example_gyro >( ).GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R2 ) == Example_gyro.JOYCON_STATE.STATE_DOWN_TRIGGER )
            {
                //敵の番号を取得
                ObjectTmp = EnemyManagerPrefab.GetComponent< EnemyManager >( ).GetTarget( );
     
                //一致していたら
                if( ObjectTmp.tag == "Down" )//現在エクセレント
                {
                   ScoreManagerPrefab.GetComponent< ScoreManager >( ).Create( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                }
                else
                {
                   ScoreManagerPrefab.GetComponent< ScoreManager >( ).Create(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                }

                bPoseFlg = true;
            }
            //左
            else if( PlayerManagerPrefab.GetComponent< Example_gyro >( ).GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R2 ) == Example_gyro.JOYCON_STATE.STATE_LEFT_TRIGGER )
            {
                //敵の番号を取得
                ObjectTmp = EnemyManagerPrefab.GetComponent< EnemyManager >( ).GetTarget( );
     
                //一致していたら
                if( ObjectTmp.tag == "Left" )//現在エクセレント
                {
                   ScoreManagerPrefab.GetComponent< ScoreManager >( ).Create( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                }
                else
                {
                   ScoreManagerPrefab.GetComponent< ScoreManager >( ).Create(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                }

                bPoseFlg = true;
            }
            //右
            else if( PlayerManagerPrefab.GetComponent< Example_gyro >( ).GetJoyconState( Example_gyro.JOYCON_TYPE.JOYCON_R2 ) == Example_gyro.JOYCON_STATE.STATE_RIGHT_TRIGGER )
            {
                //敵の番号を取得
                ObjectTmp = EnemyManagerPrefab.GetComponent< EnemyManager >( ).GetTarget( );
     
                //一致していたら
                if( ObjectTmp.tag == "Right" )//現在エクセレント
                {
                   ScoreManagerPrefab.GetComponent< ScoreManager >( ).Create( transform.position , ScoreManager.EVALUATION.EVALUATION_EXCELLENT );
                }
                else
                {
                   ScoreManagerPrefab.GetComponent< ScoreManager >( ).Create(transform.position , ScoreManager.EVALUATION.EVALUATION_BAD );
                }

                bPoseFlg = true;
            }
        }
    }


    public void ReleasePoseFlg( )
    {
        bPoseFlg = false;
    }
}
