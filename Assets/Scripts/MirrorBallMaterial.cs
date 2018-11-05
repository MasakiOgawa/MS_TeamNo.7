using System.Collections;
using UnityEngine;


public class MirrorBallMaterial : MonoBehaviour
{
    public GameObject Obj;
    public GameObject Obj2;
    public GameObject Obj3;

    Renderer ObjRenderer;
    Renderer Obj2Renderer;
    Renderer Obj3Renderer;

    //色が変わるタイミング(時間)
    public float duration;

    void Start( )
    {
        ObjRenderer = Obj.GetComponent<Renderer>();
        Obj2Renderer = Obj2.GetComponent<Renderer>();
        Obj3Renderer = Obj3.GetComponent<Renderer>();
    }


    public void SetColor( int nCountDown )
    {
        if( nCountDown == 3 )
        {
            ObjRenderer.material.color = Color.red;
            Obj2Renderer.material.color = Color.red;
            Obj3Renderer.material.color = Color.red;
        }
        else if( nCountDown == 2 )
        {
            ObjRenderer.material.color = Color.yellow;
            Obj2Renderer.material.color = Color.yellow;
            Obj3Renderer.material.color = Color.yellow;
        }
        else if( nCountDown == 1 )
        {
            ObjRenderer.material.color = Color.blue;
            Obj2Renderer.material.color = Color.blue;
            Obj3Renderer.material.color = Color.blue;
        }
        else if( nCountDown == 0 )
        {
            ObjRenderer.material.color = Color.green;
            Obj2Renderer.material.color = Color.green;
            Obj3Renderer.material.color = Color.green;
        }
        else if( nCountDown == 4 )
        {
            ObjRenderer.material.color = Color.white;
            Obj2Renderer.material.color = Color.white;
            Obj3Renderer.material.color = Color.white;
        }
    }


    public void BonusMaterial( )
    {
        //durationの時間ごとに色が変わる
        float phi = Time.time / duration * 2 * Mathf.PI;
        float amplitude = Mathf.Cos(phi) * 0.5F + 0.5F;

        //色をRGBではなくHSVで指定
        ObjRenderer.material.color = Color.HSVToRGB(amplitude, 1, 1);
        Obj2Renderer.material.color = Color.HSVToRGB(amplitude, 1, 1);
        Obj3Renderer.material.color = Color.HSVToRGB(amplitude, 1, 1);
	}
}
