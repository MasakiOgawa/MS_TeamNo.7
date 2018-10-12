using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RankingManager : MonoBehaviour {

    [SerializeField] private float _ranking1stScoreStartTime;   // 1st表示までの時間
    [SerializeField] private float _ranking2ndScoreStartTime;   // 2nd表示までの時間
    [SerializeField] private float _ranking3rdScoreStartTime;   // 3rd表示までの時間
    // 下層キャンバスの管理
    [SerializeField] private GameObject _1stScore;
    [SerializeField] private GameObject _2ndScore;
    [SerializeField] private GameObject _3rdScore;
    private bool bEnable1stScore;
    private bool bEnable2ndScore;
    private bool bEnable3rdScore;


    // Time
    private float fTime;

    // 起動フラグ
    private bool isEnableRankingManager;

    // ランキング保存キー
    private string _1stKey = "1ST";
    private string _2ndKey = "2ND";
    private string _3rdKey = "3RD";

    private int Rank1stScore;
    private int Rank2ndScore;
    private int Rank3rdScore;

    private int Rank;

    // Use this for initialization
    void Start () {
        // 初期化
        InitRankingManager();

    }
	
	// Update is called once per frame
	void Update () {

        // 未起動で時間超えた場合
        if ( bEnable1stScore == false && fTime > _ranking1stScoreStartTime)
        {
            _1stScore.SetActive(true);
            bEnable1stScore = true;
            // 表示スコア受け渡し
            TMPro.TextMeshProUGUI tmp = _1stScore.GetComponent<TMPro.TextMeshProUGUI>();
            tmp.text = Rank1stScore.ToString();

            if ( Rank ==1 )
               tmp.color = new Color(255, 0, 0, 255);
        }

        if ( bEnable2ndScore == false && fTime > _ranking2ndScoreStartTime)
        {
            _2ndScore.SetActive(true);
            bEnable2ndScore = true;
            // 表示スコア受け渡し
            TMPro.TextMeshProUGUI tmp = _2ndScore.GetComponent<TMPro.TextMeshProUGUI>();
            tmp.text = Rank2ndScore.ToString();

            if (Rank == 2)
                tmp.color = new Color(255, 0, 0, 255);
        }

        if ( bEnable3rdScore == false && fTime > _ranking3rdScoreStartTime)
        {
            _3rdScore.SetActive(true);
            bEnable3rdScore = true;
            // 表示スコア受け渡し
            TMPro.TextMeshProUGUI tmp = _3rdScore.GetComponent<TMPro.TextMeshProUGUI>();
            tmp.text = Rank3rdScore.ToString();

            if (Rank == 3)
                tmp.color = new Color(255, 0, 0, 255);

            fTime = 0.0f;
        }

        fTime += Time.deltaTime;

        if( bEnable3rdScore == true && fTime > 10.0f )
        {
            SceneManager.LoadScene( "Result" );
        }
	}

    // RankingManager起動
    public void StartRankingManager ( int nScore)
    {
        // 起動開始
        isEnableRankingManager = true;

        // スコア保存
        SaveRanking(nScore);
    }

    // 初期化
    public void InitRankingManager ()
    {
        isEnableRankingManager = false;

        _1stScore.SetActive(false);
        _2ndScore.SetActive(false);
        _3rdScore.SetActive(false);

        bEnable1stScore = false;
        bEnable3rdScore = false;
        bEnable2ndScore = false;
    }

    // セーブデータ呼び出し、今回のスコアを適用させてソート
    public void SaveRanking ( int nScore)
    {
        // セーブデータから取得
        Rank1stScore = PlayerPrefs.GetInt(_1stKey, 0);
        Rank2ndScore = PlayerPrefs.GetInt(_2ndKey, 0);
        Rank3rdScore = PlayerPrefs.GetInt(_3rdKey, 0);

        // ソート
        // 1stより大きい場合
        if ( Rank1stScore <= nScore)
        {
            Rank3rdScore = Rank2ndScore;
            Rank2ndScore = Rank1stScore;
            Rank1stScore = nScore;
            Rank = 1;
        }
        // 1st > score > 2nd
        else if ( Rank1stScore > nScore && nScore >= Rank2ndScore)
        {
            Rank3rdScore = Rank2ndScore;
            Rank2ndScore = nScore;
            Rank = 2;
        }
        // 2nd > score > 3rd
        else if ( Rank2ndScore > nScore && nScore >= Rank3rdScore )
        {
            Rank3rdScore = nScore;
            Rank = 3;
        }
        else
        {
            Rank = 0;
        }

        // 保存
        PlayerPrefs.SetInt(_1stKey, Rank1stScore );
        PlayerPrefs.SetInt(_2ndKey, Rank2ndScore);
        PlayerPrefs.SetInt(_3rdKey, Rank3rdScore);

        Debug.Log(Rank1stScore);
        Debug.Log(Rank2ndScore);
        Debug.Log(Rank3rdScore);
    }
}
