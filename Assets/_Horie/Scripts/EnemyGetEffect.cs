using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGetEffect : MonoBehaviour {

    // 演出生成するプレハブを登録
    [SerializeField] private GameObject[] Enemy_Prefabs;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float distMove;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetEnemyEffect ( int nType , Vector3 pos)
    {
        // プレハブを取得
        GameObject prefab = Enemy_Prefabs[nType];
        // プレハブからインスタンスを生成
        GameObject obj = Instantiate(prefab);

        // エネミー生成初期設定関数 向きとか変えたい

        // モーション再生
        obj.GetComponent<PlayerAnim>().MotionChange(PlayerAnimDefine.Idx.HappyWalk);
        
        // 移動処理用スクリプトをアタッチ
        obj.AddComponent<EnemyEffect>();
        obj.GetComponent<EnemyEffect>().SetSpeedAndDist(moveSpeed, distMove);
    }
}
