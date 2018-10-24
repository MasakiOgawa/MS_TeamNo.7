using System.Collections;
using UnityEngine;


public class BGM : MonoBehaviour
{
    public AudioClip   BGMData;       //BGM情報
           AudioSource AudioSource;       


	void Start( )
    {
        //BGM情報を取得
        AudioSource      = gameObject.GetComponent< AudioSource >( );
        AudioSource.clip = BGMData;
	}


    //BGMを再生
    public void EmitBGM( )
    {
       AudioSource.Play( );
    }


    //BGMの再生位置を設定
    public void SetBGM( float fBGMTime )
    {
        AudioSource.time = fBGMTime;
    }
}
