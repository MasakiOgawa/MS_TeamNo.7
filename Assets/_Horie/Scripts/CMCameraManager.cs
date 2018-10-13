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
                Debug.Log("再生終了1");
                // 再生終了した場合
                PlayableDirector pDirector = CutSceneCamera[n].GetComponent<PlayableDirector>();
                if (pDirector.state != PlayState.Playing)
                {
                    // CutScene , CutSceneCameraを終了させる
                    CutScene[n].SetActive(false);
                    CutSceneCamera[n].SetActive(false);

                    // Mainカメラを復活させる
                    MainCamera.SetActive(true);
                    Debug.Log("再生終了2");
                    // 終了フラグにする
                    isPlayingCutScene[n] = false;
                }
            }
        }

        // 0を押してシーン0を再生
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // カットシーンの数だけ繰り返す
            for (int n = 0; n < CutScene.Length; n++)
            {
                // どれかのカットシーンを再生している場合
                if (isPlayingCutScene[n] == true)
                    return;
            }

            PlayableDirector pd = CutSceneCamera[0].GetComponent<PlayableDirector>();

            // メインカメラを終了させる
            MainCamera.SetActive(false);
            // CutScene , CutSceneCameraを起動する
            CutScene[0].SetActive(true);
            CutSceneCamera[0].SetActive(true);
            // 再生する
            pd.Play();
            isPlayingCutScene[0] = true;
            //Debug.Log("再生開始");
        }

        // 1を押してシーン1を再生
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // カットシーンの数だけ繰り返す
            for (int n = 0; n < CutScene.Length; n++)
            {
                // どれかのカットシーンを再生している場合
                if (isPlayingCutScene[n] == true)
                    return;
            }

            PlayableDirector pd = CutSceneCamera[1].GetComponent<PlayableDirector>();

            // メインカメラを終了させる
            MainCamera.SetActive(false);
            // CutScene , CutSceneCameraを起動する
            CutScene[1].SetActive(true);
            CutSceneCamera[1].SetActive(true);
            // 再生する
            pd.Play();
            isPlayingCutScene[1] = true;
            //Debug.Log("再生開始");
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
        MainCamera.SetActive(false);
        // CutScene , CutSceneCameraを起動する
        CutScene[SceneNo].SetActive(true);
        CutSceneCamera[SceneNo].SetActive(true);
        // 再生する
        pd.Play();
        isPlayingCutScene[SceneNo] = true;
    }
}
