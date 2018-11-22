using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CMCameraManager : MonoBehaviour {

    // 各カメラの呼び出し制御
    [SerializeField] private GameObject MainCamera;
    [SerializeField] private GameObject[] CutScene;
    [SerializeField] private GameObject[] CutSceneCamera;
    private bool[] isPlayingCutScene;

    [SerializeField] private float RandomCutScenePlayTime;
    private float CutScenePlayTimeDelta;

	// Use this for initialization
	void Start () {
        
        // 初期化
        for ( int n = 0; n < CutSceneCamera.Length; n ++ )
        {
            CutScene[n].SetActive(false);
            CutSceneCamera[n].SetActive(false);
        }

        // CutSceneの数だけ再生フラグを作成
        isPlayingCutScene = new bool[CutSceneCamera.Length];

        // 再生フラグ初期化
        for ( int n = 0; n < CutSceneCamera.Length; n ++)
        {
            isPlayingCutScene[n] = false;
        }
        CutScenePlayTimeDelta =0;
    }
	
	// Update is called once per frame
	void Update () {

        // カットシーンの数だけ繰り返す
        for ( int n = 0; n < CutScene.Length; n ++)
        {
            // どれかのカットシーンを再生している場合
            if ( isPlayingCutScene[n] == true)
            {
                //Debug.Log("再生終了1");
                // 再生終了した場合
                PlayableDirector pDirector = CutSceneCamera[n].GetComponent<PlayableDirector>();
                if (pDirector.state != PlayState.Playing)
                {

                    // Mainカメラを復活させる
                    MainCamera.GetComponent<Camera>().enabled = true;
                    MainCamera.GetComponent<AudioListener>().enabled = true;

                    // CutScene , CutSceneCameraを終了させる
                    CutScene[n].SetActive(false);
                    CutSceneCamera[n].SetActive(false);

                    //Debug.Log("再生終了2");
                    // 終了フラグにする
                    isPlayingCutScene[n] = false;
                }
            }
        }

        // カットシーンの数だけ繰り返す
        for (int n = 0; n < CutScene.Length; n++)
        {
            // どれかのカットシーンを再生している場合
            if (isPlayingCutScene[n] == true)
                return;
        }

        if (SceneManager.GetActiveScene().name == "Title_2.0")
        {
            // 以降どのカットシーンも再生していない場合
            // 時間加算
            CutScenePlayTimeDelta += Time.deltaTime;

            if (CutScenePlayTimeDelta > RandomCutScenePlayTime)
            {
                int rand = Random.RandomRange(0, 5);

                SetCutScene(rand);

                CutScenePlayTimeDelta = 0;

            }
        }


        // 作業用BGM再生
        if (Input.GetKeyDown(KeyCode.M))
        {
           
        }

        // 0を押してシーン0を再生
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CutScenePlayTimeDelta = 0;
            SetCutScene(0);

        }

        // 1を押してシーン1を再生
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CutScenePlayTimeDelta = 0;
            SetCutScene(1);

 
        }

        // 2を押してシーン2を再生
        if (Input.GetKeyDown(KeyCode.Alpha2))
            
        {
            CutScenePlayTimeDelta = 0;
            SetCutScene(2);

        }
        // 3を押してシーン3を再生
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CutScenePlayTimeDelta = 0;
            SetCutScene(3);

        }
        // 4を押してシーン4を再生
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CutScenePlayTimeDelta = 0;
            SetCutScene(4);

        }

        // 5を押してシーン5を再生
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CutScenePlayTimeDelta = 0;
            SetCutScene(5);
            // BGMを再生する
            //GetComponent<AudioSource>().Play();
        }



    }

    // 外部からカットシーンの呼び出し
    public void SetCutScene ( int SceneNo )
    {
        // カットシーンの数だけ繰り返す
        for (int n = 0; n < CutScene.Length; n++)
        {
            // どれかのカットシーンを再生している場合
            if (isPlayingCutScene[n] == true)
                return;
        }


        PlayableDirector pd = CutSceneCamera[SceneNo].GetComponent<PlayableDirector>();

        // メインカメラを終了させる
        //MainCamera.SetActive(false);
        MainCamera.GetComponent<Camera>().enabled = false;
        MainCamera.GetComponent<AudioListener>().enabled = false;

        // CutScene , CutSceneCameraを起動する
        CutScene[SceneNo].SetActive(true);
        CutSceneCamera[SceneNo].SetActive(true);
        // 再生する
        pd.Play();
        isPlayingCutScene[SceneNo] = true;
    }
}
