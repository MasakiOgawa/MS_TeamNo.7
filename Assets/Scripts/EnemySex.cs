using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySex : MonoBehaviour {

    public enum EnemySexType
    { 
        TYPE_MAN = 0 ,
        TYPE_WOMAN
    };

    public EnemySexType SexType;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public EnemySexType GetSexType( )
    {
        return SexType;
    }
}
