using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public GameObject Dance_01Obj;
    public GameObject Dance_02Obj;


    public void TrueTutorial01( )
    {
        Dance_01Obj.SetActive( true );
    }


    public void TrueTutorial02( )
    {
        Dance_02Obj.SetActive( true );
    }


    public void FalseTutorial01( )
    {
        Dance_01Obj.SetActive( false );
    }


    public void FalseTutorial02( )
    {
        Dance_02Obj.SetActive( false );
    }
}
