using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class iconController : MonoBehaviour {

    [SerializeField]
    private float FadeTime;     // フェードの時間

    [SerializeField]
    private string SceneName;   // シーン遷移させたいシーンの名前

    [SerializeField] public float reverseTime;

    // shakeItからreadyテクスチャに変更管理画面遷移

    [SerializeField] private GameObject shakeIt1;
    [SerializeField] private GameObject shakeIt2;
    [SerializeField] private GameObject shakeIt3;
    [SerializeField] private GameObject ready1;
    [SerializeField] private GameObject ready2;
    [SerializeField] private GameObject ready3;

    private bool isReady1;
    private bool isReady2;
    private bool isReady3;

    // ジョイコンスクリプト
    Example_gyro gyro;

    // タイムカウンタ(Readyから一定時間でshakeItに後退)
    private float seconds;




    // Use this for initialization
    void Start () {
        if (shakeIt1 == null || shakeIt2 == null || shakeIt3 == null)
            Debug.Log("error : shakeIt GameObject none");

        //if ( ready1 == null || ready2 = null || ready3 == null)
        //    Debug.Log("error : ready GameObject none");

        isReady1 = false;
        isReady2 = false;
        isReady3 = false;

        // ジャイロ取得
        gyro = GetComponent<Example_gyro>();

        //  タイムカウンタ
        seconds = 0;

    }
	
	// Update is called once per frame
	void Update () {

        Example_gyro.JOYCON_STATE state;
        
        // 1:falseのときジョイコン取って見てtrue
        if ( isReady1 == false)
        {
            state = gyro.GetJoyconState(Example_gyro.JOYCON_TYPE.JOYCON_L1);
            // 振られた
            if ( state == Example_gyro.JOYCON_STATE.STATE_DOWN_TRIGGER)
            {
                isReady1 = true;
                shakeIt1.SetActive(false);
                ready1.SetActive(true);
                seconds = 0;
            }
        }
        // readyのとき
        else
        {
            if ( seconds > reverseTime)
            {
                isReady1 = false;
                shakeIt1.SetActive(true);
                ready1.SetActive(false);
            }
        }

        // 2:falseのときジョイコン取って見てtrue
        if (isReady2 == false)
        {
            state = gyro.GetJoyconState(Example_gyro.JOYCON_TYPE.JOYCON_R1);
            // 振られた
            if (state == Example_gyro.JOYCON_STATE.STATE_DOWN_TRIGGER)
            {
                isReady2 = true;
                shakeIt2.SetActive(false);
                ready2.SetActive(true);
                seconds = 0;
            }
        }
        // readyのとき
        else
        {
            if (seconds > reverseTime)
            {
                isReady2 = false;
                shakeIt2.SetActive(true);
                ready2.SetActive(false);
            }
        }

        // 3:falseのときジョイコン取って見てtrue
        if (isReady3 == false)
        {
            state = gyro.GetJoyconState(Example_gyro.JOYCON_TYPE.JOYCON_R2);
            // 振られた
            if (state == Example_gyro.JOYCON_STATE.STATE_DOWN_TRIGGER)
            {
                isReady3 = true;
                shakeIt3.SetActive(false);
                ready3.SetActive(true);
                seconds = 0;
            }
        }
        // readyのとき
        else
        {
            if (seconds > reverseTime)
            {
                isReady3 = false;
                shakeIt3.SetActive(true);
                ready3.SetActive(false);
                seconds = 0;
            }
        }


        //どれか一つ以上がtrueのとき
        if (isReady1 == true || isReady2 == true || isReady3 == true)
        {         // カウンタ回す
            seconds += Time.deltaTime;
        }

        //3つがtrueのとき
        if (isReady1 == true && isReady2 == true && isReady3 == true)
        {
            //scene change
            FadeManager.Instance.LoadScene(SceneName, FadeTime);
        }
    }
}
