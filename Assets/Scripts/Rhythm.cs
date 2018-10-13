using System.Collections;
using UnityEngine;


public class Rhythm : MonoBehaviour
{
	public AudioClip   audioClip;
           AudioSource audioSource;


    void Start( )
    {
        audioSource      = gameObject.GetComponent< AudioSource >( );
        audioSource.clip = audioClip;
    }


    //リズムを鳴らす
    public void Emit( )
    {
        audioSource.Play( );
    }
}
