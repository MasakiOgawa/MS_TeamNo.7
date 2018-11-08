using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeParticleSystem : MonoBehaviour {

    public enum TYPE_COLOR
    {
        TYPE_BLUE,
        TYPE_GREEN,
        TYPE_ORANGE,
    };

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetColor (TYPE_COLOR type)
    {
        if ( type == TYPE_COLOR.TYPE_BLUE)
        {
            ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
            color.mode = ParticleSystemGradientMode.TwoColors;
            color.colorMax = new Color((float)0 / (float)255, (float)49 / (float)255, (float)255 / (float)255, (float)33 / (float)255);
            color.colorMin = new Color((float)64 / (float)255, (float)143 / (float)255, (float)255 / (float)255, (float)34 / (float)255);

            ParticleSystem particleSystem = GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = particleSystem.main;
            main.startColor = color;
        }

        else if ( type == TYPE_COLOR.TYPE_GREEN )
        {
            ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
            color.mode = ParticleSystemGradientMode.TwoColors;
            color.colorMax = new Color((float)18 / (float)255, (float)255 / (float)255, (float)0 / (float)255, (float)33 / (float)255);
            color.colorMin = new Color((float)32 / (float)255, (float)255 / (float)255, (float)106 / (float)255, (float)34 / (float)255);

            ParticleSystem particleSystem = GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = particleSystem.main;
            main.startColor = color;
        }

        else if ( type == TYPE_COLOR.TYPE_ORANGE)
        {
            ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
            color.mode = ParticleSystemGradientMode.TwoColors;
            color.colorMax = new Color((float)255 / (float)255, (float)144 / (float)255, (float)0 / (float)255, (float)33 / (float)255);
            color.colorMin = new Color((float)255 / (float)255, (float)170 / (float)255, (float)49 / (float)255, (float)34 / (float)255);

            ParticleSystem particleSystem = GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = particleSystem.main;
            main.startColor = color;
        }

    }
}
