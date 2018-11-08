using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShot : MonoBehaviour {

    public enum ONESHOT_TYPE
    {
        BUBBLE0,
        BUBBLE1,
        LIGHTS0,
        LIGHTS1,
        HEARTS0,
        HEARTS1,
        CLOVER0,
        CLOVER1,
        STAR0,
        STAR1,
        STAR2,
    };

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    static public void Create ( ONESHOT_TYPE type , Vector3  pos )
    {
        GameObject prefab;
        // typeで生成切り替え
        switch ( type )
        {
            case ONESHOT_TYPE.BUBBLE0:
                {
                    prefab = (GameObject)Resources.Load("Particle/OneShot/Bubbles_04");
                    break;
                }
            case ONESHOT_TYPE.BUBBLE1:
                {
                    prefab = (GameObject)Resources.Load("Particle/OneShot/Bubbles_05");
                    break;
                }
            case ONESHOT_TYPE.CLOVER0:
                {
                    prefab = (GameObject)Resources.Load("Particle/OneShot/FourLeafClover_01");
                    break;
                }
            case ONESHOT_TYPE.CLOVER1:
                {
                    prefab = (GameObject)Resources.Load("Particle/OneShot/FourLeafClover_02");
                    break;
                }
            case ONESHOT_TYPE.HEARTS0:
                {
                    prefab = (GameObject)Resources.Load("Particle/OneShot/Hearts_01");
                    break;
                }
            case ONESHOT_TYPE.HEARTS1:
                {
                    prefab = (GameObject)Resources.Load("Particle/OneShot/Hearts_02");
                    break;
                }
            case ONESHOT_TYPE.LIGHTS0:
                {
                    prefab = (GameObject)Resources.Load("Particle/OneShot/Lights_Burst_02");
                    break;
                }
            case ONESHOT_TYPE.LIGHTS1:
                {
                    prefab = (GameObject)Resources.Load("Particle/OneShot/Lights_Burst_03");
                    break;
                }
            case ONESHOT_TYPE.STAR0:
                {
                    prefab = (GameObject)Resources.Load("Particle/OneShot/Star_Burst_01");
                    break;
                }
            case ONESHOT_TYPE.STAR1:
                {
                    prefab = (GameObject)Resources.Load("Particle/OneShot/Star_Burst_02");
                    break;
                }
            case ONESHOT_TYPE.STAR2:
                {
                    prefab = (GameObject)Resources.Load("Particle/OneShot/Star_Burst_03");
                    break;
                }
            default:
                {
                    prefab = (GameObject)Resources.Load("Prefabs/Particle/OneShot/Star_Burst_03");
                    break;
                }
        }

        // プレハブからインスタンスを生成
        GameObject obj = Instantiate(prefab);
        // 座標設定
        obj.transform.position = new Vector3(pos.x, pos.y, pos.z);
    }
    
}
