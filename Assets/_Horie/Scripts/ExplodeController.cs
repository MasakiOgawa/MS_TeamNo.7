using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeController : MonoBehaviour {

    // 球の種類列挙
    public enum EXPLODE_TYPE
    {
        TYPE_BAD,
        TYPE_FINE,
        TYPE_EXCELLENT,
        TYPE_BONUS,
    };

    private EXPLODE_TYPE m_explodeType;


    // 移動速度
    [SerializeField] private GameObject ParticleSystem2;
    [SerializeField] private GameObject PointLight;

    [SerializeField] private float m_SourceToMirrorBallMoveSpeed;
    [SerializeField] private float m_MirrorBallToTargetMoveSpeed;

    [SerializeField] private float SelfKillSize;

    public GameObject m_TargetObj;
    public float m_TargetOffsetY;

    public Vector3 SourcePos;      // 発射プレイヤー座標
    public Vector3 TargetPos;      // 敵座標
    public Vector3 MirrorBallPos;  // ミラーボール座標

    public Vector3 SourceToMirrorBallMoveVector;
    public Vector3 MirrorBallToTargetMoveVector;

    private enum EXPLODE_PHASE
    {
        PHASE_SOURCE_TO_MIRRORBALL,
        PHASE_MIRRORBALL_TO_TARGET,
    };

    private EXPLODE_PHASE explodePhase;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // プレイヤーからミラーボールへ発射
        if (explodePhase == EXPLODE_PHASE.PHASE_SOURCE_TO_MIRRORBALL)
        {
            // 現在のposにMoveVecを足す
            this.transform.position = new Vector3(transform.position.x + SourceToMirrorBallMoveVector.x,
                transform.position.y + SourceToMirrorBallMoveVector.y,
                transform.position.z + SourceToMirrorBallMoveVector.z);
            // ミラーボールに到達したらフェーズ変更
            if (( MirrorBallPos.x - SelfKillSize <= transform.position.x &&
                transform.position.x <= MirrorBallPos.x + SelfKillSize &&

                MirrorBallPos.y - SelfKillSize <= transform.position.y &&

                MirrorBallPos.z - SelfKillSize <= transform.position.z &&
                transform.position.z <= MirrorBallPos.z + SelfKillSize ) ||

                MirrorBallPos.y - SelfKillSize <= transform.position.y)
            {
                // フェーズ変更
                explodePhase = EXPLODE_PHASE.PHASE_MIRRORBALL_TO_TARGET;
            }
        }
        // ミラーボールから敵へ移動
        else if (explodePhase == EXPLODE_PHASE.PHASE_MIRRORBALL_TO_TARGET)
        {
            // 現在のposにMoveVecを足す
            this.transform.position = new Vector3(transform.position.x + MirrorBallToTargetMoveVector.x,
                transform.position.y + MirrorBallToTargetMoveVector.y,
                transform.position.z + MirrorBallToTargetMoveVector.z);
            // 敵に到達
            if ( ( TargetPos.x - SelfKillSize <= transform.position.x &&
                transform.position.x <= TargetPos.x + SelfKillSize &&
                TargetPos.z - SelfKillSize <= transform.position.z &&
                transform.position.z <= TargetPos.z + SelfKillSize ) ||

                TargetPos.z - SelfKillSize <= transform.position.z)
            {
                // 爆発
                HitController.Create(m_TargetObj, m_TargetOffsetY, -1);
                // 当たったので消滅
                Destroy(this.gameObject);
            }
        }



    }

    public void StartFire ( GameObject Source , float SourceY , GameObject Target , float TargetY ,
        GameObject MirrorBall , float MirrorBallY , ExplodeController.EXPLODE_TYPE exType , float SourceToMirrorBallMoveSpeed,
        float MirrorBallToTargetMoveSpeed)
    {
        // 弾情報
        m_explodeType = exType;

        // ターゲット情報
        m_TargetObj = Target;
        m_TargetOffsetY = TargetY;

        // 発射座標の設定
        SourcePos = new Vector3(Source.transform.position.x,
            Source.transform.position.y + SourceY,
            Source.transform.position.z);

        // 目的位置の設定
        TargetPos = new Vector3(Target.transform.position.x,
            Target.transform.position.y + TargetY,
            Target.transform.position.z);

        // ミラーボール座標の設定
        MirrorBallPos = new Vector3(MirrorBall.transform.position.x,
            MirrorBall.transform.position.y + MirrorBallY,
            MirrorBall.transform.position.z);

        // 速度設定
        m_SourceToMirrorBallMoveSpeed = SourceToMirrorBallMoveSpeed;
        m_MirrorBallToTargetMoveSpeed = MirrorBallToTargetMoveSpeed;

        // sourceToMirrorBallベクトル算出
        SourceToMirrorBallMoveVector = MirrorBallPos - SourcePos;
        // 正規化
        SourceToMirrorBallMoveVector = SourceToMirrorBallMoveVector.normalized;
        // 移動ベクトルに指定した移動量をかける
        SourceToMirrorBallMoveVector *= m_SourceToMirrorBallMoveSpeed;

        // MirrorBallToTargetベクトル算出
        MirrorBallToTargetMoveVector = TargetPos - MirrorBallPos;
        // 正規化
        MirrorBallToTargetMoveVector = MirrorBallToTargetMoveVector.normalized;
        // 移動ベクトルに指定した移動量をかける
        MirrorBallToTargetMoveVector *= m_MirrorBallToTargetMoveSpeed;

        // フェーズ設定
        explodePhase = EXPLODE_PHASE.PHASE_SOURCE_TO_MIRRORBALL;

        // Typeに応じてパーティクルの色を変更
        if ( exType == EXPLODE_TYPE.TYPE_BAD)
        {
            ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
            color.mode = ParticleSystemGradientMode.TwoColors;

            color.colorMax = new Color((float)94 / (float)255, (float)128 / (float)255, (float)226 / (float)255, (float)136 / (float)255);
            color.colorMin = new Color((float)213 / (float)255, (float)215 / (float)255, (float)246 / (float)255, (float)138 / (float)255);


            ParticleSystem particleSystem = GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = particleSystem.main;
            main.startColor = color;
            //
            //ParticleSystem.
            ParticleSystem2.GetComponent<ExplodeParticleSystem>().SetColor(ExplodeParticleSystem.TYPE_COLOR.TYPE_BLUE);

            // point Light
            PointLight.GetComponent<ExplodePointLight>().SetColor(ExplodePointLight.LIGHT_TYPE.TYPE_BLUE);
        }
        else if ( exType == EXPLODE_TYPE.TYPE_FINE)
        {
            ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
            color.mode = ParticleSystemGradientMode.TwoColors;
            // みどり
            color.colorMax = new Color((float)0 / (float)255, (float)15 / (float)255, (float)15 / (float)255, (float)136 / (float)255);
            color.colorMin = new Color((float)160 / (float)255, (float)255 / (float)255, (float)0 / (float)255, (float)138 / (float)255);


            ParticleSystem particleSystem = GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = particleSystem.main;
            main.startColor = color;
            //
            //ParticleSystem.
            ParticleSystem2.GetComponent<ExplodeParticleSystem>().SetColor(ExplodeParticleSystem.TYPE_COLOR.TYPE_GREEN);

            // point Light
            PointLight.GetComponent<ExplodePointLight>().SetColor(ExplodePointLight.LIGHT_TYPE.TYPE_GREEN);
        }
        else if ( exType == EXPLODE_TYPE.TYPE_EXCELLENT)
        {
            ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
            color.mode = ParticleSystemGradientMode.TwoColors;
            // オレンジ
            color.colorMax = new Color((float)255 / (float)255, (float)192 / (float)255, (float)0 / (float)255, (float)136 / (float)255);
            color.colorMin = new Color((float)255 / (float)255, (float)177 / (float)255, (float)0 / (float)255, (float)138 / (float)255);


            ParticleSystem particleSystem = GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = particleSystem.main;
            main.startColor = color;
            //
            //ParticleSystem.
            ParticleSystem2.GetComponent<ExplodeParticleSystem>().SetColor(ExplodeParticleSystem.TYPE_COLOR.TYPE_ORANGE);

            // point Light
            PointLight.GetComponent<ExplodePointLight>().SetColor(ExplodePointLight.LIGHT_TYPE.TYPE_ORANGE);
        }
    }

    static public void Create(GameObject Source, float SourceY, GameObject Target, float TargetY , 
        GameObject MirrorBall , float MirrorBallY, ExplodeController.EXPLODE_TYPE exType , float SourceToMirrorBallMoveSpeed,
        float MirrorBallToTargetMoveSpeed)
    {
        // プレハブを取得
        GameObject prefab = (GameObject)Resources.Load("Prefabs/Explode");
        // プレハブからインスタンスを生成
        GameObject obj = Instantiate(prefab);
        //  
        obj.GetComponent<ExplodeController>().StartFire(Source, SourceY, Target, TargetY ,
            MirrorBall , MirrorBallY, exType , SourceToMirrorBallMoveSpeed , MirrorBallToTargetMoveSpeed);
    }
}
