using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounusEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Set ( Vector3 pos )
    {
        transform.position = pos;
    }



    // 生成
    static public void Create ( GameObject TargetObject )
    {
        // プレハブを取得
        GameObject prefab = (GameObject)Resources.Load("Prefabs/Bounus/SpawnYellow");
        // プレハブからインスタンスを生成
        GameObject obj = Instantiate(prefab );
        obj.GetComponent<BounusEffect>().Set(TargetObject.transform.position);

        // プレハブを取得
        prefab = (GameObject)Resources.Load("Prefabs/Bounus/WarpYellow");
        // プレハブからインスタンスを生成
        obj = Instantiate(prefab, TargetObject.transform);
        obj.GetComponent<BounusEffect>().Set(TargetObject.transform.position);

    }

}
