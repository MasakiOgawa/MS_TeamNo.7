﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class CountDown : MonoBehaviour
{
    public GameObject ManagerObj;    //マネージャのオブジェクト
    public GameObject CountNo3Obj;   //3のオブジェクト
    public GameObject CountNo2Obj;   //2のオブジェクト
    public GameObject CountNo1Obj;   //1のオブジェクト
    public GameObject CountGoObj;    //GOのオブジェクト
           Rhythm     RhythmObj;     //リズムのオブジェクト
           float      fBPM;          //BPM
           float      fCntFrame;     //経過フレーム
           int        nCnt;
           bool       bFlg;


	void Start( )
    {
        //変数の初期化
		fCntFrame = 0.0f;
        fBPM      = 60.0f / ManagerObj.GetComponent< Manager >( ).GetBGM( ).GetComponent< BGM >( ).GetBPM( );
        nCnt      = 3;
        bFlg      = false;

        //リズムのオブジェクトを取得
        RhythmObj = ManagerObj.GetComponent< Manager >( ).GetRhythm( ).GetComponent< Rhythm >( );

        //カウントダウンオブジェクトを非表示にしておく
       /* CountNo3Obj.SetActive( false );
        CountNo2Obj.SetActive( false );
        CountNo1Obj.SetActive( false );
        CountGoObj.SetActive( false );*/
	}
	

    //テキストの内容をカウントダウン
    public void ChangeText( )
    {
        //フレーム数を計測
		fCntFrame += Time.deltaTime;

        //1拍毎にカウントダウン
        if( fCntFrame >= fBPM )
        {
            bFlg      = true;
            fCntFrame = 0.0f;
          
            if( nCnt == 3 )
            {
                nCnt = 2;
                CountNo3Obj.SetActive( false );
                CountNo2Obj.SetActive( true );
                RhythmObj.Emit( );
            }
            else if( nCnt == 2 )
            {
                nCnt = 1;
                CountNo2Obj.SetActive( false );
                CountNo1Obj.SetActive( true );
                RhythmObj.Emit( );
            }
            else if( nCnt == 1 )
            {
                nCnt = 0;
                CountNo1Obj.SetActive( false );
                CountGoObj.SetActive( true );
                RhythmObj.Emit( );
            }

            //カウントダウンの終了
            else if( nCnt == 0 )
            {
                nCnt = 3;
                CountGoObj.SetActive( false );
         
                //プレイヤーのダンス
                ManagerObj.GetComponent< Manager >( ).SetPhase( Manager.GAME_PHASE.PHASE_PLAYER_DANCE );
            }
        }
    }


    //カウントダウンテキストを可視化
    public void SetText( )
    {
        if( ManagerObj.GetComponent< Manager >( ).GetPhase( ) != Manager.GAME_PHASE.PHASE_END_PERFORMANCE )
        {
            nCnt = 3;
            CountNo3Obj.SetActive( true );
            RhythmObj.Emit( );
        }
    }
}
