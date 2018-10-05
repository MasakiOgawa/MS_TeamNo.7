using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour {

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
            }
            TMPscore.text = _score.ToString();
        }
    }

    // スコア受け取り
    public void SetScore ( int score )
    {
        // ResultManagerからスコアを受け取り
        TMPscore = GetComponent<TMPro.TextMeshProUGUI>();
        _maxScore = score;

        int nZeroScore = 0;

        _score = nZeroScore;
        TMPscore.text = _score.ToString();

        isEnableScoreCounter = true;
    }
}
