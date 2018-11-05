using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour {

    // スコア各桁
    [SerializeField] private GameObject NumberObj100;
    [SerializeField] private GameObject NumberObj10;
    [SerializeField] private GameObject NumberObj1;

    [SerializeField] private GameObject NumberAsset;


    // スコアカウンタ終了情報送信用
    [SerializeField] private GameObject _ResultManager;

    // スコア上昇幅
    [SerializeField] private int scoreUpValue;

    // TextMeshPro
    private TMPro.TextMeshProUGUI TMPscore;
    // score
    private int _score;
    private int _maxScore;
    // 起動フラグ
    private bool isEnableScoreCounter;

    private AudioSource ScoreCounterSE;
    private AudioSource ScoreStopSE;
    

    // Use this for initialization
    void Start () {
        TMPscore = GetComponent<TMPro.TextMeshProUGUI>();


    }
	
	// Update is called once per frame
	void Update () {
        if (isEnableScoreCounter == true)
        {
            // 回転中
            if (_score < _maxScore)
            {
                _score += scoreUpValue;

                // 振られたらスキップ
                if (Input.GetKeyDown(KeyCode.F2))
                {
                    _score = _maxScore;
                }

                // エラー回避 : あふれたら戻す
                if (_score > _maxScore)
                    _score = _maxScore;

            }
            // 回転終了
            else
            {
                isEnableScoreCounter = false;
                // スコアカウンタ終了合図を送る
                ResultManager rm = _ResultManager.GetComponent<ResultManager>();
                rm.GetFinishScoreCounter();

                ScoreCounterSE.Stop();
                ScoreStopSE.Play();


            }

            // このフレームで表示したいスコア
            //_score


        }
    }

    // スコア受け取り
    public void SetScore ( int score )
    {
        // SE・BGM取得
        AudioSource[] audioSources = GetComponents<AudioSource>();
        ScoreCounterSE = audioSources[0];
        ScoreStopSE = audioSources[1];

        ScoreCounterSE.Play();

        // ResultManagerからスコアを受け取り
        TMPscore = GetComponent<TMPro.TextMeshProUGUI>();
        _maxScore = score;

        int nZeroScore = 0;

        _score = nZeroScore;
        TMPscore.text = _score.ToString();

        isEnableScoreCounter = true;
    }
}
