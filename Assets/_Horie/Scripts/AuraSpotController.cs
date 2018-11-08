using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraSpotController : MonoBehaviour {

    public enum AURA_TYPE
    {
        TYPE_0,
        TYPE_1,
        TYPE_2,
        TYPE_3,
        TYPE_4,
    };
    [SerializeField] private AURA_TYPE auraType;


    [SerializeField] private float RotateSpeedY;
    [SerializeField] private GameObject[] AuraPointLight;


    [SerializeField] private float TYPE_2_AccelSpeed;
    private float moveSpeed;
    private bool TYPE_2_isUp;

    [SerializeField] private float TYPE_3_RotateSpeedX;

	// Use this for initialization
	void Start ()
    { 
        // アタッチされていない場合
        for ( int n =0; n < AuraPointLight.Length; n ++)
        {
            if (AuraPointLight[n] == null)
                Debug.Log("AuraPointLight not attach : " + n);

        }

        // すべてのAuraを消灯する
        for ( int n = 0; n < AuraPointLight.Length; n ++)
        {
            AuraPointLight[n].SetActive(false);
        }

        //auraType = AURA_TYPE.TYPE_0;

        moveSpeed = 1;
        

        switch (auraType)
        {
            //―――――――――――――――――――――――――――――――――――――――
            // 全消灯
            //―――――――――――――――――――――――――――――――――――――――
            case AURA_TYPE.TYPE_0:
                {
                    // すべてのAuraを消灯する
                    for (int n = 0; n < AuraPointLight.Length; n++)
                    {
                        AuraPointLight[n].SetActive(false);
                    }
                    break;
                }
            //―――――――――――――――――――――――――――――――――――――――
            // 1つ
            //―――――――――――――――――――――――――――――――――――――――
            case AURA_TYPE.TYPE_1:
                {
                    AuraPointLight[0].SetActive(true);
                    AuraPointLight[0].GetComponent<AuraAPI.AuraLight>().enabled = true;


                    break;
                }
            //―――――――――――――――――――――――――――――――――――――――
            // 2
            //―――――――――――――――――――――――――――――――――――――――
            case AURA_TYPE.TYPE_2:
                {
                    AuraPointLight[0].SetActive(true);
                    AuraPointLight[0].GetComponent<AuraAPI.AuraLight>().enabled = true;
                    TYPE_2_isUp = false;
                    AuraPointLight[1].SetActive(true);
                    AuraPointLight[1].GetComponent<AuraAPI.AuraLight>().enabled = true;
                    moveSpeed = 1;
                    break;
                }
            //―――――――――――――――――――――――――――――――――――――――
            // 3
            //―――――――――――――――――――――――――――――――――――――――
            case AURA_TYPE.TYPE_3:
                {
                    AuraPointLight[0].SetActive(true);
                    AuraPointLight[0].GetComponent<AuraAPI.AuraLight>().enabled = true;
                    AuraPointLight[1].SetActive(true);
                    AuraPointLight[1].GetComponent<AuraAPI.AuraLight>().enabled = true;

                    AuraPointLight[2].SetActive(true);
                    transform.position = new Vector3(7.0f,
                        transform.position.y,
                        transform.position.z);
                    AuraPointLight[2].GetComponent<AuraAPI.AuraLight>().enabled = true;
                    break;
                }
            //―――――――――――――――――――――――――――――――――――――――
            // 4
            //―――――――――――――――――――――――――――――――――――――――
            case AURA_TYPE.TYPE_4:
                {
                    AuraPointLight[0].SetActive(true);
                    AuraPointLight[0].GetComponent<AuraAPI.AuraLight>().enabled = true;
                    AuraPointLight[1].SetActive(true);
                    AuraPointLight[1].GetComponent<AuraAPI.AuraLight>().enabled = true;
                    AuraPointLight[2].SetActive(true);
                    AuraPointLight[2].GetComponent<AuraAPI.AuraLight>().enabled = true;

                    AuraPointLight[3].SetActive(true);
                    AuraPointLight[3].GetComponent<AuraAPI.AuraLight>().enabled = true;
                    break;
                }
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.C))
        {
            IncreaseType(AURA_TYPE.TYPE_0);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            IncreaseType(AURA_TYPE.TYPE_1);

        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            IncreaseType(AURA_TYPE.TYPE_2);

        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            IncreaseType(AURA_TYPE.TYPE_3);

        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            IncreaseType(AURA_TYPE.TYPE_4);

        }

        // 現在の点灯パターンに応じて処理変更
        if ( auraType == AURA_TYPE.TYPE_2 )
        {
            // 加速しつつ上下移動 -14 ~ 24
            moveSpeed *= TYPE_2_AccelSpeed;

            Debug.Log("1");

            // 上昇中
            if ( TYPE_2_isUp == true )
            {
                transform.position = new Vector3(transform.position.x,
                    transform.position.y + moveSpeed,
                    transform.position.z);
                Debug.Log("2");
                // 上昇上限超えた場合
                if ( transform.position.y >= 24 )
                {
                    transform.position = new Vector3(transform.position.x,
                    24.0f,
                    transform.position.z);
                    moveSpeed = 1;
                    TYPE_2_isUp = false;
                    Debug.Log("3");
                }
            }
            // 下降中
            else
            {
                transform.position = new Vector3(transform.position.x,
                transform.position.y - moveSpeed,
                transform.position.z);
                Debug.Log("4");
                // 下降上限を超えた場合
                if ( transform.position.y <= -14)
                {
                    transform.position = new Vector3(transform.position.x,
                    -14.0f,
                    transform.position.z);
                    moveSpeed = 1;
                    TYPE_2_isUp = true;
                    Debug.Log("5");
                }
            }
            
            
        }
        else if ( auraType == AURA_TYPE.TYPE_3)
        {
            transform.Rotate(new Vector3(TYPE_3_RotateSpeedX, 0, 0));

        }


        //if ( auraType != AURA_TYPE.TYPE_3)
            transform.Rotate(new Vector3(0, RotateSpeedY, 0));

	}

    // type一つ進める
    public void IncreaseType ( AURA_TYPE Aura_Type)
    {
        // 位置初期化
        transform.position = new Vector3(0, 7, 20);
        transform.rotation = Quaternion.identity; 
        auraType = Aura_Type;

        // 各タイプに応じて初期設定を行う

        // すべてのAuraを消灯する
        for (int n = 0; n < AuraPointLight.Length; n++)
        {
            AuraPointLight[n].SetActive(false);

        }
        switch (auraType)
        {
            //―――――――――――――――――――――――――――――――――――――――
            // 全消灯
            //―――――――――――――――――――――――――――――――――――――――
            case AURA_TYPE.TYPE_0:
                {
                    // すべてのAuraを消灯する
                    for (int n = 0; n < AuraPointLight.Length; n++)
                    {
                        AuraPointLight[n].SetActive(false);
                    }
                    break;
                }
            //―――――――――――――――――――――――――――――――――――――――
            // 1つ
            //―――――――――――――――――――――――――――――――――――――――
            case AURA_TYPE.TYPE_1:
                {
                    AuraPointLight[0].SetActive(true);
                    AuraPointLight[0].GetComponent<AuraAPI.AuraLight>().enabled = true;


                    break;
                }
            //―――――――――――――――――――――――――――――――――――――――
            // 2
            //―――――――――――――――――――――――――――――――――――――――
            case AURA_TYPE.TYPE_2:
                {
                    AuraPointLight[0].SetActive(true);
                    AuraPointLight[0].GetComponent<AuraAPI.AuraLight>().enabled = true;
                    TYPE_2_isUp = false;
                    AuraPointLight[1].SetActive(true);
                    AuraPointLight[1].GetComponent<AuraAPI.AuraLight>().enabled = true;
                    moveSpeed = 1;
                    break;
                }
            //―――――――――――――――――――――――――――――――――――――――
            // 3
            //―――――――――――――――――――――――――――――――――――――――
            case AURA_TYPE.TYPE_3:
                {
                    AuraPointLight[0].SetActive(true);
                    AuraPointLight[0].GetComponent<AuraAPI.AuraLight>().enabled = true;
                    AuraPointLight[1].SetActive(true);
                    AuraPointLight[1].GetComponent<AuraAPI.AuraLight>().enabled = true;

                    AuraPointLight[2].SetActive(true);
                    transform.position = new Vector3(7.0f,
                        transform.position.y,
                        transform.position.z);
                    AuraPointLight[2].GetComponent<AuraAPI.AuraLight>().enabled = true;
                    break;
                }
            //―――――――――――――――――――――――――――――――――――――――
            // 4
            //―――――――――――――――――――――――――――――――――――――――
            case AURA_TYPE.TYPE_4:
                {
                    AuraPointLight[0].SetActive(true);
                    AuraPointLight[0].GetComponent<AuraAPI.AuraLight>().enabled = true;
                    AuraPointLight[1].SetActive(true);
                    AuraPointLight[1].GetComponent<AuraAPI.AuraLight>().enabled = true;
                    AuraPointLight[2].SetActive(true);
                    AuraPointLight[2].GetComponent<AuraAPI.AuraLight>().enabled = true;

                    AuraPointLight[3].SetActive(true);
                    AuraPointLight[3].GetComponent<AuraAPI.AuraLight>().enabled = true;
                    break;
                }
        }



    }
}
