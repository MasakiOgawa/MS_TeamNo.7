using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounusAreaController : MonoBehaviour {

    [SerializeField] private GameObject Vertical;
    [SerializeField] private GameObject GlowBits;
    [SerializeField] private GameObject BigGlow;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeColor ( bool bGreen )
    {
        ChangeColorThis(bGreen);
        ChangeColorVertical(bGreen);
        ChangeColorGlowBits(bGreen);
        ChangeColorBigGlow(bGreen);

    }

    // 自身の色変更
    private void ChangeColorThis(bool bGreen)
    {
        Color col;
        if (bGreen == true)
            col = Color.green;
        else
            col = Color.yellow;
        // ――――――――――――――――――――――――――――――――――――
        //
        // ――――――――――――――――――――――――――――――――――――
        //Create Gradient key Min
        GradientColorKey[] gradientColorKeyMin;
        gradientColorKeyMin = new GradientColorKey[2];
        gradientColorKeyMin[0].color = col;
        gradientColorKeyMin[0].time = 0f;
        gradientColorKeyMin[1].color = col;
        gradientColorKeyMin[1].time = 1f;

        //Create Gradient alpha Min
        GradientAlphaKey[] gradientAlphaKeyMin;
        gradientAlphaKeyMin = new GradientAlphaKey[4];
        gradientAlphaKeyMin[0].alpha = 0.0f;
        gradientAlphaKeyMin[0].time = 0.0f;
        gradientAlphaKeyMin[1].alpha = (float)255.0f / (float)255.0f;
        gradientAlphaKeyMin[1].time = 0.305f;
        gradientAlphaKeyMin[2].alpha = 173.0f / 255.0f;
        gradientAlphaKeyMin[2].time = 0.702f;
        gradientAlphaKeyMin[3].alpha = 0f;
        gradientAlphaKeyMin[3].time = 1f;

        //Create Gradient key Max
        GradientColorKey[] gradientColorKeyMax;
        gradientColorKeyMax = new GradientColorKey[2];
        gradientColorKeyMax[0].color = col;
        gradientColorKeyMax[0].time = 0f;
        gradientColorKeyMax[1].color = col;
        gradientColorKeyMax[1].time = 1f;

        //Create Gradient alpha Max
        GradientAlphaKey[] gradientAlphaKeyMax;
        gradientAlphaKeyMax = new GradientAlphaKey[4];
        gradientAlphaKeyMax[0].alpha = 0.0f;
        gradientAlphaKeyMax[0].time = 0.0f;
        gradientAlphaKeyMax[1].alpha = (float)255.0f / (float)255.0f;
        gradientAlphaKeyMax[1].time = 0.305f;
        gradientAlphaKeyMax[2].alpha = 173.0f / 255.0f;
        gradientAlphaKeyMax[2].time = 0.702f;
        gradientAlphaKeyMax[3].alpha = 0f;
        gradientAlphaKeyMax[3].time = 1f;

        //Create Gradient Min
        Gradient gradientMin = new Gradient();
        gradientMin.SetKeys(gradientColorKeyMin, gradientAlphaKeyMin);

        //Create Gradient Max
        Gradient gradientMax = new Gradient();
        gradientMax.SetKeys(gradientColorKeyMax, gradientAlphaKeyMax);


        //Create Color from Gradient
        ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
        color.mode = ParticleSystemGradientMode.TwoGradients;
        color.gradientMin = gradientMin;
        color.gradientMax = gradientMax;

        //Assign the color to the particle
        ParticleSystem.ColorOverLifetimeModule main = GetComponent<ParticleSystem>().colorOverLifetime;
        main.color = new ParticleSystem.MinMaxGradient(gradientMin , gradientMax);
        //main.startColor = color;
    }

    // Vertical色変更
    private void ChangeColorVertical(bool bGreen)
    {
        Color col;
        if (bGreen == true)
            col = Color.green;
        else
            col = Color.yellow;
        // ――――――――――――――――――――――――――――――――――――
        //
        // ――――――――――――――――――――――――――――――――――――
        //Create Gradient key Min
        GradientColorKey[] gradientColorKeyMin;
        gradientColorKeyMin = new GradientColorKey[2];
        gradientColorKeyMin[0].color = col;
        gradientColorKeyMin[0].time = 0f;
        gradientColorKeyMin[1].color = col;
        gradientColorKeyMin[1].time = 1f;

        //Create Gradient alpha Min
        GradientAlphaKey[] gradientAlphaKeyMin;
        gradientAlphaKeyMin = new GradientAlphaKey[4];
        gradientAlphaKeyMin[0].alpha = 0.0f;
        gradientAlphaKeyMin[0].time = 0.0f;
        gradientAlphaKeyMin[1].alpha = (float)255.0f / (float)255.0f;
        gradientAlphaKeyMin[1].time = 0.305f;
        gradientAlphaKeyMin[2].alpha = 173.0f / 255.0f;
        gradientAlphaKeyMin[2].time = 0.702f;
        gradientAlphaKeyMin[3].alpha = 0f;
        gradientAlphaKeyMin[3].time = 1f;

        //Create Gradient key Max
        GradientColorKey[] gradientColorKeyMax;
        gradientColorKeyMax = new GradientColorKey[2];
        gradientColorKeyMax[0].color = col;
        gradientColorKeyMax[0].time = 0f;
        gradientColorKeyMax[1].color = col;
        gradientColorKeyMax[1].time = 1f;

        //Create Gradient alpha Max
        GradientAlphaKey[] gradientAlphaKeyMax;
        gradientAlphaKeyMax = new GradientAlphaKey[4];
        gradientAlphaKeyMax[0].alpha = 0.0f;
        gradientAlphaKeyMax[0].time = 0.0f;
        gradientAlphaKeyMax[1].alpha = (float)255.0f / (float)255.0f;
        gradientAlphaKeyMax[1].time = 0.305f;
        gradientAlphaKeyMax[2].alpha = 173.0f / 255.0f;
        gradientAlphaKeyMax[2].time = 0.702f;
        gradientAlphaKeyMax[3].alpha = 0f;
        gradientAlphaKeyMax[3].time = 1f;

        //Create Gradient Min
        Gradient gradientMin = new Gradient();
        gradientMin.SetKeys(gradientColorKeyMin, gradientAlphaKeyMin);

        //Create Gradient Max
        Gradient gradientMax = new Gradient();
        gradientMax.SetKeys(gradientColorKeyMax, gradientAlphaKeyMax);


        //Create Color from Gradient
        ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
        color.mode = ParticleSystemGradientMode.TwoGradients;
        color.gradientMin = gradientMin;
        color.gradientMax = gradientMax;

        //Assign the color to the particle
        ParticleSystem.ColorOverLifetimeModule main = Vertical.GetComponent<ParticleSystem>().colorOverLifetime;
        main.color = new ParticleSystem.MinMaxGradient(gradientMin, gradientMax);
        //main.startColor = color;
    }

    // GlowBits色変更
    private void ChangeColorGlowBits(bool bGreen)
    {
        Color col;
        if (bGreen == true)
            col = Color.green;
        else
            col = Color.yellow;
        // ――――――――――――――――――――――――――――――――――――
        //
        // ――――――――――――――――――――――――――――――――――――
        //Create Gradient key Min
        GradientColorKey[] gradientColorKeyMin;
        gradientColorKeyMin = new GradientColorKey[2];
        gradientColorKeyMin[0].color = col;
        gradientColorKeyMin[0].time = 0f;
        gradientColorKeyMin[1].color = col;
        gradientColorKeyMin[1].time = 1f;

        //Create Gradient alpha Min
        GradientAlphaKey[] gradientAlphaKeyMin;
        gradientAlphaKeyMin = new GradientAlphaKey[4];
        gradientAlphaKeyMin[0].alpha = 0.0f;
        gradientAlphaKeyMin[0].time = 0.0f;
        gradientAlphaKeyMin[1].alpha = (float)255.0f / (float)255.0f;
        gradientAlphaKeyMin[1].time = 0.305f;
        gradientAlphaKeyMin[2].alpha = 173.0f / 255.0f;
        gradientAlphaKeyMin[2].time = 0.702f;
        gradientAlphaKeyMin[3].alpha = 0f;
        gradientAlphaKeyMin[3].time = 1f;

        //Create Gradient key Max
        GradientColorKey[] gradientColorKeyMax;
        gradientColorKeyMax = new GradientColorKey[2];
        gradientColorKeyMax[0].color = col;
        gradientColorKeyMax[0].time = 0f;
        gradientColorKeyMax[1].color = col;
        gradientColorKeyMax[1].time = 1f;

        //Create Gradient alpha Max
        GradientAlphaKey[] gradientAlphaKeyMax;
        gradientAlphaKeyMax = new GradientAlphaKey[4];
        gradientAlphaKeyMax[0].alpha = 0.0f;
        gradientAlphaKeyMax[0].time = 0.0f;
        gradientAlphaKeyMax[1].alpha = (float)255.0f / (float)255.0f;
        gradientAlphaKeyMax[1].time = 0.305f;
        gradientAlphaKeyMax[2].alpha = 173.0f / 255.0f;
        gradientAlphaKeyMax[2].time = 0.702f;
        gradientAlphaKeyMax[3].alpha = 0f;
        gradientAlphaKeyMax[3].time = 1f;

        //Create Gradient Min
        Gradient gradientMin = new Gradient();
        gradientMin.SetKeys(gradientColorKeyMin, gradientAlphaKeyMin);

        //Create Gradient Max
        Gradient gradientMax = new Gradient();
        gradientMax.SetKeys(gradientColorKeyMax, gradientAlphaKeyMax);


        //Create Color from Gradient
        ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
        color.mode = ParticleSystemGradientMode.TwoGradients;
        color.gradientMin = gradientMin;
        color.gradientMax = gradientMax;

        //Assign the color to the particle
        ParticleSystem.ColorOverLifetimeModule main = GlowBits.GetComponent<ParticleSystem>().colorOverLifetime;
        main.color = new ParticleSystem.MinMaxGradient(gradientMin, gradientMax);
        // main.startColor = color;
    }

    // BigGlow色変更
    private void ChangeColorBigGlow(bool bGreen)
    {
        Color col;
        if (bGreen == true)
            col = Color.green;
        else
            col = Color.yellow;
        // ――――――――――――――――――――――――――――――――――――
        //
        // ――――――――――――――――――――――――――――――――――――
        //Create Gradient key Min
        GradientColorKey[] gradientColorKeyMin;
        gradientColorKeyMin = new GradientColorKey[2];
        gradientColorKeyMin[0].color = col;
        gradientColorKeyMin[0].time = 0f;
        gradientColorKeyMin[1].color = col;
        gradientColorKeyMin[1].time = 1f;

        //Create Gradient alpha Min
        GradientAlphaKey[] gradientAlphaKeyMin;
        gradientAlphaKeyMin = new GradientAlphaKey[4];
        gradientAlphaKeyMin[0].alpha = 0.0f;
        gradientAlphaKeyMin[0].time = 0.0f;
        gradientAlphaKeyMin[1].alpha = (float)255.0f / (float)255.0f;
        gradientAlphaKeyMin[1].time = 0.305f;
        gradientAlphaKeyMin[2].alpha = 173.0f / 255.0f;
        gradientAlphaKeyMin[2].time = 0.702f;
        gradientAlphaKeyMin[3].alpha = 0f;
        gradientAlphaKeyMin[3].time = 1f;

        //Create Gradient key Max
        GradientColorKey[] gradientColorKeyMax;
        gradientColorKeyMax = new GradientColorKey[2];
        gradientColorKeyMax[0].color = col;
        gradientColorKeyMax[0].time = 0f;
        gradientColorKeyMax[1].color = col;
        gradientColorKeyMax[1].time = 1f;

        //Create Gradient alpha Max
        GradientAlphaKey[] gradientAlphaKeyMax;
        gradientAlphaKeyMax = new GradientAlphaKey[4];
        gradientAlphaKeyMax[0].alpha = 0.0f;
        gradientAlphaKeyMax[0].time = 0.0f;
        gradientAlphaKeyMax[1].alpha = (float)255.0f / (float)255.0f;
        gradientAlphaKeyMax[1].time = 0.305f;
        gradientAlphaKeyMax[2].alpha = 173.0f / 255.0f;
        gradientAlphaKeyMax[2].time = 0.702f;
        gradientAlphaKeyMax[3].alpha = 0f;
        gradientAlphaKeyMax[3].time = 1f;

        //Create Gradient Min
        Gradient gradientMin = new Gradient();
        gradientMin.SetKeys(gradientColorKeyMin, gradientAlphaKeyMin);

        //Create Gradient Max
        Gradient gradientMax = new Gradient();
        gradientMax.SetKeys(gradientColorKeyMax, gradientAlphaKeyMax);


        //Create Color from Gradient
        ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
        color.mode = ParticleSystemGradientMode.TwoGradients;
        color.gradientMin = gradientMin;
        color.gradientMax = gradientMax;

        //Assign the color to the particle
        ParticleSystem.ColorOverLifetimeModule main = BigGlow.GetComponent<ParticleSystem>().colorOverLifetime;
        main.color = new ParticleSystem.MinMaxGradient(gradientMin, gradientMax);
        //main.startColor = color;
    }
}
