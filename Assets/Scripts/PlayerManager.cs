using System.Collections;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public GameObject         ManagerObj;                //マネージャのオブジェクト
           Manager            ManagerClass;              //マネージャのクラス
           GameObject         PlayersObj;                //プレイヤー達のオブジェクト
           PlayerLeft         PlayerLeftClass;           //左プレイヤーのクラス
           PlayerCenter       PlayerCenterClass;         //中央プレイヤーのクラス
           PlayerRight        PlayerRightClass;          //右プレイヤーのクラス
    public GameObject         PlayerLeftPrefab;          //左プレイヤーのプレハブ
    public GameObject         PlayerCenterPrefab;        //中央プレイヤーのプレハブ
    public GameObject         PlayerRightPrefab;         //右プレイヤーのプレハブ 
           EnemyManager       EnemyManagerClass;         //エネミーマネージャのクラス
           Rhythm             RhythmClass;               //リズムのクラス
           PerformanceManager PerformanceManagerClass;   //パフォーマンスマネージャのクラス
           bool               bTargetChangeFlg;          //ターゲット切り替えのフラグ
           int                nTargetNo;                 //現在のターゲット
    public float              fMove;                     //プレイヤー達の移動量
    public float              fDist;                     //プレイヤー達の移動距離
           int                nCntRhythm;                //リズムが鳴った回数
           int                nPerformanceBar;           //パフォーマンスの小節数
    public float              fHalfBar;                  //半拍分のリズム


	void Start( )
    {
        //変数の初期化
        bTargetChangeFlg = false;  
        nTargetNo        = 0;
        fDist            = 0.0f;
        nCntRhythm       = 0;

        //各クラスの情報を取得
        ManagerClass            = ManagerObj.GetComponent< Manager >( );
        EnemyManagerClass       = ManagerClass.GetEnemyManager( ).GetComponent< EnemyManager >( );
        RhythmClass             = ManagerClass.GetRhythm( ).GetComponent< Rhythm >( );
        PlayerLeftClass         = PlayerLeftPrefab.GetComponent< PlayerLeft >( );
        PlayerCenterClass       = PlayerCenterPrefab.GetComponent< PlayerCenter >( );
        PlayerRightClass        = PlayerRightPrefab.GetComponent< PlayerRight >( );
        PerformanceManagerClass =  ManagerClass.GetPerformanceManager( ).GetComponent< PerformanceManager >( );
        
        //プレイヤー達のオブジェクトを取得
        PlayersObj = ManagerClass.GetPlayers( );   
	}
	

    //プレイヤー達の入力処理
    void Update( )
    {
        //ダンス状態ならプレイヤーの入力を検出
        if( ManagerClass.GetPhase( ) == Manager.GAME_PHASE.PHASE_PLAYER_DANCE )
        {
            PlayerLeftClass.Pose( );
            PlayerCenterClass.Pose( );
            PlayerRightClass.Pose( );
        }
    }


    //プレイヤーのダンス
    public void Dance( )
    {
         //半拍毎にターゲットを切り替える
        if( ManagerClass.GetfCntHalfFrame( ) >= fHalfBar || bTargetChangeFlg == false )
        {
            bTargetChangeFlg = true;

            ManagerClass.ResetfCntHalfFrame( );//???
            
            //次のターゲットを設定
            EnemyManagerClass.SetTarget( nTargetNo );
            nTargetNo++;

            //ジョイコンを振れる様にする
            PlayerLeftClass.ReleasebPoseFlg( );
            PlayerCenterClass.ReleasebPoseFlg( );
            PlayerRightClass.ReleasebPoseFlg( );
        }

        //1拍毎にリズムを鳴らす
        if( ManagerClass.GetfCntFrame( ) >= 0.92286395f )
        {
            nCntRhythm++;

            if( nCntRhythm < 4 )
            {
               RhythmClass.Emit( );
            }

            ManagerClass.ResetfCntFrame( );//???
        }

        //四拍でダンスの終了
        if( nCntRhythm >= 4 )
        {
            nCntRhythm       = 0;
            nTargetNo        = 0;
            bTargetChangeFlg = false;

            ManagerClass.ResetfPoseFrame( );//???

            //スコアの集計
            ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_AGGREGATE_SCORE );
        }
    }


    //プレイヤー達の移動
    public void PlayersMove( )
    {
        PlayersObj.transform.position += new Vector3( 0.0f , 0.0f , fMove );
        fDist                         += fMove;

        //パフォーマンスが終了したら
        if( ManagerClass.GetfCntFrame( ) > nPerformanceBar * 0.92286395f )
        {
            //現在のパフォーマンスによって遷移先を決める
            if( PerformanceManagerClass.GetnCntPerformance( ) == 3 )
            {
                //ボーナス
                ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_BONUS );
            }
            else if( PerformanceManagerClass.GetnCntPerformance( ) == 6 )
            {
                //最後のパフォーマンス
                ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_END_PERFORMANCE );
            }
            else
            {
                //敵を出現させる
                ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
                ManagerClass.GetCountDown( ).GetComponent< CountDown >( ).ActiveCountDown( );
            }

            ManagerClass.ResetfCntFrame( );//???
        }
    }


    //ターゲットの番号を取得
    public int GetnTargetNo( )
    {
        return nTargetNo;
    }


    //パフォーマンスの小節数を設定
    public void SetnPerformanceBar( int nSetPerformanceBar )
    {
        nPerformanceBar = nSetPerformanceBar;
    }
}
