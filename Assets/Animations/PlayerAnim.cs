using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    // アニメーション ステート
    public enum PLAYER_ANIM
    {
        IDLE = 0,       // 待機
        WALK,           // 歩く
        GANGNAM_STYLE   // カンナムスタイル
    }

    private Animator anim;  // アニメーター情報

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // ↑キーを押して歩くモーションへ
        if (Input.GetKey(KeyCode.UpArrow))
        {
            MotionChange(PLAYER_ANIM.WALK);
        }

        // →キーを押してカンナムスタイルへ
        if (Input.GetKey(KeyCode.RightArrow))
        {
            MotionChange(PLAYER_ANIM.GANGNAM_STYLE);
        }

        // ←キーを押して待機モーションへ
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MotionChange(PLAYER_ANIM.IDLE);
        }
    }

    // プレイヤーアニメーション変更
    public void MotionChange(PLAYER_ANIM playerAnim)
    {
        switch (playerAnim)
        {
            case PLAYER_ANIM.IDLE:              // 待機
                anim.SetBool("Walk", false);
                anim.SetBool("GangnamStyle", false);
                break;

            case PLAYER_ANIM.WALK:              // 歩く
                anim.SetBool("Walk", true);
                AnimFlagReset(PLAYER_ANIM.WALK);
                break;

            case PLAYER_ANIM.GANGNAM_STYLE:     // カンナムスタイル
                anim.SetBool("GangnamStyle", true);
                AnimFlagReset(PLAYER_ANIM.GANGNAM_STYLE);
                break;
        }
    }

    // 実行中のアニメーション以外falseに
    private void AnimFlagReset(PLAYER_ANIM playerAnim)
    {
        // 一回全てのフラグをオフに
        anim.SetBool("Walk", false);
        anim.SetBool("GangnamStyle", false);

        switch (playerAnim)
        {
            case PLAYER_ANIM.IDLE:              // 待機
                break;

            case PLAYER_ANIM.WALK:              // 歩く
                anim.SetBool("Walk", true);
                break;

            case PLAYER_ANIM.GANGNAM_STYLE:     // カンナムスタイル
                anim.SetBool("GangnamStyle", true);
                break;
        }
    }
}