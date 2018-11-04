using System.Collections;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    Vector3 Vec;
    float   fMove;

    void Start( )
    {
        
    }


	void Update( )
    {
		this.transform.position += new Vector3( Vec.x * fMove , Vec.y * fMove , Vec.z * fMove );
	}


    public void SetState( Vector3 Dist )
    {
       // Vec     = 
        Vec     = new Vector3( Dist.x , Dist.y , Dist.z );
        Vec     = Vec.normalized;
        fMove   = Vec.magnitude / 0.4475f;
    }
}
