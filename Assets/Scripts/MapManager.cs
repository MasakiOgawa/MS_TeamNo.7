using System.Collections;
using UnityEngine;


public class MapManager : MonoBehaviour
{
    //道用の変数
    public GameObject    RoadPrefab;      //道のプレハブ
           GameObject[ ] RoadPrefabTmp;   //生成した道路のオブジェクト保存用配列
    public int           nNumRoad;        //道路の生成数
           int           nRoadIndexNo;    //現在の道路配列の添え字
    public float         fRoadLength;     //道路の奥行
           float         fRoadPosZ;       //道路のZ座標

    //建物用の変数
    public GameObject    City1Prefab;      //建物1のプレハブ
    public GameObject    City2Prefab;      //建物2のプレハブ
    public GameObject    City3Prefab;      //建物3のプレハブ
    public GameObject    City4Prefab;      //建物4のプレハブ
           GameObject[ ] CityPrefabTmp;    //生成した建物のオブジェクト保存用配列
    int                  nCntCity;         //生成した建物の数
    public int           nNumCity;         //建物の生成数
           int           nCityIndexNo;     //現在の建物配列の添え字
    public float         fCityLength;      //建物の奥行
           float         fCityPosZ;        //建物のZ座標
 

	void Start( )
    {
        //道用変数の初期化
        nRoadIndexNo  = 0;
        fRoadPosZ     = 42.0f;

		RoadPrefabTmp = new GameObject[ nNumRoad ];

        for( int nCnt = 0; nCnt < nNumRoad; nCnt++ )
        {
            RoadPrefabTmp[ nCnt ] = Instantiate( RoadPrefab , new Vector3( 0.0f , 0.0f , fRoadPosZ ) , Quaternion.identity );
            RoadPrefabTmp[ nCnt ].transform.rotation = Quaternion.Euler( 270.0f , 0.0f , 0.0f );
            fRoadPosZ += fRoadLength;
        }

        //建物用変数の初期化
        nCntCity      = 3;
        nCityIndexNo  = 0;
        fCityPosZ     = 0.0f;

		CityPrefabTmp = new GameObject[ nNumCity ];

        CityPrefabTmp[ 0 ] = Instantiate( City1Prefab , new Vector3( 0.0f , 0.0f , fCityPosZ ) , Quaternion.identity );
        CityPrefabTmp[ 1 ] = Instantiate( City2Prefab , new Vector3( 0.0f , 0.0f , fCityPosZ ) , Quaternion.identity );
        CityPrefabTmp[ 2 ] = Instantiate( City3Prefab , new Vector3( 0.0f , 0.0f , fCityPosZ ) , Quaternion.identity );
	}
	

    //道路の生成
    public void RoadCreate( )
    {
        //道路を一枚破棄
        Destroy( RoadPrefabTmp[ nRoadIndexNo ].gameObject );

        //新たに道路を生成
        RoadPrefabTmp[ nRoadIndexNo ] = Instantiate( RoadPrefab , new Vector3( 0.0f , 0.0f , fRoadPosZ ) , Quaternion.identity );
        RoadPrefabTmp[ nRoadIndexNo ].transform.rotation = Quaternion.Euler( 270.0f , 0.0f , 0.0f );
        fRoadPosZ += fRoadLength;

        //配列の添え字を進める
        nRoadIndexNo++;

        if( nRoadIndexNo == nNumRoad )
        {
            nRoadIndexNo = 0;
        }
    }


    //建物の生成
    public void CityCreate( )
    {
        //建物を一枚破棄
        Destroy( CityPrefabTmp[ nCityIndexNo ].gameObject );

        //新たに建物を生成
        switch ( nCntCity )
        {
            case 0 :
                CityPrefabTmp[ nCityIndexNo ] = Instantiate( City1Prefab , new Vector3( 0.0f , 0.0f , fCityPosZ ) , Quaternion.identity );
            break;

            case 1 :
                CityPrefabTmp[ nCityIndexNo ] = Instantiate( City2Prefab , new Vector3( 0.0f , 0.0f , fCityPosZ ) , Quaternion.identity );
            break;

            case 2 :
                CityPrefabTmp[ nCityIndexNo ] = Instantiate( City3Prefab , new Vector3( 0.0f , 0.0f , fCityPosZ ) , Quaternion.identity );
            break;

            case 3 :
                CityPrefabTmp[ nCityIndexNo ] = Instantiate( City4Prefab , new Vector3( 0.0f , 0.0f , fCityPosZ ) , Quaternion.identity );
            break;
        }

        nCntCity++;

        //建物を4種類生成したら奥行を進める
        if( nCntCity == 4 )
        {
            nCntCity = 0;

            fCityPosZ += fCityLength;
        }
        

        //配列の添え字を進める
        nCityIndexNo++;

        if( nCityIndexNo == nNumCity )
        {
            nCityIndexNo = 0;
        }
    }
}
