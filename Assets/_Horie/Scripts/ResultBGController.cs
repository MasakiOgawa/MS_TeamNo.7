using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultBGController : MonoBehaviour {

    // 開始数秒で横スケール大きくそのあと縦スケール大きく
    [SerializeField] private GameObject ResultManagerOBJ;
    [SerializeField] private float oneFrameDeltaSizeX;
    [SerializeField] private float oneFrameDeltaSizeY;


    private int ScalePhase; //0:ストップ 1:横 2:縦

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if ( ScalePhase == 1 )
        {
            // 拡大
            transform.localScale = new Vector3(transform.localScale.x + oneFrameDeltaSizeX, transform.localScale.y, transform.localScale.z);
            
            // あふれ
            if ( transform.localScale.x >= 1 )
            {
                transform.localScale = new Vector3(1.0f, transform.localScale.y, transform.localScale.z);
                ScalePhase = 2;
            }
        }
        else if ( ScalePhase == 2 )
        {
            // 拡大
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + oneFrameDeltaSizeY, transform.localScale.z);

            // あふれ
            if (transform.localScale.y >= 1)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, transform.localScale.z);
                ScalePhase = 0;

                // アニメーション終わり
                ResultManagerOBJ.GetComponent<ResultManager>().EndBGStartResult();
            }
        }

	}

    public void StartBG ()
    {

        // 初期設定を行う
        ScalePhase = 0;

        transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        ScalePhase = 1;
    }
}
