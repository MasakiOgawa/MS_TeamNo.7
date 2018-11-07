using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeController : MonoBehaviour {

    // 移動速度
    [SerializeField] private float moveSpeed;

    [SerializeField] private float SelfKillSize;

    private GameObject m_TargetObj;
    private float m_TargetOffsetY;

    private Vector3 SourcePos;
    private Vector3 TargetPos;

    private Vector3 MoveVector;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // 現在のposにMoveVecを足す
        this.transform.position = new Vector3(transform.position.x + MoveVector.x,
            transform.position.y + MoveVector.y,
            transform.position.z + MoveVector.z);

        // 到達したら自殺
        if ( TargetPos.x - SelfKillSize <= transform.position.x &&
            transform.position.x <= TargetPos.x + SelfKillSize &&
            TargetPos.z - SelfKillSize <= transform.position.z &&
            transform.position.z <= TargetPos.z + SelfKillSize )
        {

            // 爆発
            HitController.Create(m_TargetObj, m_TargetOffsetY , -1);

            // 当たったので消滅
            Destroy(this.gameObject);
        }


	}

    public void StartFire ( GameObject Source , float SourceY , GameObject Target , float TargetY )
    {

        m_TargetObj = Target;
        m_TargetOffsetY = TargetY;

        GameObject SourceObj;
        GameObject TargetObj;
        float OffsetSourceY;
        float OffsetTargetY;

        SourceObj = Source;
        TargetObj = Target;
        OffsetSourceY = SourceY;
        OffsetTargetY = TargetY;

        // 発射位置の設定
        TargetPos = new Vector3(Target.transform.position.x,
            Target.transform.position.y + OffsetTargetY,
            Target.transform.position.z);

        // 目的座標の設定
        SourcePos = new Vector3(Source.transform.position.x,
            Source.transform.position.y + OffsetSourceY,
            Source.transform.position.z);

        // ベクトル算出
        MoveVector = TargetPos - SourcePos;
        // 正規化
        MoveVector = MoveVector.normalized;

        // 移動ベクトルに指定した移動量をかける
        MoveVector *= moveSpeed;
    }

    static public void Create(GameObject Source, float SourceY, GameObject Target, float TargetY)
    {
        // プレハブを取得
        GameObject prefab = (GameObject)Resources.Load("Prefabs/Explode");
        // プレハブからインスタンスを生成
        GameObject obj = Instantiate(prefab);
        //  
        obj.GetComponent<ExplodeController>().StartFire(Source, SourceY, Target, TargetY);
    }
}
