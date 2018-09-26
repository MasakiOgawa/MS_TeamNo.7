using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScene : MonoBehaviour
{
    [SerializeField]
    private float FadeTime;     // フェードの時間

    [SerializeField]
    private string SceneName;   // シーン遷移させたいシーンの名前

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // エンターを押してシーン遷移
        if (Input.GetKeyDown(KeyCode.Return))
        {
            FadeManager.Instance.LoadScene(SceneName, FadeTime);
        }
    }
}
