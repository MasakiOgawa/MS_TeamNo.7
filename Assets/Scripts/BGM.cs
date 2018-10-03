using System.Collections;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public GameObject  ManagerPrefab;     //マネージャ情報
    public GameObject  CountDownPrefab;   //カウントダウンのプレハブ
    public AudioClip   BGMData;           //BGM情報
           AudioSource audioSource;     
           int         nBpm;              //BPM


	void Start( )
    {
        //BPMを取得
        nBpm = UniBpmAnalyzer.AnalyzeBpm( BGMData );

        audioSource      = gameObject.GetComponent< AudioSource >( );
        audioSource.clip = BGMData;
	}


    //BGMを再生
    public void EmitBGM( )
    {
        //BGMを再生
        audioSource.Play( );

        //敵を生成
        ManagerPrefab.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
    }


    //BPMの取得
    public int GetBPM( )
    {
        return 60;
    }


    //曲が終了しているかをチェック
    public void CheckEndBGM( )
    {
        if( audioSource.isPlaying == false )
        {
            ManagerPrefab.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_END_PERFORMANCE );
        }
        else if( audioSource.isPlaying == true &&
                 ManagerPrefab.GetComponent< Manager >( ).GetPhase( ) != Manager.GAME_PHASE.PHASE_END_PERFORMANCE )
        {
            //ManagerPrefab.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
            //CountDownPrefab.GetComponent< CountDown >( ).SetText( );

            //実験！！ここで常にカメラパフォーマンスに移す！
            ManagerPrefab.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_CAMERA_PERFORMANCE );
        }
    }
}
