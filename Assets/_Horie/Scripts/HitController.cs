using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour {

    [SerializeField] private int KillFrame;

    private int nFrame;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if ( KillFrame < nFrame )
        {
            Destroy(this.gameObject);
        }

        nFrame++;

	}

    private void SetPos ( GameObject Target , float OffsetY , float OffsetZ )
    {
        this.transform.position = new Vector3(Target.transform.position.x,
            Target.transform.position.y + OffsetY,
            Target.transform.position.z + OffsetZ);

        nFrame = 0;
    }
       

    static public void Create( GameObject Target , float OffsetY , float OffsetZ )
    {
        // プレハブを取得
        GameObject prefab = (GameObject)Resources.Load("Prefabs/Hit");
        // プレハブからインスタンスを生成
        GameObject obj = Instantiate(prefab);
        obj.GetComponent<HitController>().SetPos(Target , OffsetY , OffsetZ);
    }
}
