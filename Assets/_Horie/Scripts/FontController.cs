using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FontController : MonoBehaviour {

    // Excellent -> 星も出す
    // Miss -> 出現後ガクって右回転
    // Bad -> 


    [SerializeField] RectTransform rect;
    [SerializeField] Material AlphaMaterial;
    [SerializeField] private float DeltaAlpha;
    Tweener tweener;            // トゥイーンの情報

    bool bRunComplete;
    bool bComplete;
    public float alpha;

    public enum FONT_TYPE
    {
        FONT_BAD,
        FONT_FINE,
        FONT_EXCELLENT,
        FONT_MISS,
    };
  
	// Use this for initialization
	void Start () {
       // bRunComplete = false;
        bComplete = false;
        alpha = 1.0f;   
    }
	
	// Update is called once per frame
	void Update () {
        
        // 1回のみ通貨
        if ( bRunComplete == true)
        {
            bRunComplete = false;
            bComplete = true;
        }

        if ( bComplete == true )
        {
            alpha -= DeltaAlpha;
            if ( alpha < 0.0f )
            {
               
                Destroy(this.gameObject);
            }
        }
    }

    public void Set ( Vector2 pos , FONT_TYPE type )
    {

        rect.anchoredPosition = new Vector3(pos.x, pos.y, 0);

        // エクセレントのとき
        if (type == FONT_TYPE.FONT_EXCELLENT)
            tweener = rect.DOScale(0.25f, 0.2f).OnComplete(() => bRunComplete = true);
        // 
        else if (type == FONT_TYPE.FONT_FINE)
        {
            tweener = rect.DOScale(0.3f, 0.2f).OnComplete(() => bRunComplete = true);
            tweener = rect.DORotate(new Vector3(0, 0, -90), 0.2f);
        }
        else if (type == FONT_TYPE.FONT_BAD)
            tweener = rect.DOScale(0.3f, 0.2f).OnComplete(() => bRunComplete = true);
        else if (type == FONT_TYPE.FONT_MISS)
            tweener = rect.DOScale(0.3f, 0.1f).OnComplete(() =>
            tweener = rect.DORotate(new Vector3(0, 0, -10), 0.1f).OnComplete(() =>
         tweener = rect.DOScale(0.3f, 0.1f).OnComplete(() => bRunComplete = true)));
    }


    // 生成
    static public void Create ( GameObject canvas , FONT_TYPE type , Vector2 pos )
    {
        GameObject prefab;
        switch (type)
        {
            case FONT_TYPE.FONT_BAD:
                {
                    // プレハブを取得
                    prefab = (GameObject)Resources.Load("Prefabs/Font/Font_Bad");
                    break;
                }
            case FONT_TYPE.FONT_FINE:
                {
                    prefab = (GameObject)Resources.Load("Prefabs/Font/Font_Fine");
                    break;
                }
            case FONT_TYPE.FONT_EXCELLENT:
                {
                    prefab = (GameObject)Resources.Load("Prefabs/Font/Font_Excellent");
                    break;
                }
            case FONT_TYPE.FONT_MISS:
                {
                    prefab = (GameObject)Resources.Load("Prefabs/Font/Font_Miss");
                    break;
                }
            default:
                {
                    prefab = (GameObject)Resources.Load("Prefabs/Font/Font_Excellent");
                    break;
                }


        }
        // プレハブからインスタンスを生成
        GameObject obj = Instantiate( prefab );
        obj.transform.SetParent( canvas.transform, false );
        obj.GetComponent<FontController>().Set(pos , type);
 
    }
}
