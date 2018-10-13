using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class shakeitController : MonoBehaviour
{

    [SerializeField]
    RectTransform rectTran;     // アニメーションしたい情報

    Tweener tweener;            // トゥイーンの情報

    // Use this for initialization
    void Start()
    {
        // スケールシェイク
        tweener = rectTran.DOShakeScale(1.0f, new Vector2(1.0f, 1.0f)).SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        // オブジェクトが消えてもアニメーションが止まらないので キルでアニメーションを消す
        tweener.Kill();
    }
}
