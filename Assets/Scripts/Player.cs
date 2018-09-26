using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static float fCntFrame;   //経過フレーム
 

	void Start( )
    {
		//変数の初期化
		fCntFrame = 0.0f;
	}
	

    public static void Dance( )
    {
        //フレーム数を計測
		fCntFrame += Time.deltaTime;

        //一定フレームが経過したらメトロノームを鳴らす
      //  if( fCntFrame > 60.0f / BGM.GetBPM( ) )
        
            //Sample.Emit( );
       // }

        //一定フレームが経過したらダンスの終了
        if( fCntFrame >= ( 60.0f / ( float )BGM.GetBPM( ) ) * 4.0f )
        {
            fCntFrame = 0.0f;

            //次の敵を生成
            Manager.SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_TAKE_IN );
        }
    }
}
