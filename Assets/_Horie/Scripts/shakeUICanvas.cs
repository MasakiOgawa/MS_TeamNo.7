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

    private GameObject[] a_shakeUI;

	// Use this for initialization
	void Start () {
        a_shakeUI = new GameObject[8];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateShakeUI ( SHAKE_TYPE type , int posIdx)
    {
        GameObject prefab;
        if ( type == SHAKE_TYPE.UP)
        {
            prefab = (GameObject)Resources.Load("Prefabs/GameShakeUpUI");
        }
        else if ( type == SHAKE_TYPE.DOWN)
        {
            prefab = (GameObject)Resources.Load("Prefabs/GameShakeDownUI");
        }
        else if ( type == SHAKE_TYPE.RIGHT)
        {
            prefab = (GameObject)Resources.Load("Prefabs/GameShakeRightUI");
        }
        else if ( type == SHAKE_TYPE.LEFT)
        {
            prefab = (GameObject)Resources.Load("Prefabs/GameShakeLeftUI");
        }
        else
        {
            prefab = (GameObject)Resources.Load("Prefabs/GameShakeLeftUI");
        }
        // プレハブからインスタンスを生成
        GameObject obj = Instantiate(prefab ,thisObj.transform );

        a_shakeUI[posIdx] = obj;

        // 座標設定
        //obj.transform.position = new Vector3(posX[posIdx], posY, 0);
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(posX[posIdx], posY, 100);

    }

    public void ResetShakeUI ()
    {
        for (int n = 0; n < 8; n++)
        {
            Destroy(a_shakeUI[n]);
        }

        a_shakeUI = new GameObject[8];



    }
}
