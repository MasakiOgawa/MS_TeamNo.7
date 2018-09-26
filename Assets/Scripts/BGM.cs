using System.Collections;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioClip          BGMData;       //BGM情報
    public static AudioSource audioSource;     
    public static int         nBpm;          //BPM


	void Start( )
    {
        //BPMを取得
        nBpm = UniBpmAnalyzer.AnalyzeBpm( BGMData );

        audioSource      = gameObject.GetComponent<AudioSource>( );
        audioSource.clip = BGMData;
	}


    //BGMを再生
    public static void EmitBGM( )
    {
        //BGMを再生
        audioSource.Play( );

        //敵を生成
        Manager.SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
    }


    //BPMを返す
    public static int GetBPM( )
    {
        return nBpm;
    }
}
