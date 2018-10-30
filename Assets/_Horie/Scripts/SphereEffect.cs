using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEffect : MonoBehaviour {

    public Vector3 m_startPos;
    public Vector3 m_target;
    public int m_frame;

    public Vector3 oneFrameMove;

    private int nFrameCounter;
	// Use this for initialization
	void Start ( ) {

    }
	
	// Update is called once per frame
	void Update () {

        // 移動量を足す
        
        this.transform.position = new Vector3 (
            this.transform.position.x + oneFrameMove.x ,
            this.transform.position.y + oneFrameMove.y ,
            this.transform.position.z + oneFrameMove.z);
  

        // フレームカウンタ
        nFrameCounter++;
        if ( m_frame < nFrameCounter)
            Destroy(this.gameObject);

    }

    // 発射
    public void Run (Vector3 startPos , Vector3 targetPos , int frame )
    {
        // 発射位置設定
        m_startPos = new Vector3( startPos.x , startPos.y , startPos.z);
        this.transform.position = new Vector3 ( startPos.x , startPos.y , startPos.z);

        // 目的座標取得
        m_target = new Vector3 (targetPos.x , targetPos.y , targetPos.z );

        // 1フレームの移動距離を算出
        oneFrameMove = (m_target - m_startPos) / (float)frame;

        m_frame = frame;
    }
}