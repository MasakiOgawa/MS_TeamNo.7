﻿using System.Collections;
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
        TARGET ,
        OUT ,
        BIRIBIRI
    }

    BONUS_TYPE BonusType;
    public BONUS_STATE BonusState;
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

                if( Mathf.Abs( this.transform.position.z - fGoal ) <= 1.5f && BonusType == BONUS_TYPE.LEFT )
                { 
                    BonusManagerClass.SetBonusLeft( this.gameObject );
                    BonusManagerClass.LeftAreaChange( false );
                    BonusState = BONUS_STATE.TARGET;
                }

                if( Mathf.Abs( this.transform.position.z - fGoal ) <= 1.5f && BonusType == BONUS_TYPE.CENTER )
                { 
                    BonusManagerClass.SetBonusCenter( this.gameObject );
                    BonusManagerClass.CenterAreaChange( false );
                    BonusState = BONUS_STATE.TARGET;
                } 

                if( Mathf.Abs( this.transform.position.z - fGoal ) <= 1.5f && BonusType == BONUS_TYPE.RIGHT )
                { 
                    BonusManagerClass.SetBonusRight( this.gameObject );  
                    BonusManagerClass.RightAreaChange( false );
                    BonusState = BONUS_STATE.TARGET;
                }
             
            break;

            case BONUS_STATE.TARGET :

                this.transform.position -= new Vector3( 0.0f , 0.0f , fMove );

                if( Mathf.Abs( this.transform.position.z - fGoal ) > 1.5f && BonusType == BONUS_TYPE.LEFT )
                { 
                    BonusManagerClass.LeftAreaChange( true );
                    BonusState = BONUS_STATE.OUT;
                }
                if( Mathf.Abs( this.transform.position.z - fGoal ) > 1.5f && BonusType == BONUS_TYPE.CENTER )
                { 
                    BonusManagerClass.CenterAreaChange( true );
                    BonusState = BONUS_STATE.OUT;
                }
                if( Mathf.Abs( this.transform.position.z - fGoal ) > 1.5f && BonusType == BONUS_TYPE.RIGHT )
                { 
                    BonusManagerClass.RightAreaChange( true );
                    BonusState = BONUS_STATE.OUT;
                }

            break;

            case BONUS_STATE.OUT:
                this.transform.position -= new Vector3( 0.0f , 0.0f , fMove );
            break;

            case BONUS_STATE.BIRIBIRI:
                fCntFrame += Time.deltaTime;

                if( fCntFrame >= fFalseFrame )
                {
                    BonusState = BONUS_STATE.OUT;
                }
            break;
        }
	}


    public void SetState( int nBeat , float Dist , float Goal , BONUS_TYPE Bonus )
    {
        fDist = Dist;
        //0.3
        //0.25
        //0.23
        //0.18fで前半早い・後半遅い
        //0.178
        fMove = ( fDist / ( 0.175f * 3.0f ) ) / 60.0f;
      
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
