using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleModelController : MonoBehaviour {

    [SerializeField] private GameObject[] PlayerModel;

    private int PlayAnimation;

	// Use this for initialization
	void Start () {
        PlayAnimation = Random.RandomRange(0, 6);

        switch (PlayAnimation)
        {
            case 0:
                {
                    PlayAnimation = (int)PlayerAnimDefine.Idx.HipHopDancing;
                    break;
                }
            case 1:
                {
                    PlayAnimation = (int)PlayerAnimDefine.Idx.HipHopDancing1;
                    break;
                }
            case 2:
                {
                    PlayAnimation = (int)PlayerAnimDefine.Idx.HipHopDancing2;
                    break;
                }
            case 3:
                {
                    PlayAnimation = (int)PlayerAnimDefine.Idx.HipHopDancing3;
                    break;
                }
            case 4:
                {
                    PlayAnimation = (int)PlayerAnimDefine.Idx.HipHopDancing4;
                    break;
                }
            case 5:
                {
                    PlayAnimation = (int)PlayerAnimDefine.Idx.HipHopDancing5;
                    break;
                }
            case 6:
                {
                    PlayAnimation = (int)PlayerAnimDefine.Idx.HeadSpinning;
                    break;
                }
        }


         // モデルの数だけ繰り返す
        for ( int n = 0; n < PlayerModel.Length; n ++ )
        {
            PlayerModel[n].GetComponent<PlayerAnim>().MotionChange( (PlayerAnimDefine.Idx) PlayAnimation);
        }


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
