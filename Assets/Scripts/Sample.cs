using System.Collections;
using UnityEngine;

public class Sample : MonoBehaviour
{
	public AudioClip    audioClip1;
           AudioSource  audioSource;


    void Start( )
    {
        audioSource      = gameObject.GetComponent< AudioSource >( );
        audioSource.clip = audioClip1;
    }


    //メトロノームを鳴らす
    public void Emit( )
    {
        audioSource.Play( );
    }
}
