using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FontController : MonoBehaviour {

    [SerializeField] RectTransform rect;
    [SerializeField] Material AlphaMaterial;
    [SerializeField] private float DeltaAlpha;
    Tweener tweener;            // トゥイーンの情報

    bool bRunComplete;
    bool bComplete;
    float alpha;

    public enum FONT_TYPE
    {
        FONT_BAD,
        FONT_FINE,
        FONT_EXCELLENT,
    };
  
	// Use this for initialization
	void Start () {
        bRunComplete = false;
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

    private void Set ( Vector2 pos )
    {
        rect.anchoredPosition = new Vector3(pos.x, pos.y, 0);

        tweener = rect.DOScale(0.2f, 0.2f).OnComplete(() => bRunComplete=true);

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
            default:
                {
                    prefab = (GameObject)Resources.Load("Prefabs/Font/Font_Excellent");
                    break;
                }


        }
        // プレハブからインスタンスを生成
        GameObject obj = Instantiate(prefab );
        obj.transform.SetParent(canvas.transform, false);
        obj.GetComponent<FontController>().Set(pos);
 
    }
}
