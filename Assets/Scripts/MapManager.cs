using System.Collections;
using UnityEngine;


public class MapManager : MonoBehaviour
{
    public GameObject    RoadPrefab;      //道のプレハブ
           GameObject[ ] RoadPrefabTmp;   //生成した道路のオブジェクト保存用配列
    public int           nNumRoad;        //道路の生成数
           int           nIndexNo;        //現在の道路配列の添え字
    public float         fRoadLength;     //道路の長さ
           float         fRoadPosZ;       //道路のZ座標


	void Start( )
    {
        //変数の初期化
        nIndexNo  = 0;
        fRoadPosZ = 42.0f;

		RoadPrefabTmp = new GameObject[ nNumRoad ];

        for( int nCnt = 0; nCnt < nNumRoad; nCnt++ )
        {
            RoadPrefabTmp[ nCnt ] = Instantiate( RoadPrefab , new Vector3( 0.0f , 0.0f , fRoadPosZ ) , Quaternion.identity );
            RoadPrefabTmp[ nCnt ].transform.rotation = Quaternion.Euler( 270.0f , 0.0f , 0.0f );
            fRoadPosZ += fRoadLength;
        }
	}
	

    //道路の生成
    void Create( )
    {
        //道路を一枚破棄
        Destroy( RoadPrefabTmp[ nIndexNo ].gameObject );

        //新たに道路を生成
        RoadPrefabTmp[ nIndexNo ] = Instantiate( RoadPrefab , new Vector3( 0.0f , 0.0f , fRoadPosZ ) , Quaternion.identity );
        RoadPrefabTmp[ nIndexNo ].transform.rotation = Quaternion.Euler( 270.0f , 0.0f , 0.0f );
        fRoadPosZ += fRoadLength;

        //配列の添え字を進める
        nIndexNo++;

        if( nIndexNo == nNumRoad )
        {
            nNumRoad = 0;
        }
    }
}
