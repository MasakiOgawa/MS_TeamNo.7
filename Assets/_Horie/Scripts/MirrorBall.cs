using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBall : MonoBehaviour {

    private GameObject MirrorBallAura;

	// Use this for initialization
	void Start () {
		
	}

    private void SetPos ( Vector3 pos )

    {

        transform.position = pos;
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 1, 0), -5);
    }

    // ミラーボールオーラ起動
    public void EnableAura ()
    {
        // プレハブを取得
        GameObject prefab = (GameObject)Resources.Load("Prefabs/MirrorBallAura");
        // プレハブからインスタンスを生成
        GameObject obj = Instantiate(prefab );
        obj.GetComponent<MirrorBall>().SetPos(this.transform.position);

        MirrorBallAura = obj;
    }

    // ミラーボールオーラ停止
    public void DisableAura()
    {
        Destroy(MirrorBallAura);

    }
        
}
