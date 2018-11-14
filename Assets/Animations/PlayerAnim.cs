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

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    MotionChange(PlayerAnimDefine.Idx.Up);
        //}

        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    MotionChange(PlayerAnimDefine.Idx.Down);
        //}

        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    MotionChange(PlayerAnimDefine.Idx.Left);
        //}

        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    MotionChange(PlayerAnimDefine.Idx.Right);
        //}

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    MotionChange(PlayerAnimDefine.Idx.Idle);
        //}
    }

    // プレイヤーアニメーション変更
    public void MotionChange(PlayerAnimDefine.Idx idx)
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("AnimIdx", (int)idx);
        AnimIdxOld = AnimIdx;
    }
}