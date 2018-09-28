using System.Collections;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public enum GAME_PHASE
    { 
        PHASE_BGM_START = 0        ,   //BGMを流す
        PHASE_ENEMY_APPEARANCE     ,   //敵の出現
        PHASE_COUNT_DOWN           ,   //カウントダウン
        PHASE_PLAYER_DANCE         ,   //プレイヤーのダンス
        PHASE_ENEMY_TAKE_IN        ,   //敵の取り込み(後ろに追従)
    };

    //ゲームの初期状態の設定
    public GAME_PHASE GamePhase = GAME_PHASE.PHASE_BGM_START;

    public GameObject BGMPrefab;            //BGMのプレハブ
    public GameObject EnemyManagerPrefab;   //エネミーマネージャのプレハブ
    public GameObject CountDownPrefab;      //カウントダウンのプレハブ
    public GameObject PlayerPrefab;         //プレイヤーのプレハブ


	void Update( )
    {
        //ゲームの進行状態によって遷移
        switch( GamePhase )
        {
             //BGMの再生
            case GAME_PHASE.PHASE_BGM_START :
                BGMPrefab.GetComponent< BGM >( ).EmitBGM( );
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
                PlayerPrefab.GetComponent< Player >( ).Dance( );
            break;

            //敵の取り込み
            case GAME_PHASE.PHASE_ENEMY_TAKE_IN :
                EnemyManagerPrefab.GetComponent< EnemyManager >( ).Kill( );
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
