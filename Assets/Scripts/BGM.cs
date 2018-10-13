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
        audioSource.Play( );

        //最初のパフォーマンスに遷移
        ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_FIRST_PERFORMANCE );
    }


    //BPMの取得
    public float GetBPM( )
    {
        return 65.0f;
    }
}
