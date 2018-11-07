using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Bonus : MonoBehaviour
{
    Vector3 Vec;
    Vector3 Goal;
    float   fMove;
     Manager       ManagerClass;         //マネージャのクラス
    BonusManager BonusManagerClass;

     //列挙型定義
    public enum BONUS_TYPE
    { 
        LEFT = 0 ,
        CENTER ,
        RIGHT
    }

    BONUS_TYPE BonusType;

    void Start( )
    {
          ManagerClass       = GameObject.Find( "GameManager" ).GetComponent< Manager>( );
         BonusManagerClass = ManagerClass.GetBonusManager( ).GetComponent< BonusManager >( );
    }


	void Update( )
    {
		this.transform.position += new Vector3( Vec.x * fMove , Vec.y * fMove , Vec.z * fMove );

        if( Vector3.Distance( this.transform.position , Goal ) <= 25.0f && BonusType == BONUS_TYPE.LEFT )
        { 
            BonusManagerClass.SetBonusLeft( this.gameObject );
        }
        if( Vector3.Distance( this.transform.position , Goal ) <= 25.0f && BonusType == BONUS_TYPE.CENTER )
        { 
            BonusManagerClass.SetBonusCenter( this.gameObject );
        }
        if( Vector3.Distance( this.transform.position , Goal ) <= 25.0f && BonusType == BONUS_TYPE.RIGHT )
        { 
            BonusManagerClass.SetBonusRight( this.gameObject );
        }
	}


    public void SetState( Vector3 Dist , Vector3 VecGoal , BONUS_TYPE Bonus )
    {
       // Vec     = 
        Vec     = new Vector3( Dist.x , Dist.y , Dist.z );
        Vec     = Vec.normalized;
        fMove   = Vec.magnitude / 0.4475f;
        Goal    = VecGoal;
        BonusType = Bonus;
    }
}
