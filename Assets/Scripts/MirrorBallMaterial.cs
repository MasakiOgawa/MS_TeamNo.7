using System.Collections;
using UnityEngine;


public class MirrorBallMaterial : MonoBehaviour
{
    public GameObject Obj;
    public GameObject Obj2;
    public GameObject Obj3;


    public void SetColor( int nCountDown )
    {
        if( nCountDown == 3 )
        {
            Obj.GetComponent<Renderer>().material.color = Color.red;
            Obj2.GetComponent<Renderer>().material.color = Color.red;
            Obj3.GetComponent<Renderer>().material.color = Color.red;
        }
        else if( nCountDown == 2 )
        {
            Obj.GetComponent<Renderer>().material.color = Color.yellow;
            Obj2.GetComponent<Renderer>().material.color = Color.yellow;
            Obj3.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if( nCountDown == 1 )
        {
            Obj.GetComponent<Renderer>().material.color = Color.blue;
            Obj2.GetComponent<Renderer>().material.color = Color.blue;
            Obj3.GetComponent<Renderer>().material.color = Color.blue;
        }
        else if( nCountDown == 0 )
        {
            Obj.GetComponent<Renderer>().material.color = Color.green;
            Obj2.GetComponent<Renderer>().material.color = Color.green;
            Obj3.GetComponent<Renderer>().material.color = Color.green;
        }
        else if( nCountDown == 4 )
        {
            Obj.GetComponent<Renderer>().material.color = Color.white;
            Obj2.GetComponent<Renderer>().material.color = Color.white;
            Obj3.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
