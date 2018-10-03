using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public enum GAME_PHASE
    { 
        PHASE_BGM_START            ,   //BGMを流す
        PHASE_FIRST_PERFORMANCE    ,   //ゲーム開始時のパフォーマンス
        PHASE_ENEMY_APPEARANCE     ,   //敵の出現
        PHASE_COUNT_DOWN           ,   //カウントダウン
        PHASE_PLAYER_DANCE         ,   //プレイヤーのダンス
        PHASE_ADD_SCORE            ,   //スコア
        PHASE_CAMERA_PERFORMANCE   ,   //カメラのパフォーマンス
        PHASE_BGM_END_CHECK        ,   //BGMの終了状態をチェック
        PHASE_END_PERFORMANCE          //ゲーム終了時のパフォーマンス
    };

    //ゲームの初期状態の設定
    public GAME_PHASE GamePhase = GAME_PHASE.PHASE_BGM_START;

    public GameObject BGMPrefab;             //BGMのプレハブ
    public GameObject EnemyManagerPrefab;    //エネミーマネージャのプレハブ
    public GameObject CountDownPrefab;       //カウントダウンのプレハブ
    public GameObject PlayerManagerPrefab;   //プレイヤーマネージャのプレハブ
    public GameObject ScoreManagerPrefab;    //スコアマネージャのプレハブ
     

	void Update( )
    {
        //ゲームの進行状態によって遷移
        switch( GamePhase )
        {
            //BGMの再生
            case GAME_PHASE.PHASE_BGM_START :
                BGMPrefab.GetComponent< BGM >( ).EmitBGM( );
            break;

            //ゲーム開始時の演出
            case GAME_PHASE.PHASE_FIRST_PERFORMANCE :
            break;

            //敵の生成
            case GAME_PHASE.PHASE_ENEMY_APPEARANCE :
                EnemyManagerPrefab.GetComponent< EnemyManager >( ).Create( );
            break;

            //カウントダウン
            case GAME_PHASE.PHASE_COUNT_DOWN :
                CountDownPrefab.GetComponent< CountDown >( ).CountFrame( );
            break;

            //プレイヤーのダンス
            case GAME_PHASE.PHASE_PLAYER_DANCE :
                PlayerManagerPrefab.GetComponent< PlayerManager >( ).Dance( );
            break;

             //スコアの加算
            case GAME_PHASE.PHASE_ADD_SCORE :
                 ScoreManagerPrefab.GetComponent< ScoreManager >( ).AddScore( );
            break;

            //カメラのパフォーマンス
            case GAME_PHASE.PHASE_CAMERA_PERFORMANCE :
            break;

            //BGMの終了状態をチェック
            case GAME_PHASE.PHASE_BGM_END_CHECK :
                 BGMPrefab.GetComponent< BGM >( ).CheckEndBGM( );
            break;
         
            //ゲーム終了時の演出(ここから遷移)
            case GAME_PHASE.PHASE_END_PERFORMANCE :
                 SceneManager.LoadScene( "Result" );
            break;
        }
	}


    //ゲームの進行状態を設定
    public void SetPhase( GAME_PHASE Phase )
    {
        GamePhase = Phase;
    }


    //ゲームの進行状態を取得
    public GAME_PHASE GetPhase( )
    {
       return GamePhase;
    }
}
