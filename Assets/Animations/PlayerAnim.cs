using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator anim;  // アニメーター情報
    [SerializeField]private PlayerAnimDefine.Idx AnimIdx;       // アニメーションインデックス
    private PlayerAnimDefine.Idx AnimIdxOld;       // アニメーションインデックス
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // アニメーションチェック用
        if (AnimIdx != AnimIdxOld)
        {
            MotionChange(AnimIdx);
            AnimIdxOld = AnimIdx;
        }
    }

    // プレイヤーアニメーション変更
    public void MotionChange(PlayerAnimDefine.Idx idx)
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("AnimIdx", (int)idx);
        AnimIdxOld = AnimIdx;
    }
}