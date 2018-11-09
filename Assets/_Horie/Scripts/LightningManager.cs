using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class LightningManager : MonoBehaviour {

    [SerializeField] private GameObject Lightning0;
    [SerializeField] private GameObject Lightning1;
    [SerializeField] private GameObject Lightning2;

    [SerializeField] private float DispFrame;
    [SerializeField] private float TargetOffsetZ;


    private GameObject sourceOBJ;
    private float sourceY;
    private GameObject m_targetOBJ;
    private float targetY;
    private int nFrame;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if ( nFrame == 1 )
        {
            // プレハブを取得
            GameObject prefab = (GameObject)Resources.Load("Prefabs/Explode_cold");
            // プレハブからインスタンスを生成
            GameObject obj = Instantiate(prefab,m_targetOBJ.transform);
        }
        if (nFrame > DispFrame)
            Destroy(this.gameObject);

        nFrame++;

	}

    // 子プレハブに情報を送る
    private void Set ( GameObject sourceOBJ , float sourceY , GameObject targetOBJ , float targetY)
    {
        m_targetOBJ = targetOBJ;

        // ミラーボール
        Lightning0.GetComponent<LightningBoltScript>().StartPosition = new Vector3(sourceOBJ.transform.position.x,
            sourceOBJ.transform.position.y + sourceY,
            sourceOBJ.transform.position.z);
        // 敵
        Lightning0.GetComponent<LightningBoltScript>().EndPosition = new Vector3(targetOBJ.transform.position.x,
            targetOBJ.transform.position.y + targetY,
            targetOBJ.transform.position.z);

        // ミラーボール
        Lightning1.GetComponent<LightningBoltScript>().StartPosition = new Vector3(sourceOBJ.transform.position.x,
            sourceOBJ.transform.position.y + sourceY,
            sourceOBJ.transform.position.z);
        // 敵
        Lightning1.GetComponent<LightningBoltScript>().EndPosition = new Vector3(targetOBJ.transform.position.x,
            targetOBJ.transform.position.y + targetY,
            targetOBJ.transform.position.z);

        // ミラーボール
        Lightning2.GetComponent<LightningBoltScript>().StartPosition = new Vector3(sourceOBJ.transform.position.x,
            sourceOBJ.transform.position.y + sourceY,
            sourceOBJ.transform.position.z);
        // 敵
        Lightning2.GetComponent<LightningBoltScript>().EndPosition = new Vector3(targetOBJ.transform.position.x,
            targetOBJ.transform.position.y + targetY,
            targetOBJ.transform.position.z);
       
        nFrame = 0;
    }

    // 生成
    static public void Create ( GameObject sourceOBJ , float sourceY , GameObject targetOBJ , float targetY )
    {
        // プレハブを取得
        GameObject prefab = (GameObject)Resources.Load("Prefabs/LightningPack");
        // プレハブからインスタンスを生成
        GameObject obj = Instantiate(prefab);
        //セット  
        obj.GetComponent<LightningManager>().Set(sourceOBJ, sourceY, targetOBJ, targetY);
    }
}
