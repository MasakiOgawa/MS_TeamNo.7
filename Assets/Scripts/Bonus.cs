using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Bonus : MonoBehaviour
{
    float fDist;
    float fGoal;
    public float fMove;
    Manager      ManagerClass;         //マネージャのクラス
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

    bool bFlg;


    void Start( )
    {
         ManagerClass       = GameObject.Find( "GameManager" ).GetComponent< Manager>( );
         BonusManagerClass = ManagerClass.GetBonusManager( ).GetComponent< BonusManager >( );

        BonusState = BONUS_STATE.MOVE;
        fCntFrame = 0.0f;

        bFlg = false;
    }


	void Update( )
    {
        if( ManagerClass.GetPhase( ) == Manager.GAME_PHASE.PHASE_BONUS )
        {
            switch( BonusState )
            {
                case BONUS_STATE.MOVE :

                    this.transform.position -= new Vector3( 0.0f , 0.0f , fMove );

                    if( this.transform.position.z - 75.0f <= 3.0f && BonusType == BONUS_TYPE.LEFT )
                    { 
                        BonusManagerClass.SetBonusLeft( this.gameObject );
                        BonusManagerClass.LeftAreaChange( false );
                        BonusState = BONUS_STATE.TARGET;
                    }

                    if( this.transform.position.z - 75.0f <= 3.0f && BonusType == BONUS_TYPE.CENTER )
                    { 
                        BonusManagerClass.SetBonusCenter( this.gameObject );
                        BonusManagerClass.CenterAreaChange( false );
                        BonusState = BONUS_STATE.TARGET;
                    } 

                    if( this.transform.position.z - 75.0f <= 3.0f && BonusType == BONUS_TYPE.RIGHT )
                    { 
                        BonusManagerClass.SetBonusRight( this.gameObject );  
                        BonusManagerClass.RightAreaChange( false );
                        BonusState = BONUS_STATE.TARGET;
                    }
             
                break;

                case BONUS_STATE.TARGET :

                    this.transform.position -= new Vector3( 0.0f , 0.0f , fMove );

                    if( 75.0f - this.transform.position.z > 3.0f && BonusType == BONUS_TYPE.LEFT )
                    { 
                        BonusManagerClass.LeftAreaChange( true );
                        BonusState = BONUS_STATE.OUT;
                    }
                    if( 75.0f - this.transform.position.z > 3.0f && BonusType == BONUS_TYPE.CENTER )
                    { 
                        BonusManagerClass.CenterAreaChange( true );
                        BonusState = BONUS_STATE.OUT;
                    }
                    if( 75.0f - this.transform.position.z > 3.0f && BonusType == BONUS_TYPE.RIGHT )
                    { 
                        BonusManagerClass.RightAreaChange( true );
                        BonusState = BONUS_STATE.OUT;
                    }

                break;

                case BONUS_STATE.BIRIBIRI:

                    if( bFlg == false )
                    {
                        bFlg = true;
                        this.gameObject.GetComponent< Animator >( ).applyRootMotion = true;
                        this.gameObject.GetComponent< PlayerAnim >( ).MotionChange( PlayerAnimDefine.Idx.VictoryIdle );
                    }

                    fCntFrame += Time.deltaTime;

                    if( fCntFrame >= fFalseFrame )
                    {
                        BonusState = BONUS_STATE.OUT;

                        if( BonusType == BONUS_TYPE.LEFT )
                        {
                            BonusManagerClass.LeftAreaChange( true );
                        }
                        else if( BonusType == BONUS_TYPE.CENTER )
                        {
                            BonusManagerClass.CenterAreaChange( true );
                        }
                        else
                        {
                            BonusManagerClass.RightAreaChange( true );
                        }

                        this.gameObject.SetActive( false );
                    }
                break;

                case BONUS_STATE.OUT:
                    this.transform.position -= new Vector3( 0.0f , 0.0f , fMove );
                break;
            }
        }
	}


    public void SetState( BONUS_TYPE Bonus )
    {
        BonusType = Bonus;
    }


    public void SetBiriBiri( )
    {
        BonusState = BONUS_STATE.BIRIBIRI;
    } 
}
