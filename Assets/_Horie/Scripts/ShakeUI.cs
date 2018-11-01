using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShakeUI : MonoBehaviour {

    [SerializeField] RectTransform rectTran;     // アニメーションしたい情報

    // 初期Y
    private float startY;

    // 落下
    [SerializeField] private float TargetDeltaY;      // 移動量
    [SerializeField] private float firstSpeedY;       // 初速度
    [SerializeField] private float moveAccelY;        // 加速度
    private float nowSpeed;                           // 現在の速度

    // 上昇
    [SerializeField] private float UpSpeedY;          // 上昇速度

    public bool isDowning;

    // Use this for initialization
    void Start()
    {
        // 初期Y格納
        startY = rectTran.transform.position.y;

        // 現在の速度=初速度
        nowSpeed = firstSpeedY;

        //
        isDowning = true;

    }

    // Update is called once per frame
    void Update()
    {
        // 落下中
        if ( isDowning == true)
        {
            // 加速
            nowSpeed *= moveAccelY;

            rectTran.transform.position = new Vector3(rectTran.transform.position.x,
                    rectTran.transform.position.y - nowSpeed,
                    rectTran.transform.position.z);

            // 目的地よりも下に行った場合
            if ( TargetDeltaY + startY > rectTran.transform.position.y)
            {
                // 位置強制指定
                rectTran.transform.position = new Vector3(rectTran.transform.position.x,
                    TargetDeltaY + startY,
                    rectTran.transform.position.z);

                // 上昇開始
                isDowning = false;

                nowSpeed = firstSpeedY;
            }
        }
        // 上昇中
        else
        {
            // 上昇
            rectTran.transform.position = new Vector3(rectTran.transform.position.x,
            rectTran.transform.position.y + UpSpeedY,
            rectTran.transform.position.z);

            // 上行き過ぎた場合
            if (startY < rectTran.transform.position.y)
            {
                // 位置強制指定
                rectTran.transform.position = new Vector3(rectTran.transform.position.x,
                    startY,
                    rectTran.transform.position.z);

                // 下降開始
                isDowning = true;
            }

        }
    }

    private void OnDestroy()
    {
        // オブジェクトが消えてもアニメーションが止まらないので キルでアニメーションを消す
        //tweener.Kill();
    }
}
