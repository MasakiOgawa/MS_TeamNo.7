using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BonusTitle : MonoBehaviour {

    [SerializeField] private bool isPika;
    // Pika
    // フロー　表示->一瞬消える->一瞬表示->一瞬消える->表示に戻る
    [SerializeField] private float PikaWaitTime;        // 再度ピカ出し直しまでの時間
    [SerializeField] private float PikaSwapTime;        // ピカ点滅間隔
    private Image PikaImage;
    public int PhasePika;      //0:表示  1:消灯    2:一瞬表示    3:消灯
    

    // 共通
    public float nowTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        // ピカ用
        if ( isPika == true)
        {
            PikaUpdate();
        }

        // 共通
        nowTime += Time.deltaTime;
	}

    private void PikaUpdate()
    {
        switch (PhasePika)
        {
            // 表示
            case 0:
                {
                    // 一定時間待って、時間経ったら1へ行って消す
                    if ( nowTime > PikaWaitTime )
                    {
                        // 時間初期化
                        nowTime = 0;
                        // 非表示化
                        PikaImage.enabled = false;
                        // フェーズ以降
                        PhasePika = 1;
                    }
                    break;
                }
            // 消灯
            case 1:
                {
                    // インスペクターの時間(短時間)待って、時間経ったら2へ行って表示する
                    if ( nowTime > PikaSwapTime )
                    {
                        // 時間初期化
                        nowTime = 0;
                        // 表示
                        PikaImage.enabled = true;
                        // フェーズ以降
                        PhasePika = 2;
                    }
                    break;
                }
            // 再表示
            case 2:
                {
                    // 時間待って、時間経ったら非表示
                    if ( nowTime > PikaSwapTime )
                    {
                        // 時間非表示
                        nowTime = 0;
                        // 非表示
                        PikaImage.enabled = false;
                        // フェーズ以降
                        PhasePika = 3;
                    }
                    break;
                }
            // 消灯
            case 3:
                {
                    // 時間経ったら表示して0へ戻る
                    if ( nowTime > PikaSwapTime )
                    {
                        // 時間非表示
                        nowTime = 0;
                        // 表示
                        PikaImage.enabled = true;
                        // フェーズ以降
                        PhasePika = 0;
                    }
                    break;
                }
        }
    }

    private void TitleUpdate()
    {

    }

    // 起動
    public void Run()
    {
        // ピカ
        if ( isPika == true )
        {
            // ピカ画像表示コンポーネントを取得
            PikaImage = GetComponent<Image>();
            PhasePika = 1;
        }
        // Title
        else
        {

        }

        nowTime = 0;
    }
}
