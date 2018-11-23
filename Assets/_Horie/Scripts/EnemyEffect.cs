using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffect : MonoBehaviour {

    private Vector3 firstPos;

    // 移動速度
    float fMoveSpeed;
    // 移動距離
    float fDistMove;
    // 移動方向
    bool m_isComing;

    // Use this for initialization
    void Start () {
        firstPos = transform.position;
       // transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    }
	
	// Update is called once per frame
	void Update () {
        // こっちにきている
        if (m_isComing == true)
        {
            // 手前に移動させる
            transform.position = new Vector3(transform.position.x, transform.position.y,
                transform.position.z + fMoveSpeed);
            if (firstPos.z + fDistMove > transform.position.z)
            {
                Destroy(this.gameObject);
            }
        }
        else if ( m_isComing == false)
        {
            // 奥に移動させる
            transform.position = new Vector3(transform.position.x, transform.position.y,
                transform.position.z + fMoveSpeed);

            if (firstPos.z + fDistMove < transform.position.z)
            {
                Destroy(this.gameObject);
            }
        }
	}

    public void SetSpeedAndDist ( float speed , float dist , bool isSuccess )
    {
        fMoveSpeed = speed;
        fDistMove = dist;
        m_isComing = isSuccess;

        if ( m_isComing == false)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }

    }

    
}
