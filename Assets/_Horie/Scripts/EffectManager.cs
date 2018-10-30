using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 開始座標、目標座標、所要フレームを送ってパーティクル生成
    static public void CreateSphereEffect(Vector3 StartPos, Vector3 TargetPos, int Frame)
    {
        // プレハブを取得
        GameObject prefab = (GameObject)Resources.Load("Prefabs/SphereEffect");
        // プレハブからインスタンスを生成
        GameObject obj = Instantiate(prefab);
        //  
        obj.GetComponent<SphereEffect>().Run(StartPos, TargetPos, Frame);
    }
}
