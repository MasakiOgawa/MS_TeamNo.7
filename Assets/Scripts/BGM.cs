using System.Collections;
using UnityEngine;


public class BGM : MonoBehaviour
{
    public GameObject  ManagerObj;   //マネージャのオブジェクト
    public AudioClip   BGMData;      //BGM情報
           AudioSource audioSource;     


	void Start( )
    {
        audioSource      = gameObject.GetComponent< AudioSource >( );
        audioSource.clip = BGMData;
	}


    //BGMを再生
    public void EmitBGM( )
    {
        //BGMを再生
       // audioSource.Play( );

        //敵を出現させる
        ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
    }


    //BPMの取得
    public int GetBPM( )
    {
        return 60;
    }


    //曲が終了しているかをチェック
    public void CheckEndBGM( )
    {
        //BGMが止まったら
        if( audioSource.isPlaying == false )
        {
            //実験！！
            //ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_END_PERFORMANCE );
             ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_CAMERA_PERFORMANCE );
        }
        //BGMが止まっていなければ
        else if( ManagerObj.GetComponent< Manager >( ).GetPhase( ) != Manager.GAME_PHASE.PHASE_END_PERFORMANCE )
        {
            //ManagerPrefab.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
            //CountDownPrefab.GetComponent< CountDown >( ).SetText( );

            //実験！！ここで常にカメラパフォーマンスに移す！
            ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_CAMERA_PERFORMANCE );
        }
    }
}
