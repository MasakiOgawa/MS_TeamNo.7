using System.Collections;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public GameObject  ManagerPrefab;   //マネージャ情報
    public AudioClip   BGMData;         //BGM情報
           AudioSource audioSource;     
           int         nBpm;            //BPM


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
      //  audioSource.Play( );

        //敵を生成
        ManagerPrefab.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
    }


    //BPMを返す
    public int GetBPM( )
    {
        return 60;
    }
}
