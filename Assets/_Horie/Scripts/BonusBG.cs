using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BonusBG : MonoBehaviour {

    [SerializeField] private GameObject BonusTutorialObj;

    [SerializeField] private bool isFrame;
    [SerializeField] private RectTransform rect;

    private bool bRunComplete;
    Tweener tweener;            // トゥイーンの情報

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (bRunComplete == true)
        {
            //BonusTutorialObj.GetComponent<BonusTutorial>().ReturnBG(isFrame);
        }

	}

    public void Run ()
    {
        bRunComplete = false;
        tweener = rect.DOScale(1.0f, 0.1f).OnComplete(() => BonusTutorialObj.GetComponent<BonusTutorial>().ReturnBG(isFrame));
    }

    public void ShutDown()
    {
        if ( isFrame )
        tweener = rect.DOScale(0.0f, 0.2f).OnComplete(() => BonusTutorialObj.GetComponent<BonusTutorial>().ShutDownBG());
    }
}
