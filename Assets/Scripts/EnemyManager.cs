using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static TextAsset     EnemyText;          //敵情報が格納されたファイルの情報
    public  GameObject          EnemyUpPrefab;      //上方向の敵のプレハブ
    public  GameObject          EnemyDownPrefab;    //下方向の敵のプレハブ
    public  GameObject          EnemyRightPrefab;   //右方向の敵のプレハブ
    public  GameObject          EnemyLeftPrefab;    //左方向の敵のプレハブ
    public static GameObject[ ] EnemyPrefabTmp;     //生成した敵情報を保存
    public Vector2              LeftEndPos;         //左端の敵の座標
    public float                fDist;              //敵同士の距離
    public static int           nCreateNo;          //ファイルの文字列の添え字


	void Start( )
    {
        //変数の初期化
        EnemyPrefabTmp = new GameObject[ 8 ];
        nCreateNo = 0;

        for( int nCnt = 0; nCnt < 8; nCnt++ )
        {
            EnemyPrefabTmp[ nCnt ] = null;
        }

        //敵情報をファイルから読み込み
        EnemyText = Resources.Load("enemy") as TextAsset;
	}


    //敵の生成
    public void Create( )
    {  
        //各方向の敵を生成
        for( int nCnt = nCreateNo , nCnt2 = 0; nCnt < nCreateNo + 8; nCnt++ , nCnt2++ )
        {
            if( EnemyText.text[ nCnt ] == '1' )
            {
                EnemyPrefabTmp[ nCnt2 ] = Instantiate( EnemyUpPrefab , new Vector3( LeftEndPos.x - ( fDist * nCnt2 ), LeftEndPos.y , 0.0f ) , Quaternion.identity );
            }
            else if( EnemyText.text[ nCnt ] == '2' )
            {
                EnemyPrefabTmp[ nCnt2 ] = Instantiate( EnemyDownPrefab , new Vector3( LeftEndPos.x - ( fDist * nCnt2 ) , LeftEndPos.y , 0.0f ) , Quaternion.identity );
            }
            else if( EnemyText.text[ nCnt ] == '3' )
            {
                EnemyPrefabTmp[ nCnt2 ] = Instantiate( EnemyLeftPrefab , new Vector3( LeftEndPos.x - ( fDist * nCnt2 ) , LeftEndPos.y , 0.0f ) , Quaternion.identity );
            }
            else if( EnemyText.text[ nCnt ] == '4' )
            {
                EnemyPrefabTmp[ nCnt2 ] = Instantiate( EnemyRightPrefab , new Vector3( LeftEndPos.x - ( fDist * nCnt2 ) , LeftEndPos.y , 0.0f ) , Quaternion.identity );
            }
        }

        //文字列の添え字を進める(敵8体+改行コード2文字分)
        nCreateNo += 10;

        //カウントダウンの開始
        Manager.SetPhase( Manager.GAME_PHASE.PHASE_COUNT_DOWN );
    }

    
    //敵の破棄
    public static void Kill( )
    {
        for( int nCnt = 0; nCnt < 8; nCnt++ )
        {
            if( EnemyPrefabTmp[ nCnt ] != null )
            {
                Destroy( EnemyPrefabTmp[ nCnt ].gameObject );
                EnemyPrefabTmp[ nCnt ] = null;
            }
        }

        //次の敵を生成
        Manager.SetPhase( Manager.GAME_PHASE.PHASE_ENEMY_APPEARANCE );
        CountDown.SetText( );
    }
}
