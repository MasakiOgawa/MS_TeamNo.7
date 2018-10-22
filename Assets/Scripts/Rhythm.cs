using System.Collections;
using UnityEngine;


public class Rhythm : MonoBehaviour
{
	public AudioClip   AudioClip;
           AudioSource AudioSource;


    void Start( )
    {
        //SEの情報を取得
        AudioSource      = gameObject.GetComponent< AudioSource >( );
        AudioSource.clip = AudioClip;
    }


    //リズムを鳴らす
    public void Emit( )
    {
        AudioSource.Play( );
    }
}
