using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CutScene0 : MonoBehaviour {


    private PostProcessingBehaviour behaviour;
    
    // Use this for initialization
    void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {

    }

    // timeline最初に呼び出す
    public void Init()
    {
        // postProcessingBehaviourを取得
        behaviour = GetComponent<PostProcessingBehaviour>();

        // postprocess情報を初期化
        Initialize();

    }

    private void Initialize()
    {
        // 初期化
        var Settings = behaviour.profile.bloom.settings;
        Settings.bloom.intensity = 0.65f;
        behaviour.profile.bloom.settings = Settings;
    }
}
