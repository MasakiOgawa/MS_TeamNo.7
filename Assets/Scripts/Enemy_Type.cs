using System.Collections;
using UnityEngine;


public class Enemy_Type : MonoBehaviour
{
    public enum EnemySex
    {
        ENEMY_MAN = 0 ,
        ENEMY_WOMAN
    };

    public EnemySex EnemySexType;


	void Start( )
    {
		
	}


	void Update( )
    {
		
	}


    public EnemySex GetEnemySex( )
    {
        return EnemySexType;
    }
  
}
