﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour {

    [SerializeField] private GameObject _ResultManager;
    [SerializeField] private GameObject Woman;
    [SerializeField] private GameObject ThunderTaerget;
    [SerializeField] private GameObject MirrorBall;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
           // EffectManager.CreateSphereEffect(new Vector3(1, 2, 3), new Vector3(10, 20, 100), 100);
        }


        if (Input.GetKeyDown(KeyCode.W))
        {
          //  WomanController womanScript = Woman.GetComponent<WomanController>();
          //  womanScript.SetThunder(ThunderTaerget);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            // ビーム発射
            ExplodeController.Create(Woman, 0, ThunderTaerget, 2 , MirrorBall , 2 ,
                ExplodeController.EXPLODE_TYPE.TYPE_FINE , 0.5f , 1.25f);
            
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            // ビーム発射
            ExplodeController.Create(Woman, 0, ThunderTaerget, 2, MirrorBall, 2,
                ExplodeController.EXPLODE_TYPE.TYPE_BAD, 0.5f, 1.25f);

        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            // ビーム発射
            ExplodeController.Create(Woman, 0, ThunderTaerget, 2, MirrorBall, 2,
                ExplodeController.EXPLODE_TYPE.TYPE_EXCELLENT, 0.5f, 1.25f);

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            // 電撃
            LightningManager.Create(MirrorBall, 0, ThunderTaerget, 0);

        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            // mirrorBallAura
            MirrorBall.GetComponent<MirrorBall>().EnableAura();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            // mirrorBallAura
            MirrorBall.GetComponent<MirrorBall>().DisableAura();
        }

        // bounusArea
        if (Input.GetKeyDown(KeyCode.A))
        {
            // 起動
            BounusArea.Create(new Vector3(0, 0, 0), new Vector3(-2, 0, 0), new Vector3(2, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            // 削除
            BounusArea.Delete();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            // 0:黄色
            BounusArea.ChangeColor(0, false);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            // 0:緑
            BounusArea.ChangeColor(0, true);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            // 1:黄色
            BounusArea.ChangeColor(1, false);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            // 1:緑
            BounusArea.ChangeColor(1, true);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            // 2:黄色
            BounusArea.ChangeColor(2, false);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            // 2:緑
            BounusArea.ChangeColor(2, true);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            BounusEffect.Create(ThunderTaerget);
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            // 爆破
            HitController.Create(ThunderTaerget , 1 , -1);

        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            // ランダムでいろいろ
            int Rand = Random.RandomRange(0, 10);

            // 
            OneShot.Create( (OneShot.ONESHOT_TYPE) Rand, new Vector3 ( ThunderTaerget.transform.position.x ,
                ThunderTaerget.transform.position.y + 2.0f ,
                ThunderTaerget.transform.position.z));

        }


        // エンターを押してシーン遷移
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _ResultManager.GetComponent<ResultManager>().StartResult(183);
        }

        // セーブデータ削除
        if (Input.GetKeyDown(KeyCode.F5))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
