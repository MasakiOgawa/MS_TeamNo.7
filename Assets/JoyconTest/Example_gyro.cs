using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Example_gyro : MonoBehaviour
{
    // 振り判定マクロ
    private float SHAKE_FRONT_START_VALUE_Y = -14.0f;
    private float SHAKE_LEFT_START_VALUE_Z = -13.0f;
    private float SHAKE_RIGHT_START_VALUE_Z = 13.0f;
    private float SHAKE_UP_START_VALUE_Y = 14.0f;

    // 振り判定列挙
    public enum SHAKE_STATE{
        SHAKE_NONE,                 // なんでもない
        SHAKE_DOWN_READY,          // 前振り予備
        SHAKE_DOWN_TRIGGER,        // 前振り実行
        SHAKE_LEFT_READY,           // 左予備
        SHAKE_LEFT_TRIGGER,         // 左実行
        SHAKE_RIGHT_READY,          // 右予備
        SHAKE_RIGHT_TRIGGER,        // 右実行
        SHAKE_UP_READY,             // 上予備
        SHAKE_UP_TRIGGER,           // 上実行
    };

    public enum JOYCON_TYPE
    {
        JOYCON_L1,
        JOYCON_R1,
        JOYCON_R2,
    };

    public enum JOYCON_STATE
    {
        STATE_NONE,
        STATE_DOWN_TRIGGER,
        STATE_LEFT_TRIGGER,
        STATE_RIGHT_TRIGGER,
        STATE_UP_TRIGGER,
    };


    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL1;
    private Joycon m_joyconR1;
    private Joycon m_joyconR2;
    private Joycon.Button? m_pressedButtonL1;
    private Joycon.Button? m_pressedButtonR1;
    private Joycon.Button? m_pressedButtonR2;
    private bool m_IsEnableJoyconL1;
    private bool m_IsEnableJoyconR1;
    private bool m_IsEnableJoyconR2;
    
    // ShakeState 0:L1 , 1:R1 , 2:R2
    private SHAKE_STATE[] m_stateShakeJoycon;

    // 一つ前のフレームのジャイロ値
    private Vector3[] m_prevGyro;

    //以下が追記のコード
    //public GameObject rCube1 , rCube2, lCube;
    private Quaternion rciq1, rciq2, lciq, riq1, riq2, liq;
    //private Animator anim;
    //private Transform RS_bone, LS_bone;

    private void Start()
    {
        // メンバ初期化
        m_IsEnableJoyconL1 = false;
        m_IsEnableJoyconR1 = false;
        m_IsEnableJoyconR2 = false;

        m_stateShakeJoycon = new SHAKE_STATE[3];
        m_prevGyro = new Vector3[3];

        // ジョイコン情報を取得
        m_joycons = JoyconManager.Instance.j;

        // ジョイコンが存在しない。数が0以下の場合終了
        if (m_joycons == null || m_joycons.Count <= 0) return;

        // Left値を取得、値に応じて格納
        //m_joyconL1 = m_joycons.Find(c => c.isLeft);
        //m_joyconR1 = m_joycons.Find(c => !c.isLeft);
        
        // ３本に満たない場合
     //   if (m_joycons.Count < 3) //Debug.Log("<<Warning : Not Enough JoyCon>> Connected JoyConNum :" + m_joycons.Count);


        int nCntRight = 0;

        for ( int nCnt = 0 ; nCnt < m_joycons.Count; nCnt ++)
        {
            // 現在のジョイコンのインデックス番号
            //Debug.Log("---Now JoyCon No." + nCnt + "--------------------------------------------------------");

            // 今見てるコントローラーが右なのか左なのか
            if (m_joycons[nCnt].isLeft)
            {
                //Debug.Log("LeftController");

                // Leftコントローラーを格納
                m_joyconL1 = m_joycons[nCnt];

                m_IsEnableJoyconL1 = true;
            }
            else
            {
                //Debug.Log("RightController");

                // 右1本目
                if (nCntRight == 0)
                {
                    // Rightコントローラーを格納
                    m_joyconR1 = m_joycons[nCnt];
                    m_IsEnableJoyconR1 = true;
                    nCntRight++;
                }
                else
                {
                    // Rightコントローラーを格納
                    m_joyconR2 = m_joycons[nCnt];
                    m_IsEnableJoyconR2 = true;
                }
            }

           // //Debug.Log( nCnt + "----------------------------------------------------------------------");
        }
        

        // 以下が追記のコード
        //anim = (Animator)FindObjectOfType(typeof(Animator));

        //rCube1 = GameObject.Find("RightCube1");
        //rCube2 = GameObject.Find("RightCube2");
        //lCube = GameObject.Find("LeftCube1");

        //RS_bone = anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
        //LS_bone = anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);

        //rciq1 = rCube1.transform.rotation;
        //rciq2 = rCube2.transform.rotation;
        //lciq = lCube.transform.rotation;
        //riq = RS_bone.rotation;
        //liq = LS_bone.rotation;
    }

    private void Update()
    {
        m_pressedButtonL1 = null;
        m_pressedButtonR1 = null;
        m_pressedButtonR2 = null;

        foreach (var button in m_buttons)
        {
            // 各コントローラーのボタン情報を取得
            if (m_IsEnableJoyconL1 == true)
            {
                if (m_joyconL1.GetButton(button))
                {
                    m_pressedButtonL1 = button;
                }
            }
            if (m_IsEnableJoyconR1 == true)
            {
                if (m_joyconR1.GetButton(button))
                {
                    m_pressedButtonR1 = button;
                }
            }
            if (m_IsEnableJoyconR2 == true)
            {
                if (m_joyconR2.GetButton(button))
                {
                    m_pressedButtonR2 = button;
                }
            }
        }
        
        // ジョイコンが入っていない場合
        if (m_joycons == null || m_joycons.Count <= 0) return;

        // 各コントローラーのボタン情報を取得
        if (m_IsEnableJoyconL1 == true)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                m_joyconL1.SetRumble(160, 320, 0.6f, 20);
            }
        }
        if (m_IsEnableJoyconR1 == true)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                m_joyconR1.SetRumble(160, 320, 0.6f, 200);
            }
        }
        if (m_IsEnableJoyconR2 == true)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                m_joyconR2.SetRumble(160, 320, 0.6f, 200);
            }
        }

        //―――――――――――――――――――――――
        // SHAKESTATEの更新
        //―――――――――――――――――――――――
        for ( int nCnt = 0; nCnt < 3; nCnt ++)
        {
            if ( nCnt == 0 )
            {
                // L1が有効の場合
                if ( m_IsEnableJoyconL1 == true)
                {
                    // NONEのときREADYに入る
                    if ( m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_NONE)
                    {
                        // FRONT_STARTの条件を満たしている場合
                        float y = m_joyconL1.GetGyro().y;
                        if ( SHAKE_FRONT_START_VALUE_Y >= y )
                        {
                            // FRONT_READYに切り替え
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_DOWN_READY;
                            // 現フレームのジャイロを格納
                            m_prevGyro[nCnt] = m_joyconL1.GetGyro();
                        }

                        // LEFT_STARTの条件を満たしている場合
                        float z = m_joyconL1.GetGyro().z;
                        if ( SHAKE_LEFT_START_VALUE_Z >= z )
                        {
                            // LEFT_READYに切り替え
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_LEFT_READY;
                            // 現フレームのジャイロを格納
                            m_prevGyro[nCnt] = m_joyconL1.GetGyro();

                        }

                        // RIGHT_STARTの条件を満たしている場合
                        z = m_joyconL1.GetGyro().z;
                        if (SHAKE_RIGHT_START_VALUE_Z <= z)
                        {
                            // LEFT_READYに切り替え
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_RIGHT_READY;
                            // 現フレームのジャイロを格納
                            m_prevGyro[nCnt] = m_joyconL1.GetGyro();
                        }

                        // UP_STARTの条件を満たしている場合
                        y = m_joyconL1.GetGyro().y;
                        if (SHAKE_UP_START_VALUE_Y <= y)
                        {
                            // FRONT_READYに切り替え
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_UP_READY;
                            // 現フレームのジャイロを格納
                            m_prevGyro[nCnt] = m_joyconL1.GetGyro();
                        }
                    }
                    // FRONT_READYのとき前フレームのジャイロ値と比較して前よりも大きくなったら(減速)トリガー
                    else if ( m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_DOWN_READY)
                    {
                        // 過去フレームのジャイロより現フレームのジャイロが大きい場合
                        if ( m_prevGyro[nCnt].y < m_joyconL1.GetGyro().y)
                        {
                            // トリガーに切り替え
                            //Debug.Log("joyconL1 : <color=red>SHAKE_FRONT_TRIGGER</color>");
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_DOWN_TRIGGER;
                            m_joyconL1.SetRumble(160, 320, 0.6f, 1);
                        }
                    }

                    // LEFT_READYのとき前フレームのジャイロ値と比較して前よりも大きくなったら(減速)トリガー
                    else if ( m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_LEFT_READY)
                    {
                        // 過去フレームのジャイロより現フレームのジャイロが大きい場合
                        if ( m_prevGyro[nCnt].z < m_joyconL1.GetGyro().z)
                        {
                            // トリガーに切り替え
                            //Debug.Log("joyconL1 : <color=blue>SHAKE_LEFT_TRIGGER</color>");
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_LEFT_TRIGGER;
                            m_joyconL1.SetRumble(100, 100, 0.6f, 1);
                        }
                    }

                    // RIGHT_READYのとき前フレームのジャイロ値と比較して前よりも小さくなったら(減速)トリガー
                    else if (m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_RIGHT_READY)
                    {
                        // 過去フレームのジャイロより現フレームのジャイロが大きい場合
                        if (m_prevGyro[nCnt].z > m_joyconL1.GetGyro().z)
                        {
                            // トリガーに切り替え
                            //Debug.Log("joyconL1 : <color=green>SHAKE_RIGHT_TRIGGER</color>");
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_RIGHT_TRIGGER;
                            m_joyconL1.SetRumble(100, 9, 0.6f, 1);
                        }
                    }
                    // UP_READYのとき前フレームのジャイロ値と比較して前よりも大きくなったら(減速)トリガー
                    else if (m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_UP_READY)
                    {
                        // 過去フレームのジャイロより現フレームのジャイロが大きい場合
                        if (m_prevGyro[nCnt].y > m_joyconL1.GetGyro().y)
                        {
                            // トリガーに切り替え
                            //Debug.Log("joyconL1 : <color=orange>SHAKE_UP_TRIGGER</color>");
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_UP_TRIGGER;
                            m_joyconL1.SetRumble(160, 320, 0.6f, 1);
                        }
                    }


                    // TRIGGERのときNONEに戻す
                    else if (m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_DOWN_TRIGGER ||
                        m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_LEFT_TRIGGER ||
                        m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_RIGHT_TRIGGER ||
                        m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_UP_TRIGGER)
                    {
                        m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_NONE;
                    }
                }
            }
            else if ( nCnt == 1 )
            {
                // R1が有効の場合
                if (m_IsEnableJoyconR1 == true)
                {
                    // NONEのときREADYに入る
                    if (m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_NONE)
                    {
                        // FRONT_STARTの条件を満たしている場合
                        float y = m_joyconR1.GetGyro().y;
                        if (SHAKE_FRONT_START_VALUE_Y >= y)
                        {
                            // FRONT_READYに切り替え
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_DOWN_READY;
                            // 現フレームのジャイロを格納
                            m_prevGyro[nCnt] = m_joyconR1.GetGyro();
                        }

                        // LEFT_STARTの条件を満たしている場合
                        float z = m_joyconR1.GetGyro().z;
                        if (SHAKE_LEFT_START_VALUE_Z >= z)
                        {
                            // LEFT_READYに切り替え
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_LEFT_READY;
                            // 現フレームのジャイロを格納
                            m_prevGyro[nCnt] = m_joyconR1.GetGyro();

                        }

                        // RIGHT_STARTの条件を満たしている場合
                        z = m_joyconR1.GetGyro().z;
                        if (SHAKE_RIGHT_START_VALUE_Z <= z)
                        {
                            // LEFT_READYに切り替え
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_RIGHT_READY;
                            // 現フレームのジャイロを格納
                            m_prevGyro[nCnt] = m_joyconR1.GetGyro();
                        }

                        // UP_STARTの条件を満たしている場合
                        y = m_joyconR1.GetGyro().y;
                        if (SHAKE_UP_START_VALUE_Y <= y)
                        {
                            // FRONT_READYに切り替え
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_UP_READY;
                            // 現フレームのジャイロを格納
                            m_prevGyro[nCnt] = m_joyconR1.GetGyro();
                        }
                    }
                    // FRONT_READYのとき前フレームのジャイロ値と比較して前よりも大きくなったら(減速)トリガー
                    else if (m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_DOWN_READY)
                    {
                        // 過去フレームのジャイロより現フレームのジャイロが大きい場合
                        if (m_prevGyro[nCnt].y < m_joyconR1.GetGyro().y)
                        {
                            // トリガーに切り替え
                            //Debug.Log("joyconR1 : <color=red>SHAKE_FRONT_TRIGGER</color>");
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_DOWN_TRIGGER;
                            m_joyconR1.SetRumble(160, 320, 0.6f, 1);
                        }
                    }

                    // LEFT_READYのとき前フレームのジャイロ値と比較して前よりも大きくなったら(減速)トリガー
                    else if (m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_LEFT_READY)
                    {
                        // 過去フレームのジャイロより現フレームのジャイロが大きい場合
                        if (m_prevGyro[nCnt].z < m_joyconR1.GetGyro().z)
                        {
                            // トリガーに切り替え
                            //Debug.Log("joyconR1 : <color=blue>SHAKE_LEFT_TRIGGER</color>");
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_LEFT_TRIGGER;
                            m_joyconR1.SetRumble(100, 100, 0.6f, 1);
                        }
                    }

                    // RIGHT_READYのとき前フレームのジャイロ値と比較して前よりも小さくなったら(減速)トリガー
                    else if (m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_RIGHT_READY)
                    {
                        // 過去フレームのジャイロより現フレームのジャイロが大きい場合
                        if (m_prevGyro[nCnt].z > m_joyconR1.GetGyro().z)
                        {
                            // トリガーに切り替え
                            //Debug.Log("joyconR1 : <color=green>SHAKE_RIGHT_TRIGGER</color>");
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_RIGHT_TRIGGER;
                            m_joyconR1.SetRumble(100, 9, 0.6f, 1);
                        }
                    }
                    // UP_READYのとき前フレームのジャイロ値と比較して前よりも大きくなったら(減速)トリガー
                    else if (m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_UP_READY)
                    {
                        // 過去フレームのジャイロより現フレームのジャイロが大きい場合
                        if (m_prevGyro[nCnt].y > m_joyconR1.GetGyro().y)
                        {
                            // トリガーに切り替え
                            //Debug.Log("joyconR1 : <color=orange>SHAKE_UP_TRIGGER</color>");
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_UP_TRIGGER;
                            m_joyconR1.SetRumble(160, 320, 0.6f, 1);
                        }
                    }

                    // TRIGGERのときNONEに戻す
                    else if (m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_DOWN_TRIGGER ||
                        m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_LEFT_TRIGGER ||
                        m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_RIGHT_TRIGGER||
                        m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_UP_TRIGGER )
                    {
                        m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_NONE;
                    }
                }
            }
            else if ( nCnt == 2)
            {
                // R2が有効の場合
                if ( m_IsEnableJoyconR2 == true)
                {
                    // NONEのときREADYに入る
                    if (m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_NONE)
                    {
                        // FRONT_STARTの条件を満たしている場合
                        float y = m_joyconR2.GetGyro().y;
                        if (SHAKE_FRONT_START_VALUE_Y >= y)
                        {
                            // FRONT_READYに切り替え
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_DOWN_READY;
                            // 現フレームのジャイロを格納
                            m_prevGyro[nCnt] = m_joyconR2.GetGyro();
                        }

                        // LEFT_STARTの条件を満たしている場合
                        float z = m_joyconR2.GetGyro().z;
                        if (SHAKE_LEFT_START_VALUE_Z >= z)
                        {
                            // LEFT_READYに切り替え
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_LEFT_READY;
                            // 現フレームのジャイロを格納
                            m_prevGyro[nCnt] = m_joyconR2.GetGyro();

                        }

                        // RIGHT_STARTの条件を満たしている場合
                        z = m_joyconR2.GetGyro().z;
                        if (SHAKE_RIGHT_START_VALUE_Z <= z)
                        {
                            // LEFT_READYに切り替え
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_RIGHT_READY;
                            // 現フレームのジャイロを格納
                            m_prevGyro[nCnt] = m_joyconR2.GetGyro();
                        }

                        // UP_STARTの条件を満たしている場合
                        y = m_joyconR2.GetGyro().y;
                        if (SHAKE_UP_START_VALUE_Y <= y)
                        {
                            // FRONT_READYに切り替え
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_UP_READY;
                            // 現フレームのジャイロを格納
                            m_prevGyro[nCnt] = m_joyconR2.GetGyro();
                        }
                    }
                    // FRONT_READYのとき前フレームのジャイロ値と比較して前よりも大きくなったら(減速)トリガー
                    else if (m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_DOWN_READY)
                    {
                        // 過去フレームのジャイロより現フレームのジャイロが大きい場合
                        if (m_prevGyro[nCnt].y < m_joyconR2.GetGyro().y)
                        {
                            // トリガーに切り替え
                            //Debug.Log("joyconR2 : <color=red>SHAKE_FRONT_TRIGGER</color>");
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_DOWN_TRIGGER;
                            m_joyconR2.SetRumble(160, 320, 0.6f, 1);
                        }
                    }

                    // LEFT_READYのとき前フレームのジャイロ値と比較して前よりも大きくなったら(減速)トリガー
                    else if (m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_LEFT_READY)
                    {
                        // 過去フレームのジャイロより現フレームのジャイロが大きい場合
                        if (m_prevGyro[nCnt].z < m_joyconR2.GetGyro().z)
                        {
                            // トリガーに切り替え
                            //Debug.Log("joyconR2 : <color=blue>SHAKE_LEFT_TRIGGER</color>");
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_LEFT_TRIGGER;
                            m_joyconR2.SetRumble(100, 100, 0.6f, 1);
                        }
                    }

                    // RIGHT_READYのとき前フレームのジャイロ値と比較して前よりも小さくなったら(減速)トリガー
                    else if (m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_RIGHT_READY)
                    {
                        // 過去フレームのジャイロより現フレームのジャイロが大きい場合
                        if (m_prevGyro[nCnt].z > m_joyconR2.GetGyro().z)
                        {
                            // トリガーに切り替え
                            //Debug.Log("joyconR2 : <color=green>SHAKE_RIGHT_TRIGGER</color>");
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_RIGHT_TRIGGER;
                            m_joyconR2.SetRumble(100, 9, 0.6f, 1);
                        }
                    }

                    // UP_READYのとき前フレームのジャイロ値と比較して前よりも大きくなったら(減速)トリガー
                    else if (m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_UP_READY)
                    {
                        // 過去フレームのジャイロより現フレームのジャイロが大きい場合
                        if (m_prevGyro[nCnt].y > m_joyconR2.GetGyro().y)
                        {
                            // トリガーに切り替え
                            //Debug.Log("joyconR2 : <color=orange>SHAKE_UP_TRIGGER</color>");
                            m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_UP_TRIGGER;
                            m_joyconR2.SetRumble(160, 320, 0.6f, 1);
                        }
                    }

                    // TRIGGERのときNONEに戻す
                    else if (m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_DOWN_TRIGGER ||
                        m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_LEFT_TRIGGER ||
                        m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_RIGHT_TRIGGER ||
                        m_stateShakeJoycon[nCnt] == SHAKE_STATE.SHAKE_UP_TRIGGER)
                    {
                        m_stateShakeJoycon[nCnt] = SHAKE_STATE.SHAKE_NONE;
                    }
                }
            }

        }





            // 以下が追記のコード
            const float MOVE_PER_CLOCK = 0.01f;
        Vector3 joyconGyro;
        Quaternion rcqt;
        //―――――――――――――――――――――――
        // RightController1
        //―――――――――――――――――――――――
       /* if (m_IsEnableJoyconR1 == true)
        {
            joyconGyro = m_joyconR1.GetGyro();
            rcqt = rCube1.transform.rotation;
            rcqt.x += -joyconGyro[1] * MOVE_PER_CLOCK;
            rcqt.y += -joyconGyro[0] * MOVE_PER_CLOCK;
            rcqt.z += -joyconGyro[2] * MOVE_PER_CLOCK;
            rCube1.transform.rotation = rcqt;

            //// 右肩
            //Quaternion rb = RS_bone.transform.rotation * Quaternion.Inverse(riq);
            //rb.x += -joyconGyro[1] * MOVE_PER_CLOCK;
            //rb.y += -joyconGyro[0] * MOVE_PER_CLOCK;
            //rb.z += -joyconGyro[2] * MOVE_PER_CLOCK;
            //RS_bone.rotation = rb * riq;

            // 右のジョイコンのAボタンが押されたら、右の箱と右肩は初期ポジションに戻る
            if (m_joyconR1.GetButtonDown(m_buttons[1]))
            {
                rCube1.transform.rotation = rciq1;
                //RS_bone.rotation = riq;
            }
        }
        //―――――――――――――――――――――――
        // RightController2
        //―――――――――――――――――――――――
        if (m_IsEnableJoyconR2 == true)
        {
            joyconGyro = m_joyconR2.GetGyro();
            rcqt = rCube2.transform.rotation;
            rcqt.x += -joyconGyro[1] * MOVE_PER_CLOCK;
            rcqt.y += -joyconGyro[0] * MOVE_PER_CLOCK;
            rcqt.z += -joyconGyro[2] * MOVE_PER_CLOCK;
            rCube2.transform.rotation = rcqt;

            //// 右肩
            //Quaternion rb = RS_bone.transform.rotation * Quaternion.Inverse(riq);
            //rb.x += -joyconGyro[1] * MOVE_PER_CLOCK;
            //rb.y += -joyconGyro[0] * MOVE_PER_CLOCK;
            //rb.z += -joyconGyro[2] * MOVE_PER_CLOCK;
            //RS_bone.rotation = rb * riq;

            // 右のジョイコンのAボタンが押されたら、右の箱と右肩は初期ポジションに戻る
            if (m_joyconR2.GetButtonDown(m_buttons[1]))
            {
                rCube2.transform.rotation = rciq2;
                //RS_bone.rotation = riq;
            }
        }
        //―――――――――――――――――――――――
        // LeftController
        //―――――――――――――――――――――――
        if (m_IsEnableJoyconL1 == true)
        {
            // 左の箱
            joyconGyro = m_joyconL1.GetGyro();
            Quaternion lcqt = lCube.transform.rotation;
            lcqt.x += -joyconGyro[1] * MOVE_PER_CLOCK;
            lcqt.y += -joyconGyro[0] * MOVE_PER_CLOCK;
            lcqt.z += -joyconGyro[2] * MOVE_PER_CLOCK;
            lCube.transform.rotation = lcqt;

            //// 左肩
            //Quaternion lb = LS_bone.transform.rotation * Quaternion.Inverse(liq);
            //lb.x += -joyconGyro[1] * MOVE_PER_CLOCK;
            //lb.y += -joyconGyro[0] * MOVE_PER_CLOCK;
            //lb.z += -joyconGyro[2] * MOVE_PER_CLOCK;
            //LS_bone.rotation = lb * liq;

            // 左のジョイコンのAボタンが押されたら、左の箱と左肩は初期ポジションに戻る
            if (m_joyconL1.GetButtonDown(m_buttons[1]))
            {
                lCube.transform.rotation = lciq;
                //LS_bone.rotation = liq;
            }
        }
        */




    }

    // 他スクリプトからの呼び出し
    public JOYCON_STATE GetJoyconState ( JOYCON_TYPE joyconType)
    {
        if ( joyconType == JOYCON_TYPE.JOYCON_L1 )
        {
            if (m_stateShakeJoycon[0] == SHAKE_STATE.SHAKE_DOWN_TRIGGER)
                return JOYCON_STATE.STATE_DOWN_TRIGGER;
            else if (m_stateShakeJoycon[0] == SHAKE_STATE.SHAKE_RIGHT_TRIGGER)
                return JOYCON_STATE.STATE_RIGHT_TRIGGER;
            else if (m_stateShakeJoycon[0] == SHAKE_STATE.SHAKE_LEFT_TRIGGER)
                return JOYCON_STATE.STATE_LEFT_TRIGGER;
            else if (m_stateShakeJoycon[0] == SHAKE_STATE.SHAKE_UP_TRIGGER)
                return JOYCON_STATE.STATE_UP_TRIGGER;
            else
                return JOYCON_STATE.STATE_NONE;
            
        }
        else if ( joyconType == JOYCON_TYPE.JOYCON_R1)
        {
            if (m_stateShakeJoycon[1] == SHAKE_STATE.SHAKE_DOWN_TRIGGER)
                return JOYCON_STATE.STATE_DOWN_TRIGGER;
            else if (m_stateShakeJoycon[1] == SHAKE_STATE.SHAKE_RIGHT_TRIGGER)
                return JOYCON_STATE.STATE_RIGHT_TRIGGER;
            else if (m_stateShakeJoycon[1] == SHAKE_STATE.SHAKE_LEFT_TRIGGER)
                return JOYCON_STATE.STATE_LEFT_TRIGGER;
            else if (m_stateShakeJoycon[1] == SHAKE_STATE.SHAKE_UP_TRIGGER)
                return JOYCON_STATE.STATE_UP_TRIGGER;
            else
                return JOYCON_STATE.STATE_NONE;
        }
        else if ( joyconType == JOYCON_TYPE.JOYCON_R2)
        {
            if (m_stateShakeJoycon[2] == SHAKE_STATE.SHAKE_DOWN_TRIGGER)
                return JOYCON_STATE.STATE_DOWN_TRIGGER;
            else if (m_stateShakeJoycon[2] == SHAKE_STATE.SHAKE_RIGHT_TRIGGER)
                return JOYCON_STATE.STATE_RIGHT_TRIGGER;
            else if (m_stateShakeJoycon[2] == SHAKE_STATE.SHAKE_LEFT_TRIGGER)
                return JOYCON_STATE.STATE_LEFT_TRIGGER;
            else if (m_stateShakeJoycon[2] == SHAKE_STATE.SHAKE_UP_TRIGGER)
                return JOYCON_STATE.STATE_UP_TRIGGER;
            else
                return JOYCON_STATE.STATE_NONE;
        }

        //Debug.Log("Error : out of range");
        return JOYCON_STATE.STATE_NONE;
    }


   /* private void OnGUI()
    {
        var style = GUI.skin.GetStyle("label");
        style.fontSize = 18;

        

        GUILayout.BeginHorizontal(GUILayout.Width(960));

        foreach (var joycon in m_joycons)
        {
            var isLeft = joycon.isLeft;
            var name = isLeft ? "Joy-Con (L)" : "Joy-Con (R)";
            var key = isLeft ? "Z キー" : "X キー";
            var button = isLeft ? m_pressedButtonL1 : m_pressedButtonR1;
            var stick = joycon.GetStick();
            var gyro = joycon.GetGyro();
            var accel = joycon.GetAccel();
            var orientation = joycon.GetVector();

            GUILayout.BeginVertical(GUILayout.Width(480));
            GUILayout.Label(name);
            GUILayout.Label(key + "：振動");
            GUILayout.Label("押されているボタン：" + button);

            if ( m_joyconR1.GetButton(Joycon.Button.DPAD_LEFT) == false &&
                m_joyconR1.GetButtonDown(Joycon.Button.DPAD_LEFT) == false)
            {
                
            }
            else
            {
                if ( !isLeft)
                    //Debug.Log("<R1> ジャイロ：" + gyro);
              
            }

            if (m_joyconR1.GetButton(Joycon.Button.DPAD_DOWN) == false &&
               m_joyconR1.GetButtonDown(Joycon.Button.DPAD_DOWN) == false)
            {

            }
            else
            {
                if (!isLeft)
                    //Debug.Log("<R1> 加速度：" + accel);
            }



            
            if (m_joyconL1.GetButton(Joycon.Button.DPAD_LEFT) == false &&
                 m_joyconL1.GetButtonDown(Joycon.Button.DPAD_LEFT) == false)
            {

            }
            else
            {
                if (isLeft)
                    //Debug.Log("<L1> ジャイロ：" + gyro);
            }

            if (m_joyconL1.GetButton(Joycon.Button.DPAD_DOWN) == false &&
               m_joyconL1.GetButtonDown(Joycon.Button.DPAD_DOWN) == false)
            {

            }
            else
            {
                if (isLeft)
                    //Debug.Log("<L1> 加速度：" + accel);
            }
            

            GUILayout.Label(string.Format("スティック：({0}, {1})", stick[0], stick[1]));
            GUILayout.Label("ジャイロ：" + gyro);
            GUILayout.Label("加速度：" + accel);
            GUILayout.Label("傾き：" + orientation);
            GUILayout.EndVertical();
        }

        GUILayout.EndHorizontal();
    }*/
}