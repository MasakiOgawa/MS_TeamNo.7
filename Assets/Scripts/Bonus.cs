﻿using System.Collections;
using UnityEngine;


public class Bonus : MonoBehaviour
{
           Manager    ManagerClass;     //マネージャのクラス
    public GameObject ManagerObj;       //マネージャのオブジェクト
    public CountDown  CountDownClass;   //カウントダウンのクラス
           bool       bFlg;


	void Start( )
    {  
        //各クラスの情報を取得
        ManagerClass   = ManagerObj.GetComponent< Manager >( );
        CountDownClass = ManagerClass.GetCountDown( ).GetComponent< CountDown >( );
	}


    //ボーナスタイム
    public void BonusTime( )
    {
         //33拍でボーナスの終了
        if(  ManagerClass.GetfCntFrame( ) > 30.45451049f )
        {
            ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_CHECK );
            ManagerClass.ResetfCntFrame( );///???
        }
    }
}
