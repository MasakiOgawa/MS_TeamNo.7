using System.Collections;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public GameObject ManagerObj;           //マネージャのオブジェクト
           GameObject EnemyManagerObj;      //エネミーマネージャのオブジェクト
           GameObject RhythmObj;            //リズムのオブジェクト
           GameObject PlayersObj;           //プレイヤー達のオブジェクト
    public GameObject PlayerLeftPrefab;     //左プレイヤーのプレハブ
    public GameObject PlayerCenterPrefab;   //中央プレイヤーのプレハブ
    public GameObject PlayerRightPrefab;    //右プレイヤーのプレハブ
           float      fBPM;                 //BPM
           float      fHalfBeat;            //半拍分のフレーム
           float      fOneBeat;             //1拍分のフレーム
           float      fFourBeat;            //4拍分のフレーム
           float      fCntFrame;            //フレーム数のカウンタ(演出のフレーム数が決まっていない為、名前は適当)
           bool       bTargetChangeFlg;     //ターゲット切り替えのフラグ
           bool       bRhythmFlg;           //リズム再生のフラグ
           int        nTargetNo;            //現在のターゲット
    public float      fDist;                //プレイヤー達の移動距離
    public float      fMove;                //プレイヤー達の移動量
        

	void Start( )
    {
		//変数の初期化
        fBPM             = ManagerObj.GetComponent< Manager >( ).GetBGM( ).GetComponent< BGM >( ).GetBPM( );
		fHalfBeat        = 0.0f;
        fOneBeat         = 0.0f;
        fFourBeat        = 0.0f;
        fCntFrame        = 0.0f;
        bTargetChangeFlg = false;  
        bRhythmFlg       = false;
        nTargetNo        = 0;
        fDist            = 0.0f;

        //エネミーーマネージャのオブジェクトを取得
        EnemyManagerObj = ManagerObj.GetComponent< Manager >( ).GetEnemyManager( );

        //リズムのオブジェクトを取得
        RhythmObj = ManagerObj.GetComponent< Manager >( ).GetRhythm( );

        //プレイヤー達のオブジェクトを取得
        PlayersObj = ManagerObj.GetComponent< Manager >( ).GetPlayers( );
	}
	

    //プレイヤーのダンス
    public void Dance( )
    {
        //フレーム数を計測
        float fTmp = Time.deltaTime;
        fHalfBeat += fTmp;
        fOneBeat  += fTmp;
		fFourBeat += fTmp;
        
         //半拍毎にターゲットを切り替える
        if( fHalfBeat >= ( 60.0f / fBPM ) * 0.5f || bTargetChangeFlg == false )
        {
            fHalfBeat        = 0.0f;
            bTargetChangeFlg = true;
            
            //次のターゲットを設定
            EnemyManagerObj.GetComponent< EnemyManager >( ).SetTarget( nTargetNo );
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

        //1拍毎にリズムを鳴らす
        if( fOneBeat >= 60.0f / fBPM || bRhythmFlg == false )
        {
            fOneBeat   = 0.0f;
            bRhythmFlg = true;
            
            RhythmObj.GetComponent< Rhythm >( ).Emit( );
        }

        //四拍でダンスの終了
        if( fFourBeat >= ( 60.0f / fBPM ) * 4.0f )
        {
            fHalfBeat        = 0.0f;
            fFourBeat        = 0.0f;
            bTargetChangeFlg = false;
            bRhythmFlg       = false;
            nTargetNo        = 0;

            //スコアの集計
            ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_AGGREGATE_SCORE );
        }
    }


    //プレイヤー達の移動
    public void PlayersMove( )
    {
		fCntFrame += Time.deltaTime;

        PlayersObj.transform.position += new Vector3( 0.0f , 0.0f , fMove );
        fDist += fMove;

        if( fCntFrame >= ( 60.0f / fBPM ) * 8.0f )
        {
            fCntFrame = 0.0f;

            //敵を出現させる
            ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
            ManagerObj.GetComponent< Manager >( ).GetCountDown( ).GetComponent< CountDown >( ).SetText( );
        }
    }
}
