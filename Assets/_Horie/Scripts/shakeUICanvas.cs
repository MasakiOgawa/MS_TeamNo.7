using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shakeUICanvas : MonoBehaviour {

    public enum SHAKE_TYPE
    {
        UP , 
        DOWN,
        RIGHT,
        LEFT,
    };
    [SerializeField] private float posY;
    [SerializeField] private float[] posX;
    [SerializeField] private GameObject thisObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateShakeUI ( SHAKE_TYPE type , int posIdx)
    {
        GameObject prefab;
        if ( type == SHAKE_TYPE.UP)
        {
            prefab = (GameObject)Resources.Load("Prefabs/ShakeUpUI");
        }
        else if ( type == SHAKE_TYPE.DOWN)
        {
            prefab = (GameObject)Resources.Load("Prefabs/ShakeDownUI");
        }
        else if ( type == SHAKE_TYPE.RIGHT)
        {
            prefab = (GameObject)Resources.Load("Prefabs/ShakeRightUI");
        }
        else if ( type == SHAKE_TYPE.LEFT)
        {
            prefab = (GameObject)Resources.Load("Prefabs/ShakeLeftUI");
        }
        else
        {
            prefab = (GameObject)Resources.Load("Prefabs/ShakeLeftUI");
        }
        // プレハブからインスタンスを生成
        GameObject obj = Instantiate(prefab ,thisObj.transform );

        // 座標設定
        //obj.transform.position = new Vector3(posX[posIdx], posY, 0);
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(posX[posIdx], posY, 0);

    }
}
