﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class Font_Fine : MonoBehaviour {

    [SerializeField] private RectTransform rect;
    [SerializeField] private GameObject ParentFont;

    public Tweener tweener;            // トゥイーンの情報

    private bool end;

    public enum FINE_TYPE
    {
        G,
        o_R,
        o_L,
        d,
    };

    public enum BAD_TYPE
    {
        B,a,d,
    };

    public enum STAR_TYPE
    {
        FONT,
        L,
        R_UP,
        R_DOWN,
    }

    [SerializeField] private FINE_TYPE type;
    [SerializeField] private BAD_TYPE badType;
    [SerializeField] private STAR_TYPE starType;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (end == true)
            ParentFont.GetComponent<FontController>().returnFontFine(0);
	}

    public void Move ()
    {
        float sclSpeed = 0.1f;
        float upY = 7.5f;


        Vector3 work = transform.position;
        switch (type)
        {
            case FINE_TYPE.G:
                {
                    end = false;
                    
                    tweener = rect.DOScale(0.3f, sclSpeed).OnComplete(() =>
                    tweener = rect.DOMove(work + new Vector3 (0, upY, 0), 0.1f).OnComplete(()=>
                    tweener = rect.DOMove(work - new Vector3(0, upY, 0), 0.1f)
                    ));

                    break;
                }
            case FINE_TYPE.o_L:
                {
                    tweener = rect.DOScale(0.3f, sclSpeed).OnComplete(() =>
                    tweener = rect.DOScale(0.3f, 0.05f).OnComplete(() =>
                    tweener = rect.DOMove(work + new Vector3(0, upY, 0), 0.1f).OnComplete(() =>
                     tweener = rect.DOMove(work - new Vector3(0, upY, 0), 0.1f)
                    )));
                    break;
                }
            case FINE_TYPE.o_R:
                {
                    tweener = rect.DOScale(0.3f, sclSpeed).OnComplete(() =>
                    tweener = rect.DOScale(0.3f, 0.1f).OnComplete(() =>
                    tweener = rect.DOMove(work + new Vector3(0, upY, 0), 0.1f).OnComplete(() =>
                     tweener = rect.DOMove(work - new Vector3(0, upY, 0), 0.1f)
                    )));
                    break;
                }
            case FINE_TYPE.d:
                {
                    tweener = rect.DOScale(0.3f, sclSpeed).OnComplete(() =>
                    tweener = rect.DOScale(0.3f, 0.15f).OnComplete(() =>
                    tweener = rect.DOMove(work + new Vector3(0, upY, 0), 0.1f).OnComplete(() =>
                        tweener = rect.DOMove(work - new Vector3(0, upY, 0), 0.1f).OnComplete(()=>
                        end = true
                    ))));
                    break;
                }
            default:
                break;


        }


    }

    public void BadMove()
    {
        float sclSpeed = 0.1f;
        float upY = 7.5f;

        float rotateTime = 0.1f;

        Vector3 work = transform.position;
        switch (badType)
        {
            case BAD_TYPE.B:
                {
                    end = false;

                    tweener = rect.DOScale(0.3f, sclSpeed).OnComplete(() =>
                    tweener = rect.DOScale(0.3f, 0.15f).OnComplete(() =>
                    tweener = rect.DORotate(new Vector3(0, 0, 20), rotateTime
                    )));

                    break;
                }
            case BAD_TYPE.a:
                {
                    tweener = rect.DOScale(0.3f, sclSpeed).OnComplete(() =>
                    tweener = rect.DOScale(0.3f, 0.25f).OnComplete(() =>
                    tweener = rect.DORotate(new Vector3(0, 0, 5), rotateTime
                    ).OnComplete(()=> end = true)));
                    break;
                }
            case BAD_TYPE.d:
                {
                    tweener = rect.DOScale(0.3f, sclSpeed).OnComplete(() =>
                    tweener = rect.DORotate(new Vector3(0, 0, -30), rotateTime));
                    break;
                }

            default:
                break;


        }


    }

    public void StarMove()
    {
        float sclSpeed = 0.1f;
        float upY = 7.5f;

        float rotateTime = 1.0f;
        float rotateZ = 720.0f;

        Vector3 work = transform.position;
        switch (starType)
        {
            case STAR_TYPE.FONT:
                {
                    end = false;
                    //tweener = rect.DOScale(0.3f, sclSpeed);

                    tweener = rect.DOScale(0.3f, sclSpeed).OnComplete(() =>
                    tweener = rect.DOScale(0.3f, 0.5f).OnComplete(() => end = true));
                    break;
                }
            case STAR_TYPE.L:
                {
                    // move
                    //tweener = rect.DOMove(work + new Vector3(-200, 10, 0), 0.1f);

                    tweener = rect.DOScale(0.15f, sclSpeed).OnComplete(() =>
                    tweener = rect.DORotate(new Vector3(0, 0, -rotateZ), rotateTime));

                    break;
                }
            case STAR_TYPE.R_UP:
                {
                    //tweener = rect.DOScale(0.3f, sclSpeed).OnComplete(() =>
                    //tweener = rect.DORotate(new Vector3(0, 0, rotateZ), rotateTime
                    //).OnComplete(() => end = true));

                    tweener = rect.DOScale(0.15f, sclSpeed).OnComplete(() =>
                    tweener = rect.DORotate(new Vector3(0, 0, -rotateZ ), rotateTime));
                    break;
                }
            case STAR_TYPE.R_DOWN:
                {
                    tweener = rect.DOScale(0.15f, sclSpeed).OnComplete(() =>
                    tweener = rect.DORotate(new Vector3(0, 0, -rotateZ), rotateTime));
                    break;
                }

            default:
                break;


        }


    }

}
