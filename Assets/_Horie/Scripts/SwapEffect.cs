using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapEffect : MonoBehaviour {

    public enum SWAP_TYPE
    {
        TYPE_UP,
        TYPE_DOWN,
        TYPE_RIGHT,
        TYPE_LEFT,
    };


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    static public void Create ( SWAP_TYPE type , Vector3 pos)
    {
        GameObject prefab;

        if ( type == SWAP_TYPE.TYPE_DOWN)
        {
            prefab = (GameObject)Resources.Load("Particle/WarpBlue");
        }
        else if ( type == SWAP_TYPE.TYPE_UP)
        {
            prefab = (GameObject)Resources.Load("Particle/WarpRed");
        }
        else if ( type == SWAP_TYPE.TYPE_LEFT)
        {
            prefab = (GameObject)Resources.Load("Particle/WarpGreen");
        }
        else if ( type == SWAP_TYPE.TYPE_RIGHT)
        {
            prefab = (GameObject)Resources.Load("Particle/WarpOrange");
        }
        else
        {
            prefab = (GameObject)Resources.Load("Particle/WarpOrange");
            Debug.Log("error");
        }

        
        // プレハブからインスタンスを生成
        GameObject obj = Instantiate(prefab);
        // 座標設定
        obj.transform.position = new Vector3(pos.x, pos.y, pos.z);


    }



}
