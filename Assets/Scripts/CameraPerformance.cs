using System.Collections;
using UnityEngine;


public class CameraPerformance : MonoBehaviour
{
    public float fMove;   //カメラの移動量


    //カメラの移動
    public void PerformanceMove( )
    {
        this.transform.position += new Vector3( 0.0f , 0.0f , fMove );
    }
}
