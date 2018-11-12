using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounusArea : MonoBehaviour
{
    // 自身のゲームオブジェクト
    static GameObject BounusAreaObj;


    [SerializeField] private GameObject Area_Green;
    [SerializeField] private GameObject Area_Green1;
    [SerializeField] private GameObject Area_Green2;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 
    static public void Create( Vector3 pos0 , Vector3 pos1 , Vector3 pos2)
    {
        GameObject prefab;
        prefab = (GameObject)Resources.Load("Prefabs/Bounus/BounusAreaManager");

        GameObject obj = Instantiate(prefab);

        // 生成したオブジェクトを保持
        BounusAreaObj = obj;

        BounusAreaObj.GetComponent<BounusArea>().Run(pos0, pos1, pos2);
    }

    // 終了
    static public void Delete()
    {

        Destroy(BounusAreaObj);
    }

    // 色変更
    static public void ChangeColor ( int nIdx , bool isGreen )
    {
        BounusArea Ba = BounusAreaObj.GetComponent<BounusArea>();

        if (nIdx == 0)
        {
            if ( isGreen == true )
            {
                //Ba.Area_Green.SetActive(true);
                Ba.Area_Green.GetComponent<BounusAreaController>().ChangeColor(true);
                //Ba.Area_Yellow.SetActive(false);
            }
            else
            {
                Ba.Area_Green.GetComponent<BounusAreaController>().ChangeColor(false);
                //Ba.Area_Green.SetActive(false);
                //Ba.Area_Yellow.SetActive(true);
            }
        }
        else if (nIdx == 1)
        {
            if (isGreen == true)
            {
                Ba.Area_Green1.GetComponent<BounusAreaController>().ChangeColor(true);
            }
            else
            {
                Ba.Area_Green1.GetComponent<BounusAreaController>().ChangeColor(false);
            }
        }
        else if ( nIdx == 2)
        {
            if (isGreen == true)
            {
                Ba.Area_Green2.GetComponent<BounusAreaController>().ChangeColor(true);
            }
            else
            {
                Ba.Area_Green2.GetComponent<BounusAreaController>().ChangeColor(false);
            }
        }
    }


    private void Run(Vector3 pos0, Vector3 pos1, Vector3 pos2)
    {
        // 緑のみ点灯
        Area_Green.SetActive(true);
        Area_Green1.SetActive(true);
        Area_Green2.SetActive(true);

        // 座標設定
        Area_Green.transform.position = pos0;
        Area_Green1.transform.position = pos1;
        Area_Green2.transform.position = pos2;

    }



}
