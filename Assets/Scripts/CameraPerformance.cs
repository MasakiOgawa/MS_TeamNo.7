using System.Collections;
using UnityEngine;


public class CameraPerformance : MonoBehaviour
{
    public float fMove;   //カメラの移動量
    bool bMoveFlg;

    void Start( )
    {
		bMoveFlg = false;
	}


    //カメラの移動
    public void PerformanceMove( )
    {
        if( bMoveFlg == false )
        {
            this.transform.position += new Vector3( 0.0f , 0.0f , fMove );
        }
    }


    public void TruebMoveFlg( )
    {
        bMoveFlg = true;
    }
}
