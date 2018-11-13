using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Bonus : MonoBehaviour
{
    float fDist;
    float fGoal;
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

    public enum BONUS_STATE
    { 
        MOVE = 0 ,
        BIRIBIRI
    }

    BONUS_TYPE BonusType;
    BONUS_STATE BonusState;
    float fCntFrame;
    public float fFalseFrame;

    void Start( )
    {
         ManagerClass       = GameObject.Find( "GameManager" ).GetComponent< Manager>( );
         BonusManagerClass = ManagerClass.GetBonusManager( ).GetComponent< BonusManager >( );

        BonusState = BONUS_STATE.MOVE;
        fCntFrame = 0.0f;
    }


	void Update( )
    {
        switch (BonusState)
        {
            case BONUS_STATE.MOVE :

                this.transform.position -= new Vector3( 0.0f , 0.0f , fMove );

                if( Mathf.Abs( this.transform.position.z - fGoal ) <= 25.0f && BonusType == BONUS_TYPE.LEFT )
                { 
                    BonusManagerClass.SetBonusLeft( this.gameObject );
                //    Debug.Log( "左セット");
                }
                if( Mathf.Abs( this.transform.position.z - fGoal ) <= 25.0f && BonusType == BONUS_TYPE.CENTER )
                { 
                    BonusManagerClass.SetBonusCenter( this.gameObject );
                  //  Debug.Log( "真ん中セット");
                }
                if( Mathf.Abs( this.transform.position.z - fGoal ) <= 25.0f && BonusType == BONUS_TYPE.RIGHT )
                { 
                    BonusManagerClass.SetBonusRight( this.gameObject );  
                   // Debug.Log( "右セット");
                }

            break;

            case BONUS_STATE.BIRIBIRI:
                fCntFrame += Time.deltaTime;

                if( fCntFrame >= fFalseFrame )
                {
                    this.gameObject.SetActive( false );
                }
            break;
        }
	}


    public void SetState( int nBeat , float Dist , float Goal , BONUS_TYPE Bonus )
    {
      
        //遅い
        //早い
        //15
        //20
        fDist = Dist;
        fMove = ( fDist / ( 0.4615f * nBeat ) );
        fMove = ( fMove / 60.0f ) * ( fMove * 60.0f );
      
        //分かってる事
        //・距離
        //・何拍で到達するか
        //・何秒で到達するか

      //  Debug.Log( 0.52f * nBeat );

        fGoal = Goal;
        BonusType = Bonus;
    }

    public void SetBiriBiri( )
    {
        BonusState = BONUS_STATE.BIRIBIRI;
    } 
}
