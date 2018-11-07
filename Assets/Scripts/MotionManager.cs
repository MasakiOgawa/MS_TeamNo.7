using System.Collections;
using UnityEngine;


public class MotionManager : MonoBehaviour
{
    public GameObject PlayerLeftObj;
    public GameObject PlayerCenterObj;
    public GameObject PlayerRightObj;

    PlayerAnim LeftAnimClass;
    PlayerAnim CenterAnimClass;
    PlayerAnim RightAnimClass;

    public float fAddRotateY;
    float fRotY;
    


	void Start( )
    {
		LeftAnimClass = PlayerLeftObj.GetComponent< PlayerAnim >( );
        CenterAnimClass = PlayerCenterObj.GetComponent< PlayerAnim >( );
        RightAnimClass = PlayerRightObj.GetComponent< PlayerAnim >( );
    }


    //モーションの切り替え
    public void ChangeAllMotion( PlayerAnimDefine.Idx idxAll )
    {
        PlayerLeftObj.transform.rotation = Quaternion.Euler( 0.0f , 0.0f , 0.0f );
        PlayerCenterObj.transform.rotation = Quaternion.Euler( 0.0f , 0.0f , 0.0f );
        PlayerRightObj.transform.rotation = Quaternion.Euler( 0.0f , 0.0f , 0.0f );
        LeftAnimClass.MotionChange( idxAll );
        CenterAnimClass.MotionChange( idxAll );
        RightAnimClass.MotionChange( idxAll );
    }


    public void ChangeLeftMotion( PlayerAnimDefine.Idx idx )
    {
        PlayerLeftObj.transform.rotation = Quaternion.Euler( 0.0f , 0.0f , 0.0f );
        LeftAnimClass.MotionChange( idx );
    }


    public void ChangeCenterMotion( PlayerAnimDefine.Idx idx )
    {
        PlayerCenterObj.transform.rotation = Quaternion.Euler( 0.0f , 0.0f , 0.0f );
        CenterAnimClass.MotionChange( idx );
    }


    public void ChangeRightMotion( PlayerAnimDefine.Idx idx )
    {
        PlayerRightObj.transform.rotation = Quaternion.Euler( 0.0f , 0.0f , 0.0f );
        RightAnimClass.MotionChange( idx );
    }


    public void HeadDanceRotate( )
    {
        fRotY += fAddRotateY;
        PlayerLeftObj.transform.rotation = Quaternion.Euler( 0.0f , fRotY , 0.0f );
        PlayerCenterObj.transform.rotation = Quaternion.Euler( 0.0f , fRotY , 0.0f );
        PlayerRightObj.transform.rotation = Quaternion.Euler( 0.0f , fRotY , 0.0f );
    }
}
