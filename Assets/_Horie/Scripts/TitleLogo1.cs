using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleLogo1 : MonoBehaviour {

    [SerializeField]
    RectTransform rectTran;     // アニメーションしたい情報

    Tweener tweener;            // トゥイーンの情報

    [SerializeField] private float animTime;        // アニメーション時間
    [SerializeField] private float Scale;           // サイズ
                                // Use this for initialization
    void Start () {
        // 3秒かけて2倍の大きさに
        // スケールシェイク
        tweener = rectTran.DOPunchScale(
        new Vector3(Scale, Scale),    // scale1.5倍指定
        animTime,                        // アニメーション時間
        0
        ).SetLoops(-1);

    }

    // Update is called once per frame
    void Update () {

    }

    private void OnDestroy()
    {
        // オブジェクトが消えてもアニメーションが止まらないので キルでアニメーションを消す
        tweener.Kill();
    }
}
