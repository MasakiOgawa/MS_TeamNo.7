using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject ManagerPrefab;         //マネージャのプレハブ
    public GameObject BGMPrefab;             //BGMのプレハブ
    public GameObject EnemyManagerPrefab;    //エネミーマネージャのプレハブ
    public GameObject SamplePrefab;   
    public GameObject ScoreManagerPrefab;    //スコアマネージャのプレハブ
    public GameObject PlayerLeftPrefab;      //左プレイヤーのプレハブ
    public GameObject PlayerCenterPrefab;    //中央プレイヤーのプレハブ
    public GameObject PlayerRightPrefab;     //右プレイヤーのプレハブ
           float      fCntFrame;             //経過フレーム
           float      fCntFrame2;            //経過フレーム
           float      fCntFrame3;            //経過フレーム
           bool       bEmitFlg;              //メトロノーム再生のフラグ
           bool       bTargetFlg;            //ターゲット切り替えのフラグ
           int        nTargetNo;             //現在のターゲット


	void Start( )
    {
		//変数の初期化
		fCntFrame  = 0.0f;
        fCntFrame2 = 0.0f;
        fCntFrame3 = 0.0f;
        bEmitFlg   = false;
        bTargetFlg = false;
        nTargetNo  = 0;
	}
	

    public void Dance( )
    {
        //フレーム数を計測
        float fTmp  = Time.deltaTime;
		fCntFrame  += fTmp;
        fCntFrame2 += fTmp;
        fCntFrame3 += fTmp;

         //半拍毎にターゲットを切り替える
        if( fCntFrame3 >= ( 60.0f / ( float )BGMPrefab.GetComponent< BGM >( ).GetBPM( ) ) / 2.0f || bTargetFlg == false )
        {
            bTargetFlg = true;
            fCntFrame3 = 0.0f;
            EnemyManagerPrefab.GetComponent< EnemyManager >( ).SetTarget( nTargetNo );
            nTargetNo++;

            //ジョイコンを振れる様にする
            PlayerLeftPrefab.GetComponent< PlayerLeft >( ).ReleasePoseFlg( );
            PlayerCenterPrefab.GetComponent< PlayerCenter >( ).ReleasePoseFlg( );
            PlayerRightPrefab.GetComponent< PlayerRight >( ).ReleasePoseFlg( );
        }

        //プレイヤーの振りを検出
        PlayerLeftPrefab.GetComponent< PlayerLeft >( ).Pose( );
        PlayerCenterPrefab.GetComponent< PlayerCenter >( ).Pose( );
        PlayerRightPrefab.GetComponent< PlayerRight >( ).Pose( );

        //一拍毎にメトロノームを鳴らす
        if( fCntFrame2 >= 60.0f / ( float )BGMPrefab.GetComponent< BGM >( ).GetBPM( ) || bEmitFlg == false )
        {
            bEmitFlg   = true;
            fCntFrame2 = 0.0f;
            SamplePrefab.GetComponent< Sample >( ).Emit( );
        }

        //四拍でダンスの終了
        if( fCntFrame >= ( 60.0f / ( float )BGMPrefab.GetComponent< BGM >( ).GetBPM( ) ) * 4.0f )
        {
            fCntFrame  = 0.0f;
            fCntFrame3 = 0.0f;
            bEmitFlg   = false;
            bTargetFlg = false;
            nTargetNo  = 0;

            //次の敵を生成
            ManagerPrefab.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_TAKE_IN );
        }
    }
}
