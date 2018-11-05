using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    
    // score
    private int _score;
    private int _maxScore;
    // 起動フラグ
    private bool isEnableScoreCounter;

    private AudioSource ScoreCounterSE;
    private AudioSource ScoreStopSE;
    

    // Use this for initialization
    void Start () {


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

            // スコア表示処理
            
            // 現在の桁数に応じて分岐
            
            // 100で割って0以上の場合->3桁
            if ( _score / 100 > 0)
            {
                NumberObj100.SetActive(true);
                NumberObj10.SetActive(true);
                NumberObj1.SetActive(true);

                // 100
                int score100 = _score / 100;
                NumberAsset.GetComponent<NumberController>().SetSprite(score100, NumberObj100);

                // 10
                int score10 = _score % 100;
                score10 = score10 / 10;
                NumberAsset.GetComponent<NumberController>().SetSprite(score10, NumberObj10);
                
                // 1
                int score1 = _score % 10;
                NumberAsset.GetComponent<NumberController>().SetSprite(score1, NumberObj1);

            }
            // 10で割って0
            else if ( _score / 10 > 0)
            {
                NumberObj100.SetActive(false);
                NumberObj10.SetActive(true);
                NumberObj1.SetActive(true);

                // 10
                int score10 = _score % 100;
                score10 = score10 / 10;
                NumberAsset.GetComponent<NumberController>().SetSprite(score10, NumberObj10);

                // 1
                int score1 = _score % 10;
                NumberAsset.GetComponent<NumberController>().SetSprite(score1, NumberObj1);
            }
            // 一桁しかない
            else
            {
                NumberObj100.SetActive(false);
                NumberObj10.SetActive(false);
                NumberObj1.SetActive(true);
                // 1
                int score1 = _score % 10;
                NumberAsset.GetComponent<NumberController>().SetSprite(score1, NumberObj1);
            }



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
        
        _maxScore = score;

        int nZeroScore = 0;

        _score = nZeroScore;

        isEnableScoreCounter = true;
    }
}
