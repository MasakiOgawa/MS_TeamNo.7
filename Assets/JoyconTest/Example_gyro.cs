using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Example_gyro : MonoBehaviour
{
    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL1;
    private Joycon m_joyconR1;
    private Joycon m_joyconR2;
    private Joycon.Button? m_pressedButtonL1;
    private Joycon.Button? m_pressedButtonR1;
    private Joycon.Button? m_pressedButtonR2;


    //以下が追記のコード
    private GameObject rCube1 , rCube2, lCube;
    private Quaternion rciq1, rciq2, lciq, riq1, riq2, liq;
    //private Animator anim;
    //private Transform RS_bone, LS_bone;

    private void Start()
    {
        // ジョイコン情報を取得
        m_joycons = JoyconManager.Instance.j;

        // ジョイコンが存在しない。数が0以下の場合終了
        if (m_joycons == null || m_joycons.Count <= 0) return;

        // Left値を取得、値に応じて格納
        //m_joyconL1 = m_joycons.Find(c => c.isLeft);
        //m_joyconR1 = m_joycons.Find(c => !c.isLeft);

        // 現在接続されているジョイコンの数を表示
        Debug.Log("Connected JoyCon ->" + m_joycons.Count);

        // ３本に満たない場合
        if (m_joycons.Count < 3) Debug.Log("Warning : Not Enough JoyCon");

        // ３本ある場合
        if (m_joycons.Count >= 3)
        {
            int nCntRight = 0;

            for ( int nCnt = 0 ; nCnt < m_joycons.Count; nCnt ++)
            {
                // 現在のジョイコンのインデックス番号
                Debug.Log("---Now JoyCon No." + nCnt + "-------------------------------");

                // 今見てるコントローラーが右なのか左なのか
                if (m_joycons[nCnt].isLeft)
                {
                    Debug.Log("LeftController");

                    // Leftコントローラーを格納
                    m_joyconL1 = m_joycons[nCnt];
                }
                else
                {
                    Debug.Log("RightController");

                    // 右1本目
                    if (nCntRight == 0)
                    {
                        // Rightコントローラーを格納
                        m_joyconR1 = m_joycons[nCnt];
                        nCntRight++;
                    }
                    else
                    {
                        // Rightコントローラーを格納
                        m_joyconR2 = m_joycons[nCnt];
                    }
                }
            }
        }

        // 以下が追記のコード
        //anim = (Animator)FindObjectOfType(typeof(Animator));

        rCube1 = GameObject.Find("RightCube1");
        rCube2 = GameObject.Find("RightCube2");
        lCube = GameObject.Find("LeftCube1");

        //RS_bone = anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
        //LS_bone = anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);

        rciq1 = rCube1.transform.rotation;
        rciq2 = rCube2.transform.rotation;
        lciq = lCube.transform.rotation;
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
            if (m_joyconL1.GetButton(button))
            {
                m_pressedButtonL1 = button;
            }
            if (m_joyconR1.GetButton(button))
            {
                m_pressedButtonR1 = button;
            }
            if (m_joyconR2.GetButton(button))
            {
                m_pressedButtonR2 = button;
            }
        }

        // ジョイコンが入っていない場合
        if (m_joycons == null || m_joycons.Count <= 0) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            m_joyconL1.SetRumble(160, 320, 0.6f, 200);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            m_joyconR1.SetRumble(160, 320, 0.6f, 200);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            m_joyconR2.SetRumble(160, 320, 0.6f, 200);
        }

        // 以下が追記のコード
        const float MOVE_PER_CLOCK = 0.01f;
        Vector3 joyconGyro;
        //―――――――――――――――――――――――
        // RightController1
        //―――――――――――――――――――――――
        joyconGyro = m_joyconR1.GetGyro();
        Quaternion rcqt = rCube1.transform.rotation;
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
        //―――――――――――――――――――――――
        // RightController2
        //―――――――――――――――――――――――
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

        //―――――――――――――――――――――――
        // LeftController
        //―――――――――――――――――――――――
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

    private void OnGUI()
    {
        var style = GUI.skin.GetStyle("label");
        style.fontSize = 24;

        if (m_joycons == null || m_joycons.Count <= 0)
        {
            GUILayout.Label("Joy-Con が接続されていません");
            return;
        }

        if (!m_joycons.Any(c => c.isLeft))
        {
            GUILayout.Label("Joy-Con (L) が接続されていません");
            return;
        }

        if (!m_joycons.Any(c => !c.isLeft))
        {
            GUILayout.Label("Joy-Con (R) が接続されていません");
            return;
        }

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
            GUILayout.Label(string.Format("スティック：({0}, {1})", stick[0], stick[1]));
            GUILayout.Label("ジャイロ：" + gyro);
            GUILayout.Label("加速度：" + accel);
            GUILayout.Label("傾き：" + orientation);
            GUILayout.EndVertical();
        }

        GUILayout.EndHorizontal();
    }
}