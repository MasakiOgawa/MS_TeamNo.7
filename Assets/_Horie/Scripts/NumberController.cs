using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NumberController : MonoBehaviour {

    // 表示したい数字を受け取ってそのテクスチャを表示するだけ

    [SerializeField] private Sprite[] spriteObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetSprite ( int nNumber , GameObject TargetSpriteObj )
    {
        TargetSpriteObj.GetComponent<Image>().sprite = spriteObj[nNumber];

    }
}
