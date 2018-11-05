using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShakeUI : MonoBehaviour {

    private enum SHAKE_UI_TYPE
    {
        SHAKE_UI_DOWN,
        SHAKE_UI_UP,
        SHAKE_UI_RIGHT,
        SHAKE_UI_LEFT,
    };

    [SerializeField] private SHAKE_UI_TYPE shakeUiType;


    [SerializeField] RectTransform rectTran;     // アニメーションしたい情報

    // 初期値
    public float startY;
    public float startX;

    // 落下
    [SerializeField] private float TargetDelta;      // 移動量
    [SerializeField] private float firstSpeed;       // 初速度
    [SerializeField] private float moveAccel;        // 加速度
    private float nowSpeed;                           // 現在の速度

    // 上昇
    [SerializeField] private float ResetSpeed;          // 復帰速度
    public bool isAcceling;

    // Use this for initialization
    void Start()
    {
        // 上下移動のためY座標を保持
        if (shakeUiType == SHAKE_UI_TYPE.SHAKE_UI_DOWN ||
            shakeUiType == SHAKE_UI_TYPE.SHAKE_UI_UP)
        {
            // 初期Y格納
            startY = rectTran.transform.position.y;
        }
        else if ( shakeUiType == SHAKE_UI_TYPE.SHAKE_UI_RIGHT||
            shakeUiType == SHAKE_UI_TYPE.SHAKE_UI_LEFT)
        {
            // 初期X格納
            startX = rectTran.transform.position.x;
        }
        
        // 現在の速度=初速度
        nowSpeed = firstSpeed;
        // フラグ初期化
        isAcceling = true;
    }

    // Update is called once per frame
    void Update()
    {
        // ―――――――――――――――――――――――――――――――――――――――――――――――
        // ShakeDown
        // ―――――――――――――――――――――――――――――――――――――――――――――――
        if (shakeUiType == SHAKE_UI_TYPE.SHAKE_UI_DOWN ) {
            // 落下中
            if (isAcceling == true) {
                // 加速
                nowSpeed *= moveAccel;
                // 座標移動
                rectTran.transform.position = new Vector3(rectTran.transform.position.x,
                        rectTran.transform.position.y - nowSpeed,
                        rectTran.transform.position.z);

                // 目的地よりも下に行った場合
                if (TargetDelta + startY > rectTran.transform.position.y) {
                    // 位置強制指定
                    rectTran.transform.position = new Vector3(rectTran.transform.position.x,
                        TargetDelta + startY,
                        rectTran.transform.position.z);
                    // 上昇開始
                    isAcceling = false;
                    nowSpeed = firstSpeed;
                }
            }
            // 上昇中
            else {
                // 上昇
                rectTran.transform.position = new Vector3(rectTran.transform.position.x,
                rectTran.transform.position.y + ResetSpeed,
                rectTran.transform.position.z);

                // 上行き過ぎた場合
                if (startY < rectTran.transform.position.y) {
                    // 位置強制指定
                    rectTran.transform.position = new Vector3(rectTran.transform.position.x,
                        startY,
                        rectTran.transform.position.z);

                    // 下降開始
                    isAcceling = true;
                }
            }
        }
        // ―――――――――――――――――――――――――――――――――――――――――――――――
        // ShakeUp
        // ―――――――――――――――――――――――――――――――――――――――――――――――
        if (shakeUiType == SHAKE_UI_TYPE.SHAKE_UI_UP)
        {
            // 上昇中
            if (isAcceling == true)
            {
                // 加速
                nowSpeed *= moveAccel;

                // 上昇
                rectTran.transform.position = new Vector3(rectTran.transform.position.x,
                        rectTran.transform.position.y + nowSpeed,
                        rectTran.transform.position.z);

                // 目的地よりも上に行った場合
                if (TargetDelta + startY < rectTran.transform.position.y)
                {
                    // 位置強制指定
                    rectTran.transform.position = new Vector3(rectTran.transform.position.x,
                        TargetDelta + startY,
                        rectTran.transform.position.z);

                    // 下降開始
                    isAcceling = false;

                    nowSpeed = firstSpeed;
                }
            }
            // 下降
            else
            {
                // 下降
                rectTran.transform.position = new Vector3(rectTran.transform.position.x,
                rectTran.transform.position.y - ResetSpeed,
                rectTran.transform.position.z);

                // 下行き過ぎた場合
                if (startY > rectTran.transform.position.y)
                {
                    // 位置強制指定
                    rectTran.transform.position = new Vector3(rectTran.transform.position.x,
                        startY,
                        rectTran.transform.position.z);

                    // 下降開始
                    isAcceling = true;
                }

            }
        }
        // ―――――――――――――――――――――――――――――――――――――――――――――――
        // ShakeRight
        // ―――――――――――――――――――――――――――――――――――――――――――――――
        if (shakeUiType == SHAKE_UI_TYPE.SHAKE_UI_RIGHT)
        {
            // 右方向加速中
            if (isAcceling == true)
            {
                // 加速
                nowSpeed *= moveAccel;

                // 加速
                rectTran.transform.position = new Vector3(rectTran.transform.position.x + nowSpeed,
                        rectTran.transform.position.y,
                        rectTran.transform.position.z);

                // 目的地よりも右に行った場合
                if (TargetDelta + startX < rectTran.transform.position.x)
                {
                    // 位置強制指定
                    rectTran.transform.position = new Vector3(TargetDelta + startX,
                        rectTran.transform.position.y,
                        rectTran.transform.position.z);

                    // 戻り開始
                    isAcceling = false;
                    nowSpeed = firstSpeed;
                }
            }
            // 復帰
            else
            {
                // 復帰
                rectTran.transform.position = new Vector3(rectTran.transform.position.x -ResetSpeed,
                rectTran.transform.position.y,
                rectTran.transform.position.z);

                // 右行き過ぎた場合
                if (startX > rectTran.transform.position.x)
                {
                    // 位置強制指定
                    rectTran.transform.position = new Vector3(startX,
                        rectTran.transform.position.y,
                        rectTran.transform.position.z);

                    // 下降開始
                    isAcceling = true;
                }

            }
        }
        // ―――――――――――――――――――――――――――――――――――――――――――――――
        // ShakeLeft
        // ―――――――――――――――――――――――――――――――――――――――――――――――
        if (shakeUiType == SHAKE_UI_TYPE.SHAKE_UI_LEFT)
        {
            // 左方向加速中
            if (isAcceling == true)
            {
                // 加速
                nowSpeed *= moveAccel;

                // 加速
                rectTran.transform.position = new Vector3(rectTran.transform.position.x - nowSpeed,
                        rectTran.transform.position.y,
                        rectTran.transform.position.z);

                // 目的地よりも左に行った場合
                if (TargetDelta + startX > rectTran.transform.position.x)
                {
                    // 位置強制指定
                    rectTran.transform.position = new Vector3(TargetDelta + startX,
                        rectTran.transform.position.y,
                        rectTran.transform.position.z);

                    // 戻り開始
                    isAcceling = false;
                    nowSpeed = firstSpeed;
                }
            }
            // 復帰
            else
            {
                // 復帰
                rectTran.transform.position = new Vector3(rectTran.transform.position.x + ResetSpeed,
                rectTran.transform.position.y,
                rectTran.transform.position.z);

                // 左行き過ぎた場合
                if (startX < rectTran.transform.position.x)
                {
                    // 位置強制指定
                    rectTran.transform.position = new Vector3(startX,
                        rectTran.transform.position.y,
                        rectTran.transform.position.z);

                    // 下降開始
                    isAcceling = true;
                }

            }
        }
    }
}
