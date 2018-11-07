using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour {

    [SerializeField] private GameObject _ResultManager;
    [SerializeField] private GameObject Woman;
    [SerializeField] private GameObject ThunderTaerget;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            EffectManager.CreateSphereEffect(new Vector3(1, 2, 3), new Vector3(10, 20, 100), 100);
        }


        if (Input.GetKeyDown(KeyCode.W))
        {
            WomanController womanScript = Woman.GetComponent<WomanController>();
            womanScript.SetThunder(ThunderTaerget);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // ビーム発射
            ExplodeController.Create(Woman, 1, ThunderTaerget, 2);
            
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 爆破
            HitController.Create(ThunderTaerget , 1 , -1);

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
