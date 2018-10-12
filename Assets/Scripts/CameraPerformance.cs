using System.Collections;
using UnityEngine;


public class CameraPerformance : MonoBehaviour
{
    public GameObject ManagerObj;          //マネージャのオブジェクト
           MapManager MapManagerObj;       //マップマネージャのオブジェクト
    public float      fMove;               //カメラの移動量
           float      fRoadDist;           //カメラの移動距離(道用)
    public float      fRoadCreateBorder;   //道路の生成時の境界
    public float      fCityCreateBorder;   //建物の生成時の境界
           float      fCityDist;           //カメラの移動距離(建物用)


    void Start( )
    {
        //変数の初期化
        fRoadDist = 0.0f;
        fCityDist = 0.0f;

        //マップマネージャオブジェクトを取得
        MapManagerObj = ManagerObj.GetComponent< Manager >( ).GetMapManager( ).GetComponent< MapManager >( );
    }


    //カメラの移動
    public void PerformanceMove( )
    {
        this.transform.position += new Vector3( 0.0f , 0.0f , fMove );
        fRoadDist += fMove;
        fCityDist += fMove;

        //一定距離を移動したら道路の生成
        if( fRoadDist > fRoadCreateBorder )
        {
            fRoadDist = 0.0f;

            MapManagerObj.RoadCreate( );
        }

         //一定距離を移動したら建物の生成
        if( fCityDist > fCityCreateBorder )
        {
            fCityDist = 0.0f;

            MapManagerObj.CityCreate( );
        }
    }
}
