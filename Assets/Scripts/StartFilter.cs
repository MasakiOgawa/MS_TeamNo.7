using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartFilter : MonoBehaviour
{
    Image FilterImage;
    float fAlpha;
    public float fAddAlpha;


	void Start( )
    {
        fAlpha = 1.0f;
        FilterImage = this.GetComponent< Image >( );
	}


    public void AddAlpha( )
    {
        fAlpha -= fAddAlpha;
        FilterImage.color = new Color( 255.0f , 255.0f , 255.0f , fAlpha );

        if( fAlpha <= 0.0f )
        {
            fAlpha = 0.0f;
            FilterImage.color = new Color( 255.0f , 255.0f , 255.0f , 0.0f );
        }
    }
}
