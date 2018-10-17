using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CMCameraManager : MonoBehaviour {

    // 各カメラの呼び出し制御
    [SerializeField] private GameObject MainCamera;
    [SerializeField] private GameObject[] CutScene;
    [SerializeField] private GameObject[] CutSceneCamera;
    private bool[] isPlayingCutScene;

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

        // 作業用BGM再生
        if (Input.GetKeyDown(KeyCode.M))
        {
           
        }

        // 0を押してシーン0を再生
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetCutScene(0);

        }

        // 1を押してシーン1を再生
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetCutScene(1);

 
        }

        // 2を押してシーン2を再生
        if (Input.GetKeyDown(KeyCode.Alpha2))
            
        {
            SetCutScene(2);

        }
        // 3を押してシーン3を再生
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetCutScene(3);

        }
        // 4を押してシーン4を再生
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetCutScene(4);

        }

        // 5を押してシーン5を再生
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
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
