using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour {
    // 起動関数を呼び出すとスコアのカウントとか始まる

    // scoreCounter
    [SerializeField] private GameObject _ResultLogo;
    [SerializeField] private GameObject _ScoreCounter;
    [SerializeField] private GameObject _RankingManager;
    [SerializeField] private GameObject _ResultBGOBJ;
    [SerializeField] private float _scoreCounterStartTime;      // ResultLogo出てからスコア回転開始までの時間
    [SerializeField] private float _rankingManagerStartTime;    // score回転終了してからランキング起動までの時間
    // 稼働中フラグ
    private bool isEnableResultManager;
    private bool isFinishScoreCounter;

    // 獲得スコア
    private int getScore;

    // 時間カウンタ
    private float fTime;

    private bool bRunningRankingManager;

	// Use this for initialization
	void Start () {

        isEnableResultManager = false;
        isFinishScoreCounter = false;
        // くっついてるゲームオブジェクトすべて非アクティブ化
        // 起動のときに起こす
        _ResultLogo.SetActive(false);
        _ScoreCounter.SetActive(false);
        _RankingManager.GetComponent<RankingManager>().InitRankingManager();
        _RankingManager.SetActive(false);
        _ResultBGOBJ.SetActive(false);
        bRunningRankingManager = false;

        //StartResult(ScoreManager.nScore);
	}
	
	// Update is called once per frame
	void Update () {
        // 起動中でない場合は返す
        if (!isEnableResultManager)
            return;

        // スコアカウンタがアクティブでない場合
        if (bRunningRankingManager == false && !_ScoreCounter.activeSelf && _scoreCounterStartTime < fTime)
        {
            // スコアカウンタの起動
            _ScoreCounter.SetActive(true);
            _ScoreCounter.GetComponent<ScoreCounter>().SetScore(getScore);
        }

        // スコア回転終了してマクロ時間経過した場合
        if (bRunningRankingManager == false && isFinishScoreCounter == true && _rankingManagerStartTime < fTime)
        {
            // ResultLogoとScoreの表示終了
            _ResultLogo.SetActive(false);
            _ScoreCounter.SetActive(false);

            // RankingManagerの起動
            _RankingManager.SetActive(true);
            _RankingManager.GetComponent<RankingManager>().StartRankingManager(getScore);

            bRunningRankingManager = true;
        }

        // タイムカウンタ回転
        fTime += Time.deltaTime;
    }

    // Resultモード起動関数
    public void StartResult( int score )
    {
        //BG起動
        _ResultBGOBJ.SetActive(true);
        _ResultBGOBJ.GetComponent<ResultBGController>().StartBG();



        // スコア受け取り
        getScore = score;



        

        // BGM
        GetComponent<AudioSource>().Play();
    }

    // BG 起動終わり
    public void EndBGStartResult ()
    {
        isEnableResultManager = true;
        // リザルトロゴの起動
        _ResultLogo.SetActive(true);
    }


    // Scoreカウンタから終了フラグをもらう
    public void GetFinishScoreCounter ( )
    {
        isFinishScoreCounter = true;

        // タイムカウンタ初期化
        fTime = 0;
    }

    public void StopBGM ()
    {
        GetComponent<AudioSource>().Stop();
    }
}
