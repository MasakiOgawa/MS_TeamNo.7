using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GPUParticleSystemBuffer
{
    #region Buffers
    //The Buffers are formated in the following way:
    //New particle buffer   (R) = New (0) / Not New (1) 
    //Meta buffer           (R) = Start Time, (G) = End of life, (B) = Start size, (A) = Start rotation
    //Position buffer       (RGB) = Position, (A) = Random Value
    //Velocity buffer       (RGB) = Velocity, (A) = Max velocity
    
    //Because of simultanious read and write, meta, position and velocity buffers need to be double buffered

    public RenderTexture newParticleBuffer;

    public RenderTexture metaBuffer_1;
    public RenderTexture positionBuffer_1;
    public RenderTexture velocityBuffer_1;

    public RenderTexture metaBuffer_2;
    public RenderTexture positionBuffer_2;
    public RenderTexture velocityBuffer_2;

	private Vector4 vector;
	private Vector4[] vec;
	private float[] stren;
	#endregion

	#region Materials
	private Material calculateSpawn;
    private Material calculateMeta;
    private Material calculateVelocity;
    private Material calculatePosition;
    private Material resetMeta;
    private Material resetPosition;
	#endregion

	#region Functions
	public GPUParticleSystemBuffer(int width, int height, RenderTextureFormat precision)
    {
        newParticleBuffer = new RenderTexture(width, height, 0, RenderTextureFormat.R8);
 
        metaBuffer_1 = new RenderTexture(width, height, 0, precision);
        positionBuffer_1 = new RenderTexture(width, height, 0, precision);
        velocityBuffer_1 = new RenderTexture(width, height, 0, precision);

        metaBuffer_2 = new RenderTexture(width, height, 0, precision);
        positionBuffer_2 = new RenderTexture(width, height, 0, precision);
        velocityBuffer_2 = new RenderTexture(width, height, 0, precision);
        
        newParticleBuffer.name = "New Particles";

        metaBuffer_1.name = "Meta 1";
        positionBuffer_1.name = "Position 1";
        velocityBuffer_1.name = "Velocity 1";

        metaBuffer_2.name = "Meta 2";
        positionBuffer_2.name = "Position 2";
        velocityBuffer_2.name = "Velocity 2";

        newParticleBuffer.filterMode = FilterMode.Point;
        metaBuffer_1.filterMode = FilterMode.Point;
        positionBuffer_1.filterMode = FilterMode.Point;
        velocityBuffer_1.filterMode = FilterMode.Point;
        metaBuffer_2.filterMode = FilterMode.Point;
        positionBuffer_2.filterMode = FilterMode.Point;
        velocityBuffer_2.filterMode = FilterMode.Point;

        metaBuffer_1.autoGenerateMips = false;
        positionBuffer_1.autoGenerateMips = false;
        velocityBuffer_1.autoGenerateMips = false;
        metaBuffer_2.autoGenerateMips = false;
        positionBuffer_2.autoGenerateMips = false;
        velocityBuffer_2.autoGenerateMips = false;

        metaBuffer_1.Create();
        positionBuffer_1.Create();
        velocityBuffer_1.Create();

        metaBuffer_2.Create();
        positionBuffer_2.Create();
        velocityBuffer_2.Create();

        calculateSpawn = new Material(Shader.Find("GPUParticles/Internal/Spawner"));
        calculateMeta = new Material(Shader.Find("GPUParticles/Internal/Meta"));
        calculateVelocity = new Material(Shader.Find("GPUParticles/Internal/Velocity"));
        calculatePosition = new Material(Shader.Find("GPUParticles/Internal/Position"));
        resetMeta = new Material(Shader.Find("GPUParticles/Internal/ResetMetaBuffer"));
        resetPosition = new Material(Shader.Find("GPUParticles/Internal/ResetPositionBuffer"));

        calculateMeta.SetTexture("_NewParticle", newParticleBuffer);
        calculateMeta.SetTexture("_Meta", metaBuffer_1);
        calculateMeta.SetTexture("_Position", positionBuffer_1);

        calculateVelocity.SetTexture("_NewParticle", newParticleBuffer);
        calculateVelocity.SetTexture("_Meta", metaBuffer_1);
        calculateVelocity.SetTexture("_Velocity", velocityBuffer_1);
        calculateVelocity.SetTexture("_Position", positionBuffer_1);

        calculatePosition.SetTexture("_NewParticle", newParticleBuffer);
        calculatePosition.SetTexture("_Meta", metaBuffer_1);
        calculatePosition.SetTexture("_Velocity", velocityBuffer_1);
        calculatePosition.SetTexture("_Position", positionBuffer_1);

		//Set up Position Buffer with Random Value
        Texture2D TextureBuffer1 = new Texture2D(width, height, TextureFormat.RGBAFloat, false);
        TextureBuffer1.filterMode = FilterMode.Point;

        Color[] c = new Color[width * height];

        for (int i = 0; i < c.Length; i++)
        {
            c[i] = new Color(
                0f,
                0f,
                0f,
                Random.Range(0.0f, 1.0f));
        }

        TextureBuffer1.SetPixels(c);
        TextureBuffer1.Apply(false);

        Graphics.Blit(TextureBuffer1, positionBuffer_1);
        
		//Graphics.Blit(null, positionBuffer_1, resetPosition);
		Setup(width, height);
    }

    public void Reset()
    {
        Graphics.Blit(metaBuffer_1, metaBuffer_1, resetMeta, 0);
        Graphics.Blit(velocityBuffer_1, velocityBuffer_1, resetMeta);
		//Graphics.Blit(null, positionBuffer_1, resetPosition);
		//Graphics.Blit(null, positionBuffer_2, resetPosition);
		Graphics.Blit(positionBuffer_1, positionBuffer_2, resetPosition);
		Graphics.CopyTexture(positionBuffer_2, positionBuffer_1);
	}
	#endregion

	#region Setup
	private void Setup(int width, int height)
    {
        SetUpSpawnMaterial(width, height);
        SetUpMetaMaterial();
        SetUpVelocityMaterial();
        SetUpPositionMaterial();
        SetUpResetPositionMaterial();
    }

    private void SetUpSpawnMaterial(int width, int height)
    {
        calculateSpawn.SetInt("_MapWidth", width);
        calculateSpawn.SetInt("_MapHeight", height);
    }

    private void SetUpMetaMaterial()
    {
        calculateMeta.SetTexture("_NewParticle", newParticleBuffer);
        calculateMeta.SetTexture("_Meta", metaBuffer_1);
        calculateMeta.SetTexture("_Position", positionBuffer_1);
    }

    private void SetUpVelocityMaterial()
    {
        calculateVelocity.SetTexture("_NewParticle", newParticleBuffer);
        calculateVelocity.SetTexture("_Meta", metaBuffer_1);
        calculateVelocity.SetTexture("_Position", positionBuffer_1);
        calculateVelocity.SetTexture("_Velocity", velocityBuffer_1);
    }

    private void SetUpPositionMaterial()
    {
        calculatePosition.SetTexture("_NewParticle", newParticleBuffer);
        calculatePosition.SetTexture("_Meta", metaBuffer_1);
        calculatePosition.SetTexture("_Position", positionBuffer_1);
        calculatePosition.SetTexture("_Velocity", velocityBuffer_1);
    }

    private void SetUpResetPositionMaterial()
    {
        resetPosition.SetTexture("_Position", positionBuffer_1);
    }
    #endregion

    #region Updates
    public void UpdateTime(float customTime, float customDeltaTime)
    {
        calculateMeta.SetFloat("_CustomTime", customTime);
        calculateVelocity.SetFloat("_CustomTime", customTime);
        calculateVelocity.SetFloat("_CustomDeltaTime", customDeltaTime);
        calculatePosition.SetFloat("_CustomTime", customTime);
        calculatePosition.SetFloat("_CustomDeltaTime", customDeltaTime);
    }

    public void UpdateEmitterParameters(Vector3 emitterPosition, Vector4 emitterParameters)
    {
        calculatePosition.SetVector("_EmitterPosition", new Vector4(emitterPosition.x, emitterPosition.y, emitterPosition.z, 1f));
        calculatePosition.SetVector("_EmitterParam", emitterParameters);
        calculateVelocity.SetVector("_EmitterParam", emitterParameters);
    }

    public void UpdateEmitterMatrix(Matrix4x4 emitterMatrix)
    {
        calculatePosition.SetMatrix("_EmitterMatrix", emitterMatrix);
        calculateVelocity.SetMatrix("_EmitterMatrix", emitterMatrix);
    }

    public void UpdateStartLifetimeSpeed(FloatCurveBundle startLifetime, FloatCurveBundle startSpeed, float progress)
    {
        Vector4 lifetimeSpeed = EvaluationHelper.EvaluateLifeTimeStartSpeed(startLifetime, startSpeed, progress);
        calculateMeta.SetVector("_StartLifeTimeSpeed", lifetimeSpeed);
        calculateVelocity.SetVector("_StartLifeTimeSpeed", lifetimeSpeed);
    }

    public void UpdateStartSizeRotation(FloatCurveBundle startSize, FloatCurveBundle startRotation, float progress)
    {
        Vector4 sizeRotation = EvaluationHelper.EvaluateStartSizeRotation(startSize, startRotation, progress);
        calculateMeta.SetVector("_StartSizeRot", sizeRotation);
    }

    public void Emit(float startID, float endID)
    {
        calculateSpawn.SetFloat("_StartID", Mathf.Floor(startID));
        calculateSpawn.SetFloat("_EndID", Mathf.Floor(endID));
        Graphics.Blit(null, newParticleBuffer, calculateSpawn, 0);
    }

    public void StepMeta()
    {
        Graphics.Blit(metaBuffer_1, metaBuffer_2, calculateMeta, 0);
        Graphics.Blit(metaBuffer_2, metaBuffer_1);
    }

    public void StepVelocity()
    {
        Graphics.Blit(velocityBuffer_1, velocityBuffer_2, calculateVelocity, 0);
        Graphics.Blit(velocityBuffer_2, velocityBuffer_1);
    }

    public void StepPosition()
    {
        Graphics.Blit(positionBuffer_1, positionBuffer_2, calculatePosition, 0);
        Graphics.Blit(positionBuffer_2, positionBuffer_1);
    }

    public void UpdateGravity(float gravity)
    {
        calculateVelocity.SetFloat("_Gravity", gravity);
    }

    public void UpdateAirResistance(float resistance)
    {
        calculateVelocity.SetFloat("_AirResistance", resistance);
    }

    public void UpdateMaxVelocityOverLifetime(SingleFloatCurveBundle maxVelocity, float progress)
    {
        calculateVelocity.SetFloat("_MaxVelocity", EvaluationHelper.EvaluateSingleFloatCurveBundle(maxVelocity, progress));
    }

    public void UpdateCircularForceOverLifetime(Vector3CurveBundle circularForce, float progress)
    {
        calculateVelocity.SetVector("_CircularForceOverLifetime", EvaluationHelper.EvaluateVector3Bundle(circularForce, progress));
    }

    public void UpdateInheritVelocity(Vector3 velocity)
    {
		calculateVelocity.SetVector("_EmitterVelocity", velocity);
	}

    public void UpdateCircularForceCenterPosition(Vector3 position)
    {
		calculateVelocity.SetVector("_CircularForceCenter", position);
    }

    public void UpdateTurbulenceTexture(Texture2D turbulenceNoise)
    {
        calculateVelocity.SetTexture("_Turbulence", turbulenceNoise);
    }

    public void UpdateVectorField(Texture3D vectorField)
    {
        calculateVelocity.SetTexture("_TurbulenceDDD", vectorField);
    }

    public void UpdateAmplitude(Vector3CurveBundle amplitude, float progress)
    {
        calculateVelocity.SetVector("_Amplitude", EvaluationHelper.EvaluateVector3Bundle(amplitude, progress));
    }

    public void UpdateFrequency(Vector3CurveBundle frequency, float progress)
    {
        Vector4 freq = EvaluationHelper.EvaluateVector3Bundle(frequency, progress);
        calculateVelocity.SetVector("_Frequency", freq);
    }

	public void UpdateTightness(float Tightness)
	{
		calculateVelocity.SetFloat("_Tightness", Tightness);
	}
	

	public void UpdateOffset(Vector3CurveBundle offset, float progress)
    {
        calculateVelocity.SetVector("_Offset", EvaluationHelper.EvaluateVector3Bundle(offset, progress));
    }

    public void UpdateVectorFieldMatrix(Transform vectorFieldTransform, Vector3CurveBundle rotation, Vector3CurveBundle frequency, float progress, float customDeltaTime)
    {
        vectorFieldTransform.localScale = EvaluationHelper.EvaluateVector3Bundle(frequency, progress);
        Vector3 rot = EvaluationHelper.EvaluateVector3Bundle(rotation, progress);
        vectorFieldTransform.Rotate(rot * customDeltaTime);
        calculateVelocity.SetMatrix("_TurbulenceMatrix", vectorFieldTransform.worldToLocalMatrix);
    }

	public void UpdateAttractors(List<AttractorListElement> attractors, Vector3 center)
	{
		int length = attractors.Count >= 4 ? 4 : attractors.Count;

		if (vec == null)
			vec = new Vector4[4];

		if (stren == null)
			stren = new float[4];

		for (int i = 0; i < length; i++)
		{
			if (attractors[i].attractorPosition == null)
			{
				vector = new Vector4(center.x, center.y, center.z, attractors[i].attenuation);
			}
			else
			{
				vector = new Vector4(attractors[i].attractorPosition.position.x, attractors[i].attractorPosition.position.y, attractors[i].attractorPosition.position.z, attractors[i].attenuation);
			}
			vec[i] = vector;
			stren[i] = attractors[i].attractionStrength;
		}

		calculateVelocity.SetVectorArray("_Attractor", vec);
		calculateVelocity.SetFloatArray("_Strength", stren);
	}

	public void UpdateForceOverLifetime(Vector3CurveBundle force, float progress)
	{
		Vector3 currentForce = EvaluationHelper.EvaluateVector3Bundle(force, progress);
		calculateVelocity.SetVector("_ForceOverLifetime", currentForce);
	}

    public void UpdateSkinnedMeshEmitterTexture(RenderTexture renderTexture)
    {
		calculateVelocity.SetTexture("_MeshEmitter", renderTexture);
		calculatePosition.SetTexture("_MeshEmitter", renderTexture);
	}

	public void UpdateMeshEmitterTexture(Texture2D texture)
	{
		calculateVelocity.SetTexture("_MeshEmitter", texture);
		calculatePosition.SetTexture("_MeshEmitter", texture);
	}
	#endregion

	#region KeyWords
	public void EmitterShape(GPUParticleSystem.EmitterShape shape)
    {
        calculateVelocity.DisableKeyword("POINT");
        calculateVelocity.DisableKeyword("EDGE");
        calculateVelocity.DisableKeyword("CIRCLE");
        calculateVelocity.DisableKeyword("BOX");
        calculateVelocity.DisableKeyword("HEMISPHERE");
        calculateVelocity.DisableKeyword("SPHERE");
        calculateVelocity.DisableKeyword("CONE");
        calculateVelocity.DisableKeyword("MESH");

        calculatePosition.DisableKeyword("POINT");
        calculatePosition.DisableKeyword("EDGE");
        calculatePosition.DisableKeyword("CIRCLE");
        calculatePosition.DisableKeyword("BOX");
        calculatePosition.DisableKeyword("HEMISPHERE");
        calculatePosition.DisableKeyword("SPHERE");
        calculatePosition.DisableKeyword("CONE");
        calculatePosition.DisableKeyword("MESH");

        switch (shape)
        {
            case GPUParticleSystem.EmitterShape.Point:
                calculateVelocity.EnableKeyword("POINT");
                calculatePosition.EnableKeyword("POINT");
                break;

            case GPUParticleSystem.EmitterShape.Edge:
                calculateVelocity.EnableKeyword("EDGE");
                calculatePosition.EnableKeyword("EDGE");
                break;

            case GPUParticleSystem.EmitterShape.Circle:
                calculateVelocity.EnableKeyword("CIRCLE");
                calculatePosition.EnableKeyword("CIRCLE");
                break;

            case GPUParticleSystem.EmitterShape.Box:
                calculateVelocity.EnableKeyword("BOX");
                calculatePosition.EnableKeyword("BOX");
                break;

            case GPUParticleSystem.EmitterShape.HemiSphere:
                calculateVelocity.EnableKeyword("HEMISPHERE");
                calculatePosition.EnableKeyword("HEMISPHERE");
                break;

            case GPUParticleSystem.EmitterShape.Sphere:
                calculateVelocity.EnableKeyword("SPHERE");
                calculatePosition.EnableKeyword("SPHERE");
                break;

            case GPUParticleSystem.EmitterShape.Cone:
                calculateVelocity.EnableKeyword("CONE");
                calculatePosition.EnableKeyword("CONE");
                break;
				/*
            case GPUParticleSystem.EmitterShape.Mesh:
                calculateVelocity.EnableKeyword("MESH");
                calculatePosition.EnableKeyword("MESH");
                break;

            case GPUParticleSystem.EmitterShape.MeshFilter:
                calculateVelocity.EnableKeyword("MESH");
                calculatePosition.EnableKeyword("MESH");
                break;

            case GPUParticleSystem.EmitterShape.SkinnedMeshRenderer:
                calculateVelocity.EnableKeyword("MESH");
                calculatePosition.EnableKeyword("MESH");
                break;
				*/
        }
    }

    public void SimSpace(GPUParticleSystem.GPUSimulationSpace simSpace)
    {
        if (simSpace == GPUParticleSystem.GPUSimulationSpace.Local)
        {
            calculateVelocity.EnableKeyword("LOCALSIM");
            calculatePosition.EnableKeyword("LOCALSIM");
        }
        else {
            calculateVelocity.DisableKeyword("LOCALSIM");
            calculatePosition.DisableKeyword("LOCALSIM");
        }
    }

    public void Attractors(bool active)
    {
        if (active)
        {
            calculateVelocity.EnableKeyword("ATTRACTORS");
        }
        else {
            calculateVelocity.DisableKeyword("ATTRACTORS");
        }
    }

    public void MeshTarget(bool active)
    {
        if (active)
        {
            calculateVelocity.EnableKeyword("MESHTARGET");
        }
        else
        {
            calculateVelocity.DisableKeyword("MESHTARGET");
        }
    }

    public void LimitVelocity(bool active)
    {
        if (active)
        {
            calculateVelocity.EnableKeyword("LIMITVELOCITY");
        }
        else
        {
            calculateVelocity.DisableKeyword("LIMITVELOCITY");
        }
    }

    public void Turbulence(GPUParticleSystem.TurbulenceType turbulenceType)
    {
        calculateVelocity.DisableKeyword("TEXTURE");
        calculateVelocity.DisableKeyword("VECTORFIELDS");

        switch (turbulenceType)
        {
            case GPUParticleSystem.TurbulenceType.Texture:
                calculateVelocity.EnableKeyword("TEXTURE");
                break;

            case GPUParticleSystem.TurbulenceType.VectorField:
                calculateVelocity.EnableKeyword("VECTORFIELDS");
                break;
        }
    }

    public void EmitFromShell(bool active)
    {
        if (active)
        {
            calculatePosition.EnableKeyword("EMITFROMSHELL");
        }
        else
        {
            calculatePosition.DisableKeyword("EMITFROMSHELL");
        }
    }

    public void EmitFromBase(bool active)
    {
        if (active)
        {
            calculatePosition.EnableKeyword("EMITFROMBASE");
        }
        else
        {
            calculatePosition.DisableKeyword("EMITFROMBASE");
        }
    }

    public void CircularForce(bool active)
    {
        if (active)
        {
            calculateVelocity.EnableKeyword("CIRCULAR_FORCE");
        }
        else
        {
            calculateVelocity.DisableKeyword("CIRCULAR_FORCE");
        }
    }
    #endregion
}
