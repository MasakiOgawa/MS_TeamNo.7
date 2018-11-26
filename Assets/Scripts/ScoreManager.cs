﻿using System.Collections;
using UnityEngine;
using Uniduino;


public class ScoreManager : MonoBehaviour
{
    //列挙型定義
    public enum EVALUATION
    { 
       EVALUATION_BAD = 0   ,   //悪い
       EVALUATION_FINE      ,   //惜しい
       EVALUATION_EXCELLENT ,   //良い
       EVALUATION_MISS          //論外
    };

    //各評価のカウンタ
    int nCntBad;   //共通
    int nCntLeftFine;
    int nCntCenterFine;
    int nCntRightFine;
    int nCntLeftExcellent;
    int nCntCenterExcellent;
    int nCntRightExcellent;


           Manager            ManagerClass;              //マネージャのクラス
    public GameObject         ManagerObj;                //マネージャのオブジェクト
           EnemyManager       EnemyManagerClass;         //エネミーマネージャのクラス
           PerformanceManager PerformanceManagerClass;   //パフォーマンスマネージャのクラス
    public int                nScore;                    //スコア
     public int                nLeftScore;                    //スコア
     public int                nCenterScore;                    //スコア
     public int                nRightScore;                    //スコア

    public SerialHandler SerialHandlerClass;
    public Arduino ArdiunoClass;
    bool bFlg;
    float fCntFrame;

    public GameObject CanvasObj;

    int nBeatExcellent;
    int nBeatBad;

    public AudioClip   AudioExcellent;
    public AudioClip   AudioFine;
    public AudioClip   AudioBad;
    public AudioClip   AudioMiss;
           AudioSource AudioSource;

	void Start( )
    {
        //各クラスの情報を取得
        ManagerClass            = ManagerObj.GetComponent< Manager >( );
        EnemyManagerClass       = ManagerClass.GetEnemyManager( ).GetComponent< EnemyManager >( );
        PerformanceManagerClass = ManagerClass.GetPerformanceManager( ).GetComponent< PerformanceManager >( );

        //変数の初期化
		nCntBad       = 0;
        nCntLeftFine = 0;
        nCntCenterFine = 0;
        nCntRightFine = 0;
        nCntLeftExcellent = 0;
        nCntCenterExcellent = 0;
        nCntRightExcellent = 0;
        nScore        = 0;
        nLeftScore = 0;
        nCenterScore = 0;
        nRightScore = 0;

        ArdiunoClass = Arduino.global;
        ArdiunoClass.Setup( ConfigurePins );
        bFlg = false;
        fCntFrame = 0.0f;    
        nBeatExcellent = 0;
        nBeatBad = 0;
        
    /*    FontController.Create( CanvasObj , FontController.FONT_TYPE.FONT_EXCELLENT , new Vector2( -240.0f , -180.0f ) );
        FontController.Create( CanvasObj , FontController.FONT_TYPE.FONT_EXCELLENT , new Vector2(  0.0f , -180.0f ) );
        FontController.Create( CanvasObj , FontController.FONT_TYPE.FONT_EXCELLENT , new Vector2( 240.0f , -180.0f ) );*/

        AudioSource      = gameObject.GetComponent< AudioSource >( );
	}


    void ConfigurePins( )
    {
        ArdiunoClass.pinMode( 6 , PinMode.OUTPUT );
    }


    void Update( )
    {
        if ( bFlg == true )
        {
            fCntFrame += Time.deltaTime;

            if( fCntFrame >= 3.0f )
            {
                bFlg = false;
                fCntFrame = 0.0f;
                SerialHandlerClass.Write( "3" );
            }
        }
    }
	

    //評価のアクティブ化
    public void ActiveTrue( Vector2 Pos , EVALUATION Evaluation , int nPlayerNo )
    {
        switch( Evaluation )
        {
            case EVALUATION.EVALUATION_BAD :   
                FontController.Create( CanvasObj , FontController.FONT_TYPE.FONT_BAD , Pos );
              
                nCntBad++;
                nBeatBad++;

                AudioSource.clip = AudioBad;
                AudioSource.PlayOneShot( AudioBad );

                if( nBeatBad >= 3 )
                {
                    if( EnemyManagerClass.GetBeatTarget( ) != null )
                    {
                        EnemyManagerClass.GetBeatTarget( ).GetComponent< PlayerAnim >( ).MotionChange( PlayerAnimDefine.Idx.Angry );
                    }
                }

            break;

            case EVALUATION.EVALUATION_FINE :
                FontController.Create( CanvasObj , FontController.FONT_TYPE.FONT_FINE ,  Pos );
               
               if( nPlayerNo == 0 )
                {
                    nCntLeftFine++;
                }
               else if ( nPlayerNo == 1 )
                {
                    nCntCenterFine++;
                }
                else
                {
                     nCntRightFine++;
                }

                AudioSource.clip = AudioFine;
                AudioSource.PlayOneShot( AudioFine );

                if( nBeatBad == 0 && nBeatExcellent == 0 && EnemyManagerClass.GetBeatTarget( ) != null )
                {
                    EnemyManagerClass.GetBeatTarget( ).GetComponent< Animator >( ).applyRootMotion = true;
                    EnemyManagerClass.GetBeatTarget( ).GetComponent< PlayerAnim >( ).MotionChange( PlayerAnimDefine.Idx.VictoryIdle );
                }

            break;

            case EVALUATION.EVALUATION_EXCELLENT :
                FontController.Create( CanvasObj , FontController.FONT_TYPE.FONT_EXCELLENT , Pos );
                
                if( nPlayerNo == 0 )
                {
                    nCntLeftExcellent++;
                }
               else if ( nPlayerNo == 1 )
                {
                    nCntCenterExcellent++;
                }
                else
                {
                     nCntRightExcellent++;
                }

                nBeatExcellent++;

                AudioSource.clip = AudioExcellent;
                AudioSource.PlayOneShot( AudioExcellent );

                if( EnemyManagerClass.GetBeatTarget( ) != null )
                {
                    EnemyManagerClass.GetBeatTarget( ).GetComponent< Animator >( ).applyRootMotion = true;
                    EnemyManagerClass.GetBeatTarget( ).GetComponent< PlayerAnim >( ).MotionChange( PlayerAnimDefine.Idx.VictoryIdle );
                }

                if( nBeatExcellent >= 3 && bFlg == false )
                {
                    bFlg = true;
                    SerialHandlerClass.Write( "4" );
                }

            break;

            case EVALUATION.EVALUATION_MISS :
                FontController.Create( CanvasObj , FontController.FONT_TYPE.FONT_MISS , Pos );

                AudioSource.clip = AudioMiss;
                AudioSource.PlayOneShot( AudioMiss );
            break;
        }

   

       // Debug.Log( nCntHyo);
    }


    //スコアの集計
    public void AggregateScore( )
    {
        nLeftScore += nCntLeftExcellent * 2 + nCntLeftFine;
        nCenterScore += nCntCenterExcellent * 2 + nCntCenterFine;
        nRightScore += nCntRightExcellent * 2 + nCntRightFine;
        nScore = nLeftScore + nCenterScore + nRightScore;

        //敵を追従させる
        EnemyManagerClass.TakeIn( ( nCntLeftExcellent * 2 + nCntLeftFine ) + ( nCntCenterExcellent * 2 + nCntCenterFine ) + ( nCntRightExcellent * 2 + nCntRightFine ) );

        //各評価のリセット
        nCntBad       = 0;
        nCntLeftExcellent = 0;
        nCntCenterExcellent = 0;
        nCntRightExcellent = 0;
        nCntLeftFine = 0;
        nCntCenterFine = 0;
        nCntRightFine = 0;
       
        //現在の敵を非アクティブ化
        EnemyManagerClass.ActiveFalse( );

        //遷移先をチェック
        ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_CHECK );
    }


    //
    public int GetnScore( )
    {
        return nScore;
    }


    public int GetnLeftScore( )
    {
        return nLeftScore;
    }


    public int GetnCenterScore( )
    {
        return nCenterScore;
    }


    public int GetnRightScore( )
    {
        return nRightScore;
    }

    public void ResetnScore( )
    {
         nScore = 0;
         nLeftScore = 0;
        nCenterScore = 0;
        nRightScore = 0;
    }


    public void ResetnBeatEvaluation( )
    {
        nBeatBad = 0;
        nBeatExcellent = 0;
    }


    public void ExcellentCount( int nPlayerNo )
    {
        if( nPlayerNo == 0 )
        {
            nCntLeftExcellent++;
        }
        else if ( nPlayerNo == 1 )
        {
            nCntCenterExcellent++;
        }
        else
        {
                nCntRightExcellent++;
        }
    }
}
