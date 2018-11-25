using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RankingScoreController : MonoBehaviour {

    // スコア各桁
    [SerializeField] private GameObject NumberObj100;
    [SerializeField] private GameObject NumberObj10;
    [SerializeField] private GameObject NumberObj1;


    [SerializeField] private GameObject NumberAsset;

    [SerializeField]
    RectTransform rectTran;     // アニメーションしたい情報

    Tweener tweener;            // トゥイーンの情報

    [SerializeField] private float animTime;        // アニメーション時間
    [SerializeField] private float Scale;           // サイズ



    private bool bRankedIn;

    private int _score;
    // Use this for initialization
    void Start () {
        bRankedIn = false;

    }
	
	// Update is called once per frame
	void Update () {

	}
    private void OnDestroy()
    {
        // オブジェクトが消えてもアニメーションが止まらないので キルでアニメーションを消す
        tweener.Kill();
    }

    public void SetScore ( int nScore , bool bRanked )
    {
        _score = nScore;
        bRankedIn = bRanked;
        Debug.Log(_score);
        // 100で割って0以上の場合->3桁
        if (_score / 100 > 0)
        {
            NumberObj100.SetActive(true);
            NumberObj10.SetActive(true);
            NumberObj1.SetActive(true);

            // 100
            int score100 = _score / 100;
            NumberAsset.GetComponent<NumberController>().SetSprite(score100, NumberObj100);
            Debug.Log("100:"+score100);
            // 10
            int score10 = _score % 100;
            score10 = score10 / 10;
            NumberAsset.GetComponent<NumberController>().SetSprite(score10, NumberObj10);
            Debug.Log("10:" + score10);
            // 1
            int score1 = _score % 10;
            NumberAsset.GetComponent<NumberController>().SetSprite(score1, NumberObj1);
            Debug.Log("1:" + score1);
        }
        // 10で割って0
        else if (_score / 10 > 0)
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

        if (bRankedIn == true)
        {
            //Debug.Log("true");

            // 3秒かけて2倍の大きさに
            // スケールシェイク
            tweener = rectTran.DOPunchScale(
            new Vector3(Scale, Scale),    // scale1.5倍指定
            animTime,                        // アニメーション時間
            0
            ).SetLoops(-1);
        }
    }
}
