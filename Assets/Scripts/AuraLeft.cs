using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraLeft : MonoBehaviour {

    public enum AuraPoint
    {
        POINT_NONE = 0 ,
        POINT_FIRST ,
        POINT_SECOND
    };

        Manager ManagerClass;   //マネージャのクラス
    public GameObject ManagerObj;     //マネージャのオブジェクト
    public GameObject Players; 

    AuraPoint Point;
    public GameObject AuraSpotObj;

    // Use this for initialization
    void Start () {
        ManagerClass = ManagerObj.GetComponent<Manager>();
        Point = AuraPoint.POINT_NONE;
    }
	
	// Update is called once per frame
	void Update () {
		if( ManagerClass.GetPhase( ) == Manager.GAME_PHASE.PHASE_COUNT_DOWN )
        {
            if (Point == AuraPoint.POINT_NONE && ManagerClass.GetfAuraLeftFrame() >= 2.65f )
            {
                Point = AuraPoint.POINT_FIRST;
                //オーラ生成
                AuraSpotObj.transform.position = new Vector3(-8.63f, 5.0f, Players.transform.position.z + 27.5f);
                AuraSpotObj.SetActive(true);
            }
            else if(Point == AuraPoint.POINT_FIRST && ManagerClass.GetfAuraLeftFrame() >= 2.65f + 0.4555f )
            {
                Point = AuraPoint.POINT_SECOND;
                //オーラ生成
                AuraSpotObj.transform.position = new Vector3(-7.03f, 5.0f, Players.transform.position.z + 27.5f);
            }
        }//2.45はやい
	}

    public void ResetPoint( )
    {
        Point = AuraPoint.POINT_NONE;
        AuraSpotObj.SetActive(false);
    }
}
