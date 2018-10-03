using System.Collections;
using UnityEngine;

public class CameraPerformance : MonoBehaviour
{
    public void PerformanceMove( )
    {
        this.transform.position += new Vector3( 0.0f , 0.0f , 0.1f );
    }
}
