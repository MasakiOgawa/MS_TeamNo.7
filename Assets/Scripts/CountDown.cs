using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class CountDown : MonoBehaviour
{
           Manager    ManagerClass;   //マネージャのクラス
    public GameObject ManagerObj;     //マネージャのオブジェクト
           BGM        BGMClass;       //BGMのクラス
           Rhythm     RhythmClass;    //リズムのクラス
           int        nCountDown;     //カウントダウンのカウンタ

    public float OneBeat;


    public GameObject MirrorBallColorObj;
    MirrorBallMaterial MirrorBallMaterialClass;

    public GameObject AuraLeftObj;
    AuraLeft AuraLeftClass;

    public GameObject ThreeObj;
    public GameObject TwoObj;
    public GameObject OneObj;
    public GameObject GoObj;


    void Start( )
    {
        //変数の初期化
        nCountDown = 3;

        //各クラスの情報を取得
        ManagerClass = ManagerObj.GetComponent< Manager >( );
        BGMClass     = ManagerClass.GetBGM( ).GetComponent< BGM >( );
        RhythmClass  = ManagerClass.GetRhythm( ).GetComponent< Rhythm >( );

        MirrorBallMaterialClass = MirrorBallColorObj.GetComponent< MirrorBallMaterial >( );

        AuraLeftClass = AuraLeftObj.GetComponent<AuraLeft>();
    }
	

    //カウントダウン
    public void ChangeCount( )
    {
        //1拍毎にカウントダウン
        if( ManagerClass.GetdCntFrame( ) >= OneBeat )
        {
            ManagerClass.SetFlg( );
            
            if( nCountDown == 3 )
            {
                ThreeObj.SetActive(false);
                TwoObj.SetActive(true);
                nCountDown = 2;
                MirrorBallMaterialClass.SetColor( nCountDown );
                RhythmClass.Emit( );
            }
            else if( nCountDown == 2 )
            {
                TwoObj.SetActive(false);
                OneObj.SetActive(true);
                nCountDown = 1;
                MirrorBallMaterialClass.SetColor( nCountDown );
                RhythmClass.Emit( );
            }
            else if( nCountDown == 1 )
            {
                OneObj.SetActive(false);
                GoObj.SetActive(true);
                nCountDown = 0;
                MirrorBallMaterialClass.SetColor( nCountDown );
                RhythmClass.Emit( );
            }
            //カウントダウンの終了
            else if( nCountDown == 0 )
            {
                GoObj.SetActive(false);
                nCountDown = 0;
                MirrorBallMaterialClass.SetColor( nCountDown );
                RhythmClass.Emit( );
         
                //プレイヤーのダンス
                ManagerClass.SetPhase( Manager.GAME_PHASE.PHASE_PLAYER_DANCE );

                AuraLeftClass.ResetPoint();            }
        }
    }


    //カウントダウンを可視化
    public void ActiveCountDown( )
    {
        if( ManagerClass.GetPhase( ) != Manager.GAME_PHASE.PHASE_END_PERFORMANCE )
        {
            ThreeObj.SetActive(true);
            nCountDown = 3;
            MirrorBallMaterialClass.SetColor( nCountDown );
            RhythmClass.Emit( );
        }
    }
}
