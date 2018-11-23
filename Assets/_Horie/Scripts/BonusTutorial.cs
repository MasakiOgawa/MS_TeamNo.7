using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTutorial : MonoBehaviour {

    [SerializeField] private GameObject BGObj;
    [SerializeField] private GameObject FrameObj;
    [SerializeField] private GameObject PikaObj;
    [SerializeField] private GameObject TitleObj;
    [SerializeField] private GameObject EG1Obj;
    [SerializeField] private GameObject EG2Obj;

    [SerializeField] private float WorkingTime;     // 稼働時間
    [SerializeField] private float AnimationDelta;  // アニメーション間隔 

    // 経過時間
    public float nowTime;

    // 起動確認フラグ
    private bool bCompleteBG;
    private bool bCompleteFrame;

    // EGアニメーションフェーズ
    private int PhaseAnim;      // 1or2
    private float EGTime;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // 時間進める
        nowTime += Time.deltaTime;
        
        // EG更新
        UpdateEG();

        // デバッグ　自殺
        if (Input.GetKeyDown(KeyCode.P))
        {
            Destroy(this.gameObject);
        }

        // 時間超えたら自殺
        if (WorkingTime < nowTime)
        {
            PikaObj.SetActive(false);
            TitleObj.SetActive(false);
            EG1Obj.SetActive(false);
            EG2Obj.SetActive(false);
            BGObj.GetComponent<BonusBG>().ShutDown();
            FrameObj.GetComponent<BonusBG>().ShutDown();
        }
    }

    // 初期化
    private void Run ()
    {
        // すべて無効化
        BGObj.SetActive(false);
        FrameObj.SetActive(false);
        PikaObj.SetActive(false);
        TitleObj.SetActive(false);
        EG1Obj.SetActive(false);
        EG2Obj.SetActive(false);

        // 変数初期化
        nowTime = 0;
        bCompleteBG = false;
        bCompleteFrame = false;
        PhaseAnim = 0;
        EGTime = 0;
        // BGとFrameを起動
        ActiveBG();

    }

    private void ActiveBG ()
    {
        // BGObjを起動させる
        BGObj.SetActive(true);
        BGObj.GetComponent<BonusBG>().Run();

        // BGFrameを起動させる
        FrameObj.SetActive(true);
        FrameObj.GetComponent<BonusBG>().Run();
    }

    // BG終了通知
    public void ReturnBG ( bool bFrame )
    {
        if (bFrame == true)
            bCompleteFrame = true;
        if (bFrame == false)
            bCompleteBG = true;

        // 両方trueになったら次の起動を開始する
        if ( bCompleteBG == true && bCompleteFrame == true)
        {
            // Font起動
            ActiveFont();

            // EG起動
            ActiveEG();
        }
    }

    public void ShutDownBG()
    {
        Destroy(this.gameObject);
    }


    // Font表示
    private void ActiveFont()
    {
        PikaObj.SetActive(true);
        PikaObj.GetComponent<BonusTitle>().Run();

        TitleObj.SetActive(true);

    }

    // EG表示
    private void ActiveEG()
    {
        EG1Obj.SetActive(true);
        PhaseAnim = 1;
    }
    // EG更新
    private void UpdateEG()
    {
        // 時間経過したら切り替える
        EGTime += Time.deltaTime;
        
        if ( EGTime > AnimationDelta)
        {
            if (PhaseAnim == 1)
            {
                EG1Obj.SetActive(false);
                EG2Obj.SetActive(true);

                EGTime = 0;
                PhaseAnim = 2;
            }
            else if ( PhaseAnim == 2 )
            {
                EG1Obj.SetActive(true);
                EG2Obj.SetActive(false);

                EGTime = 0;
                PhaseAnim = 1;
            }

        }

    }


    // 生成
    static public void Create()
    {
        GameObject prefab = (GameObject)Resources.Load("Prefabs/BonusTutorial");
        // プレハブからインスタンスを生成
        GameObject obj = Instantiate(prefab);

        obj.GetComponent<BonusTutorial>().Run();
    }

    
}
