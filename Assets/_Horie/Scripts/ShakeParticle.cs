using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeParticle : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 使っていないとき
        if (GetComponent<ParticleSystem>().isPlaying == false)
        {
            // 停止させる
            GetComponent<ParticleSystem>().Stop();
        }
    }

    public void PlayParticle(Vector3 pos)
    {
        // 再生座標設定
        transform.position = pos;
        // 再生
        GetComponent<ParticleSystem>().Play();
    }
}
