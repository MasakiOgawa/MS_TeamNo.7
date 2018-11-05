using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResultBG : MonoBehaviour {

    // 0から1へ向かう
    [SerializeField] private float deltaAlpha;
    [SerializeField] private float TargetAlpha;
    private bool bRunning;

	// Use this for initialization
	void Start () {
        bRunning = false;

    }
	
	// Update is called once per frame
	void Update () {
		
        if ( bRunning == true)
        {
            // 現在のalphaを取得
            float fAlpha = GetComponent<Image>().color.a;

            Debug.Log(fAlpha);

            fAlpha += deltaAlpha;

            // 暗くなりすぎた場合
            if ( fAlpha > TargetAlpha )
            {
                fAlpha = TargetAlpha;
                bRunning = false;
            }
            GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b,
                fAlpha);

        }
	}

    // 起動
    public void StartBG ()
    {
        bRunning = true;

    }
}
