using System.Collections;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public GameObject ManagerObj;           //マネージャのオブジェクト
           EnemyManager EnemyManagerObj;      //エネミーマネージャのオブジェクト
           Rhythm     RhythmObj;            //リズムのオブジェクト
           GameObject PlayersObj;           //プレイヤー達のオブジェクト
    public GameObject PlayerLeftPrefab;     //左プレイヤーのプレハブ
    public GameObject PlayerCenterPrefab;   //中央プレイヤーのプレハブ
    public GameObject PlayerRightPrefab;    //右プレイヤーのプレハブ
           float      fBPM;                 //BPM
           int       nHalfBeat;            //半拍分のフレーム
      /*     float      fOneBeat;             //1拍分のフレーム
           float      fFourBeat;            //4拍分のフレーム*/
           float      fCntFrame;            //フレーム数のカウンタ(演出のフレーム数が決まっていない為、名前は適当)
           bool       bTargetChangeFlg;     //ターゲット切り替えのフラグ
           bool       bRhythmFlg;           //リズム再生のフラグ
           int        nTargetNo;            //現在のターゲット
    public float      fDist;                //プレイヤー達の移動距離
    public float      fMove;                //プレイヤー達の移動量
           int        nPerformanceTmp;

    PerformanceManager PerformanceManagerObj;
    PlayerLeft         PlayerLeftObj;
    PlayerRight        PlayerRightObj;
    PlayerCenter       PlayerCenterObj;

    Manager ManagerClass;
    int nCntRhythm;


	void Start( )
    {
		//変数の初期化
        fBPM             = 60.0f / ManagerObj.GetComponent< Manager >( ).GetBGM( ).GetComponent< BGM >( ).GetBPM( );
		nHalfBeat        = 0;
    /*    fOneBeat         = 0.0f;
        fFourBeat        = 0.0f;*/
        fCntFrame        = 0.0f;
        bTargetChangeFlg = false;  
        bRhythmFlg       = false;
        nTargetNo        = 0;
        fDist            = 0.0f;

        //エネミーマネージャのオブジェクトを取得
        EnemyManagerObj = ManagerObj.GetComponent< Manager >( ).GetEnemyManager( ).GetComponent< EnemyManager >( );

        //リズムのオブジェクトを取得
        RhythmObj = ManagerObj.GetComponent< Manager >( ).GetRhythm( ).GetComponent< Rhythm >( );

        //プレイヤー達のオブジェクトを取得
        PlayersObj = ManagerObj.GetComponent< Manager >( ).GetPlayers( );

        PerformanceManagerObj =  ManagerObj.GetComponent< Manager >( ).GetPerformanceManager( ).GetComponent< PerformanceManager >( );

        PlayerLeftObj = PlayerLeftPrefab.GetComponent< PlayerLeft >( );
        PlayerRightObj = PlayerRightPrefab.GetComponent< PlayerRight >( );
        PlayerCenterObj = PlayerCenterPrefab.GetComponent< PlayerCenter >( );

        ManagerClass = ManagerObj.GetComponent< Manager >( );
        nCntRhythm = 0;
	}
	

    //
    void Update( )
    {
        if( ManagerClass.GetPhase( ) == Manager.GAME_PHASE.PHASE_PLAYER_DANCE )
        {
             //プレイヤーの振りを検出
            PlayerLeftObj.Pose( );
            PlayerRightObj.Pose( );
            PlayerCenterObj.Pose( );
        }
    }


    //プレイヤーのダンス
    public void Dance( )
    {
        int nTmp = 28;

        if( nTargetNo == 6 )
        {
            nTmp = 27;
        }

         //半拍毎にターゲットを切り替える
        if( ManagerClass.GetlCntHalfFrame( ) >= nTmp || bTargetChangeFlg == false )
        {
            ManagerClass.ResetlCntHalfFrame( );
            bTargetChangeFlg = true;
            
            //次のターゲットを設定
            EnemyManagerObj.SetTarget( nTargetNo );
            nTargetNo++;

            //ジョイコンを振れる様にする
            PlayerLeftObj.ReleasePoseFlg( );
            PlayerRightObj.ReleasePoseFlg( );
            PlayerCenterObj.ReleasePoseFlg( );
        }

       //元々Poseがあった場所

           //1拍毎にリズムを鳴らす
        if( ManagerClass.GetlCntFrame( ) >= 55 ) 
        {
            nHalfBeat = 0;
            ManagerClass.ResetlCntFrame( );
            nCntRhythm++;
            bRhythmFlg = true;
            
            if( nCntRhythm < 4 )
            {
               RhythmObj.Emit( );
            }
        }

        //四拍でダンスの終了
        if( nCntRhythm >= 4 )
        {
            nCntRhythm = 0;
            ManagerClass.ResetlCntFrame( );
            ManagerClass.ResetPoseFrame( );
        
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
		//fCntFrame += Time.deltaTime;

        PlayersObj.transform.position += new Vector3( 0.0f , 0.0f , fMove );
        fDist += fMove;

        if( /*fCntFrame >= fBPM * nPerformanceTmp*/ ManagerClass.GetlCntFrame( ) > nPerformanceTmp * 55 )
        {
            //fCntFrame = 0.0f;

            if( PerformanceManagerObj.GetnCntPerformance( ) == 3 )
            {
                ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_BONUS );
            }
            else if( PerformanceManagerObj.GetnCntPerformance( ) == 6 )
            {
                ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_END_PERFORMANCE );
            }
            else
            {
                //敵を出現させる
                ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
                ManagerObj.GetComponent< Manager >( ).GetCountDown( ).GetComponent< CountDown >( ).SetText( );
            }

            ManagerClass.ResetlCntFrame( );
        }
    }


    //プレイヤーが振った瞬間のフレーム数を取得
    public float GetFourBeat( )
    {
        return 0.0f;//fFourBeat;
    }


    //ターゲットの添え字を取得
    public int GetTargetNo( )
    {
        return nTargetNo;
    }


    //パフォーマンスの小節数を設定
    public void SetnPerformanceTmp( int nPerformanceMeasure )
    {
        nPerformanceTmp = nPerformanceMeasure;
    }
}
