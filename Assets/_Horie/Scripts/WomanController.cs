using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class WomanController : MonoBehaviour {

    [SerializeField] private GameObject MirrorBallObj;
    [SerializeField] private GameObject ThunderPrefab0;
    [SerializeField] private GameObject ThunderPrefab1;
    [SerializeField] private int ThunderEffectFrame;


    // 雷
    public int nFrameCounter;
    private int ThunderPhase;
    private GameObject targetObj;

	// Use this for initialization
	void Start () {
        InitThunder();

    }
	
	// Update is called once per frame
	void Update () {
        // 雷エフェクト更新
        UpdateThunder();
	}


    //――――――――――――――――――――――――――――――――――――――――――――――
    //  雷エフェクト関係
    //――――――――――――――――――――――――――――――――――――――――――――――
    // 雷生成
    public void SetThunder (GameObject TargetObj )
    {
        // プレハブを有効化
        ThunderPrefab0.SetActive(true);
        ThunderPrefab1.SetActive(true);

        // ライトニングオブジェクトを取得
        LightningBoltScript thunderScript0 = ThunderPrefab0.GetComponent<LightningBoltScript>();
        // スタートエンドを設定
        thunderScript0.StartObject = this.gameObject;
        thunderScript0.EndObject = MirrorBallObj;


        // ライトニングオブジェクトを取得
        LightningBoltScript thunderScript1 = ThunderPrefab1.GetComponent<LightningBoltScript>();
        // スタートエンドを設定
        thunderScript1.StartObject = MirrorBallObj;
        thunderScript1.EndObject = TargetObj;

        targetObj = TargetObj;

        // フレームカウンタを初期化
        nFrameCounter = 0;
        // フェーズを設定
        ThunderPhase = 1;

        CreateColdEffect();
    }

    // 雷初期化
    private void InitThunder()
    {
        ThunderPrefab0.SetActive(false);
        ThunderPrefab1.SetActive(false);
        nFrameCounter = 0;
        ThunderPhase = 0;       // 0:無効　1:プレイヤーからミラーボール　2:ミラーボールからエネミー
    }
    
    // 雷更新
    private void UpdateThunder ()
    {
        // 現在のフェーズから0ならreturn
        if (ThunderPhase == 0)
            return;

        // 一定時間超えたら
        if ( nFrameCounter > ThunderEffectFrame)
        {
            // 現在のフェーズが1なら
            if ( ThunderPhase == 1)
            {
                // 次のフェーズに切り替える
                ThunderPhase = 0;

                nFrameCounter = 0;

                InitThunder();



                return;
            }
        }

        nFrameCounter++;

    }

    //――――――――――――――――――――――――――――――――――――――――――――――
    //  氷エフェクト関係
    //――――――――――――――――――――――――――――――――――――――――――――――
    private void CreateColdEffect()
    {
        // プレハブを取得
        GameObject prefab = (GameObject)Resources.Load("Prefabs/HitEffect/Hit_01_Blue");
        Instantiate(prefab , targetObj.transform );
        prefab = (GameObject)Resources.Load("Prefabs/HitEffect/Hit_02_Red");
        Instantiate(prefab, targetObj.transform);
        prefab = (GameObject)Resources.Load("Prefabs/HitEffect/Hit_03_Yellow");
        Instantiate(prefab, targetObj.transform);
        prefab = (GameObject)Resources.Load("Prefabs/HitEffect/Hit_04_Green");
        Instantiate(prefab, targetObj.transform);
        prefab = (GameObject)Resources.Load("Prefabs/HitEffect/Hit_05_Purple");
        Instantiate(prefab, targetObj.transform);

    }

}
