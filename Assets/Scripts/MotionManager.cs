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

        fRotY = 0.0f;
    }


    //モーションの切り替え
    public void ChangeAllMotion( PlayerAnimDefine.Idx idxAll )
    {
        PlayerLeftObj.transform.position = new Vector3( -3.0f ,0.0f , PlayerLeftObj.transform.position.z );
        PlayerCenterObj.transform.position = new Vector3( 0.0f ,0.0f , PlayerCenterObj.transform.position.z );
        PlayerRightObj.transform.position = new Vector3( 3.0f ,0.0f , PlayerRightObj.transform.position.z );
        PlayerLeftObj.transform.rotation = Quaternion.Euler( 0.0f , 0.0f , 0.0f );
        PlayerCenterObj.transform.rotation = Quaternion.Euler( 0.0f , 0.0f , 0.0f );
        PlayerRightObj.transform.rotation = Quaternion.Euler( 0.0f , 0.0f , 0.0f );
        PlayerLeftObj.GetComponent<Animator>().ForceStateNormalizedTime(0.0f);
        PlayerCenterObj.GetComponent<Animator>().ForceStateNormalizedTime(0.0f);
        PlayerRightObj.GetComponent<Animator>().ForceStateNormalizedTime(0.0f);
        LeftAnimClass.MotionChange( idxAll );
        CenterAnimClass.MotionChange( idxAll );
        RightAnimClass.MotionChange( idxAll );
    }


    public void ChangeLeftMotion( PlayerAnimDefine.Idx idx )
    {
        PlayerLeftObj.transform.position = new Vector3( -3.0f ,0.0f , PlayerLeftObj.transform.position.z );
        PlayerLeftObj.transform.rotation = Quaternion.Euler( 0.0f , 0.0f , 0.0f );
        LeftAnimClass.MotionChange( idx );
    }


    public void ChangeCenterMotion( PlayerAnimDefine.Idx idx )
    {
        PlayerCenterObj.transform.position = new Vector3( 0.0f ,0.0f , PlayerCenterObj.transform.position.z );
        PlayerCenterObj.transform.rotation = Quaternion.Euler( 0.0f , 0.0f , 0.0f );
        CenterAnimClass.MotionChange( idx );
    }


    public void ChangeRightMotion( PlayerAnimDefine.Idx idx )
    {
        PlayerRightObj.transform.position = new Vector3( 3.0f ,0.0f , PlayerRightObj.transform.position.z );
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


    public void ApplyFalse( )
    {
        PlayerLeftObj.GetComponent< Animator >( ).applyRootMotion = false;
        PlayerCenterObj.GetComponent< Animator >( ).applyRootMotion = false;
        PlayerRightObj.GetComponent< Animator >( ).applyRootMotion = false;
    }


    public void ApplyTrue( )
    {
        PlayerLeftObj.GetComponent< Animator >( ).applyRootMotion = true;
        PlayerCenterObj.GetComponent< Animator >( ).applyRootMotion = true;
        PlayerRightObj.GetComponent< Animator >( ).applyRootMotion = true;
    }
}
