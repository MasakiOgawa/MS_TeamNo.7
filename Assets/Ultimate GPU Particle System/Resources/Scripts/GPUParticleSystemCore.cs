using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GPUParticleSystem
{
    #region Core
	public void PrepareParticleData()
	{
		maxParticles = bufferWidth * bufferHeight;

		if (precision == RenderTexturePrecision.Half)
		{
			particleData = new GPUParticleSystemBuffer(bufferWidth, bufferHeight, RenderTextureFormat.ARGBHalf);
		}
		else
		{
			particleData = new GPUParticleSystemBuffer(bufferWidth, bufferHeight, RenderTextureFormat.ARGBFloat);
		}

		if (particleMaterial == null)
		{
			particleMaterial = new Material(Shader.Find("GPUParticles/GPUParticles"));
			SetRenderQueue();
		}
	}

	public void ClearParticleData()
	{
		RenderTexture.active = null;
		DestroyImmediate(particleData.newParticleBuffer);
		DestroyImmediate(particleData.metaBuffer_1);
		DestroyImmediate(particleData.metaBuffer_2);
		DestroyImmediate(particleData.positionBuffer_1);
		DestroyImmediate(particleData.positionBuffer_2);
		DestroyImmediate(particleData.velocityBuffer_1);
		DestroyImmediate(particleData.velocityBuffer_2);
		particleData = null;
	}

    private void InternalEmit()
    {
        if (startID >= maxParticles)
        {
			startID = 0f;
            endID = 0f;
        }

        if (emit)
        {
            float cEmission = EvaluationHelper.EvaluateFloatCurveBundle(emissionRate, progress);
            endID += burstNum + cEmission * customDeltaTime;
			burstNum = 0;

			if (endID >= startID + 1)
            {
				particleData.Emit(startID, endID);
				startID = endID;
			}
            else
            {
                particleData.Emit(0f, 0f);
            }
        }
    }

    private void InternalStep()
    {
        UpdateValues();
        particleData.StepMeta();
        particleData.StepVelocity();
        particleData.StepPosition();
    }

    private void Initialize()
    {
        SetMainTexture();
        SetMetaTexture();
        SetVelocityTexture();
        SetPositionTexture();
        SetUpDataTextures();
        SetParticleStretch();
        SetBlendMode();
        SetZBuffer();
        UpdateVectorfieldFile();
        SetTurbulenceKeyword();
        UpdateTurbulenceTexture();
        SetLimitVelocity();
        SetCircularForce();
        UpdateColorOverLifeTime();

		//UpdateEmitterTexture();

		if (turbulenceType == TurbulenceType.VectorField)
        {
            UpdateVectorField();
            UpdateVectorFieldMatrix();
        }
		/*
        if (emitterShape == EmitterShape.SkinnedMeshRenderer)
        {
            PrepareSkinnedMesh();
        }
		*/
		UpdateAttractors();

		//Keywords only
		SetRotationKeyword();
        SetEmitterShapeKeyword();
        SetEmitFromShell(emitFromShell);
        SetEmitFromBase(emitFromBase);
		SetAttractorKeyword();
        SetParticleTypeKeyword(particleType);
        SetSimulationSpace();
    }

    private void UpdateValues()
    {
        UpdateStartSpeedAndLifetime();
        UpdateStartSizeAndRotation();
        UpdateEmitter();
        UpdateEmitterMatrix();
        UpdateSizeOverLifetime();
        UpdateRotationOverLifetime();
        UpdateOffset();
        UpdateColorIntensity();
        UpdateAmplitude();
        UpdateFrequency();
		UpdateTightness();
		UpdateOffset();
        UpdateAirResistance();
        UpdateGravity();
        UpdateCircularForceOverLifetime();
        UpdateCircularForceCenter();
		UpdateMaxVelocityOverLifetime();

		if (useInheritVelocity)
		{
			UpdateInheritVelocity();
		}
		/*
		if (emitterShape == EmitterShape.SkinnedMeshRenderer)
        {
            UpdateSkinnedMeshTexture();
        }
		*/
        if (turbulenceType == TurbulenceType.VectorField)
        {
            UpdateVectorFieldMatrix();
        }

        if (gravity.mode == SimpleValueMode.Curve)
        {
            UpdateGravity();
        }

        if (airResistance.mode == SimpleValueMode.Curve)
        {
            UpdateAirResistance();
        }

		UpdateAttractors();
		UpdateForceOverLifetime();
	}

    public void EmitNumParticles(int numParticles)
    {
		burstNum += numParticles;
		
		//endID += numParticles;
        //particleData.Emit(startID, endID);
        //startID = endID;
        //InternalStep();
    }
    #endregion

    #region ParticleMesh
    public void ForceRecreateParticles()
    {
		ClearMeshes();
		MakeMeshes();
		Initialize();
    }

    private void MakeMeshes()
    {
		ClearMeshes();
		SetRenderQueue();

		switch (particleType)
        {
            case ParticleType.Point:
				
                particleMeshes = GPUParticleSystemMeshUtility.CreateParticlesPoint(bufferWidth, bufferHeight, particleMaterial);
                break;

            case ParticleType.Triangle:
                particleMeshes = GPUParticleSystemMeshUtility.CreateParticlesTriangle(bufferWidth, bufferHeight, particleMaterial);
                break;
				/*
            case ParticleType.Mesh:
                if (meshParticle == null)
                {
                    Debug.LogWarning("[GPUP] No particles created, assign a Mesh!");
                }
                else
                {
                    particleMeshes = GPUParticleSystemMeshUtility.CreateMeshParticles(meshParticle, bufferWidth, bufferHeight, particleMaterial, true);
                }
                break;

            case ParticleType.AnimatedMesh:
                if (meshParticle == null)
                {
                    Debug.LogWarning("[GPUP] No particles created, assign a Mesh!");
                }
                else
                {
                    particleMeshes = GPUParticleSystemMeshUtility.CreateMeshParticles(meshParticle, bufferWidth, bufferHeight, particleMaterial, true);
                }
                break;
				*/
            case ParticleType.StretchedTail:
                particleMeshes = GPUParticleSystemMeshUtility.CreateParticlesDoubleQuad(bufferWidth, bufferHeight, particleMaterial);
                break;

            default:
                particleMeshes = GPUParticleSystemMeshUtility.CreateParticlesQuad(bufferWidth, bufferHeight, particleMaterial);
                break;
        }
    }

    private void ClearMeshes()
    {
        if (particleMeshes != null)
        {
            for (int i = 0; i < particleMeshes.Length; i++)
            {
                if (particleMeshes[i] != null)
                {
                    DestroyImmediate(particleMeshes[i]);
                }
            }
        }
		particleMeshes = null;

	}
    #endregion

    #region MaterialManagement
    private void UpdateTimeInMaterials()
    {
        particleMaterial.SetFloat("_CustomTime", customTime);
        particleData.UpdateTime(customTime, customDeltaTime);
    }

    public void SetMainTexture()
    {
        if (mainTexture != null)
        {
            particleMaterial.SetTexture("_MainTex", mainTexture);
            SetMainTextureKeyword(true);
        }
        else {
            SetMainTextureKeyword(false);
        }
    }

    private void SetMetaTexture()
    {
        particleMaterial.SetTexture("_Meta", particleData.metaBuffer_1);
    }

    private void SetVelocityTexture()
    {
        particleMaterial.SetTexture("_Velocity", particleData.velocityBuffer_1);
    }

    private void SetPositionTexture()
    {
        particleMaterial.SetTexture("_Position", particleData.positionBuffer_1);
    }

    private void SetParticleStretch()
    {
        particleMaterial.SetFloat("_VelocityScale", stretchMultiplier);
    }
	#endregion

	#region ValueManagement
	public void UpdateParticleMainTex(Texture2D mainTexture)
    {
        if (mainTexture != null)
        {
            this.mainTexture = mainTexture;
            particleMaterial.SetTexture("_MainTex", mainTexture);
            SetMainTextureKeyword(true);
            return;
        }

        if (this.mainTexture != null)
        {
            particleMaterial.SetTexture("_MainTex", this.mainTexture);
            SetMainTextureKeyword(true);
            return;
        }

        SetMainTextureKeyword(false);
    }

    public void UpdateStartSpeedAndLifetime()
    {
        particleData.UpdateStartLifetimeSpeed(startLifetime, startSpeed, progress);
    }

    public void UpdateStartSizeAndRotation()
    {
        particleData.UpdateStartSizeRotation(startSize, startRotation, progress);
    }

    public void UpdateEmitterParameters()
    {
        param4 = Mathf.Tan(param2 * Mathf.Deg2Rad) * param3;
        Vector4 emitterParameters = new Vector4(param1, param2, param3, param1 + param4);

        if (simulationSpace == GPUSimulationSpace.Local)
        {
            particleData.UpdateEmitterParameters(transform.position, emitterParameters);
        } else {
            particleData.UpdateEmitterParameters(transform.position, Vector3.zero);
        }
    }

    public void UpdateEmitterMatrix()
    {
        switch (emitterShape)
        {
			/*
            case EmitterShape.MeshFilter:
                if (meshFilterEmitter != null)
                {
                    particleData.UpdateEmitterMatrix(meshFilterEmitter.transform.localToWorldMatrix);
                }
                break;
            case EmitterShape.SkinnedMeshRenderer:
                if (skinnedMeshRendererEmitter != null)
                {
                    particleData.UpdateEmitterMatrix(skinnedMeshRendererEmitter.transform.localToWorldMatrix);
                }
                break;
				*/
            default:
                particleData.UpdateEmitterMatrix(transform.localToWorldMatrix);
                break;
        }
    }

    public void UpdateParticleColorIntensity()
    {
        particleMaterial.SetFloat("_ColorIntensity", colorOverLifetime.intensity);
    }

    public void UpdateStretchMultiplier()
    {
        particleMaterial.SetFloat("_VelocityScale", stretchMultiplier);
    }

    public void UpdateColorOverLifeTime()
    {
        if (colorTexture == null)
        {
            SetUpDataTextures();
        }

        switch (colorOverLifetime.mode)
        {
            case ColorMode.Color:
                colorTexture = GPUParticleSystemTextureHelper.BakeColor(colorOverLifetime.color1, colorTexture);
                break;

            case ColorMode.RandomTwoColors:
                colorTexture = GPUParticleSystemTextureHelper.BakeColor(colorOverLifetime.color1, colorOverLifetime.color2, colorTexture);
                break;

            case ColorMode.Gradient:
                colorTexture = GPUParticleSystemTextureHelper.BakeColor(colorOverLifetime.gradient1, colorTexture);
                break;

            case ColorMode.RandomTwoGradients:
                colorTexture = GPUParticleSystemTextureHelper.BakeColor(colorOverLifetime.gradient1, colorOverLifetime.gradient2, colorTexture);
                break;
        }

        particleMaterial.SetTexture("_ColorOverLifetime", colorTexture);
    }

    public void UpdateColorIntensity()
    {
        particleMaterial.SetFloat("_ColorIntensity", EvaluationHelper.EvaluateSingleFloatCurveBundle(colorIntensityOverLifetime, progress));
    }

    public void UpdateMaxVelocityOverLifetime()
    {
        particleData.UpdateMaxVelocityOverLifetime(maxVelocity, progress);
    }

    public void UpdateCircularForceOverLifetime()
    {
        particleData.UpdateCircularForceOverLifetime(circularForce, progress);
    }

    private void UpdateCircularForceCenter()
    {
        if (!useCircularForce)
            return;

        if (circularForceCenter == null)
        {
            GameObject g = new GameObject("Center of circular force");
			g.hideFlags = HideFlags.HideInHierarchy;
			circularForceCenter = g.transform;
        }

        particleData.UpdateCircularForceCenterPosition(circularForceCenter.position + safetyVector);
    }

    public void UpdateSizeOverLifetime()
    {
        SetSizeOverLifetimeKeyword(sizeOverLifetime.mode);

        switch (sizeOverLifetime.mode)
        {
            case CurveMode.Off:
                break;

            case CurveMode.Linear:
                sizeOverLifetime.skew = Mathf.Clamp(sizeOverLifetime.skew, 0.1f, 15f);
                particleMaterial.SetFloat("_SizeMultiplier", sizeOverLifetime.multiplier);
                particleMaterial.SetFloat("_SizeOverLifetimeSkew", sizeOverLifetime.skew);

                if (sizeOverLifetime.invert)
                {
                    particleMaterial.SetFloat("_Invert", 1f);
                }
                else {
                    particleMaterial.SetFloat("_Invert", 0f);
                }
                break;

            case CurveMode.Smooth:
                sizeOverLifetime.skew = Mathf.Clamp(sizeOverLifetime.skew, 0.1f, 15f);
                particleMaterial.SetFloat("_SizeMultiplier", sizeOverLifetime.multiplier);
                particleMaterial.SetFloat("_SizeOverLifetimeSkew", sizeOverLifetime.skew);

                if (sizeOverLifetime.invert)
                {
                    particleMaterial.SetFloat("_Invert", 1f);
                }
                else
                {
                    particleMaterial.SetFloat("_Invert", 0f);
                }
                break;

            case CurveMode.Curve:
                sizeOverLifetime.bezier1 = GPUParticleSystemMeshUtility.AnimationCurveToBezier(sizeOverLifetime.curve1);
                particleMaterial.SetVectorArray("_SizeOverLifetimeBezierC1", sizeOverLifetime.bezier1);
                particleMaterial.SetInt("_SOLNumSegments", sizeOverLifetime.curve1.keys.Length - 1);
                break;

            case CurveMode.RandomTwoCurves:
                sizeOverLifetime.bezier1 = GPUParticleSystemMeshUtility.AnimationCurveToBezier(sizeOverLifetime.curve1);
                sizeOverLifetime.bezier2 = GPUParticleSystemMeshUtility.AnimationCurveToBezier(sizeOverLifetime.curve2);
                particleMaterial.SetVectorArray("_SizeOverLifetimeBezierC1", sizeOverLifetime.bezier1);
                particleMaterial.SetVectorArray("_SizeOverLifetimeBezierC2", sizeOverLifetime.bezier2);
                particleMaterial.SetInt("_SOLNumSegments", sizeOverLifetime.curve1.keys.Length - 1);
                break;

            default:
                Debug.Log("Error!");
                break;
        }
    }

    public void UpdateRotationOverLifetime()
    {
        SetRotationOverLifetimeKeyword(rotationOverLifetime.mode);

        switch (rotationOverLifetime.mode)
        {
            case CurveMode.Off:
                break;

            case CurveMode.Linear:
                rotationOverLifetime.skew = Mathf.Clamp(rotationOverLifetime.skew, 0.1f, 15f);
                particleMaterial.SetFloat("_RotationMultiplier", rotationOverLifetime.multiplier);
                particleMaterial.SetFloat("_RotationOverLifetimeSkew", rotationOverLifetime.skew);
                break;

            case CurveMode.Smooth:
                rotationOverLifetime.skew = Mathf.Clamp(rotationOverLifetime.skew, 0.1f, 15f);
                particleMaterial.SetFloat("_RotationMultiplier", rotationOverLifetime.multiplier);
                particleMaterial.SetFloat("_RotationOverLifetimeSkew", rotationOverLifetime.skew);
                break;

            case CurveMode.Curve:
                rotationOverLifetime.bezier1 = GPUParticleSystemMeshUtility.AnimationCurveToBezier(rotationOverLifetime.curve1);
                particleMaterial.SetVectorArray("_RotationOverLifetimeBezierC1", rotationOverLifetime.bezier1);
                particleMaterial.SetInt("_ROLNumSegments", rotationOverLifetime.curve1.keys.Length - 1);
                break;

            case CurveMode.RandomTwoCurves:
                rotationOverLifetime.bezier1 = GPUParticleSystemMeshUtility.AnimationCurveToBezier(rotationOverLifetime.curve1);
                rotationOverLifetime.bezier2 = GPUParticleSystemMeshUtility.AnimationCurveToBezier(rotationOverLifetime.curve2);
                particleMaterial.SetVectorArray("_RotationOverLifetimeBezierC1", rotationOverLifetime.bezier1);
                particleMaterial.SetVectorArray("_RotationOverLifetimeBezierC2", rotationOverLifetime.bezier2);
                particleMaterial.SetInt("_ROLNumSegments", rotationOverLifetime.curve1.keys.Length - 1);
                break;

            default:
                Debug.Log("Error!");
                break;
        }
    }

    public void UpdateGravity()
    {
        switch (gravity.mode)
        {
            case SimpleValueMode.Value:
                particleData.UpdateGravity(gravity.value);
                break;

            case SimpleValueMode.Curve:
                particleData.UpdateGravity(gravity.curve.Evaluate(progress));
                break;
        }
    }

    public void UpdateAirResistance()
    {
        particleData.UpdateAirResistance(EvaluationHelper.EvaluateSingleFloatCurveBundle(airResistance, progress));
    }

    public void UpdateTurbulenceTexture()
    {
        particleData.UpdateTurbulenceTexture(vectorNoise);
    }

    public void UpdateVectorField()
    {
        particleData.UpdateVectorField(vectorField);
    }

    public void UpdateAmplitude()
    {
        particleData.UpdateAmplitude(turbulenceAmplitude, progress);
    }

    public void UpdateFrequency()
    {
        particleData.UpdateFrequency(turbulenceFrequency, progress);
    }

	public void UpdateTightness()
	{
		particleData.UpdateTightness(Tightness);
	}

	public void UpdateOffset()
    {
        particleData.UpdateOffset(turbulenceOffset, progress);
    }

    public void UpdateVectorfieldFile()
    {
        if (fgaFile != null)
            vectorField = EvaluationHelper.DeserializeVectorField(fgaFile);
    }

    public void UpdateVectorFieldMatrix()
    {
        if (vectorFieldObject == null)
        {
            GameObject g = new GameObject("Vector field transform");
			g.hideFlags = HideFlags.HideInHierarchy;
            vectorFieldObject = g.transform;
        }

        if (vectorFieldObject != null)
        {
			if (simulationSpace == GPUSimulationSpace.Local)
			{
				vectorFieldObject.position = transform.position;
			}
			else {
				vectorFieldObject.position = Vector3.zero;
			}
            particleData.UpdateVectorFieldMatrix(vectorFieldObject, turbulenceRotation, turbulenceFrequency, progress, customDeltaTime);
        }
    }

	/*
    public void UpdateEmitterTexture()
    {
		switch(emitterShape)
		{
			case EmitterShape.Mesh:
				meshEmitterPositionTexture = GPUParticleSystemMeshUtility.MeshToTexture(meshEmitter, meshBakeType, meshEmitterResolution);
				particleData.UpdateMeshEmitterTexture(meshEmitterPositionTexture);
				break;

			case EmitterShape.MeshFilter:
				meshEmitterPositionTexture = GPUParticleSystemMeshUtility.MeshToTexture(meshEmitter, meshBakeType, meshEmitterResolution);
				particleData.UpdateMeshEmitterTexture(meshEmitterPositionTexture);
				break;

			case EmitterShape.SkinnedMeshRenderer:
				if (skinnedMeshEmitterPositionTexture == null)
				{
					skinnedMeshEmitterPositionTexture = new RenderTexture(meshEmitterResolution, meshEmitterResolution, 0, RenderTextureFormat.ARGBHalf);
					skinnedMeshEmitterPositionTexture.name = "Emitter positions";
					skinnedMeshEmitterPositionTexture.filterMode = FilterMode.Point;
					skinnedMeshEmitterPositionTexture.useMipMap = false;
				}
				else
				{
					if (meshEmitterResolution != skinnedMeshEmitterPositionTexture.width)
					{
						DestroyImmediate(skinnedMeshEmitterPositionTexture);
						skinnedMeshEmitterPositionTexture = new RenderTexture(meshEmitterResolution, meshEmitterResolution, 0, RenderTextureFormat.ARGBHalf);
						skinnedMeshEmitterPositionTexture.name = "Emitter positions";
						skinnedMeshEmitterPositionTexture.filterMode = FilterMode.Point;
						skinnedMeshEmitterPositionTexture.useMipMap = false;
					}
					particleData.UpdateSkinnedMeshEmitterTexture(skinnedMeshEmitterPositionTexture);
				}
				break;
		}
    }
	*/

		/*
    public void PrepareSkinnedMesh()
    {
        if (meshEmitter == null)
        {
            meshEmitter = new Mesh();
        }

        if (skinnedMeshEmitterTransform == null)
        {
            CreateSkinnedMeshCam();
        }

        if (skinnedMeshRendererEmitter != null)
        {
            originalMesh = skinnedMeshRendererEmitter.sharedMesh;
            string s = originalMesh.name;

            if (originalMesh != null && !s.Contains("converted"))
            {
                meshEmitter = GPUParticleSystemMeshUtility.ProcessSkinnedMesh(originalMesh);

                if (meshEmitter != null)
                {
                    skinnedMeshRendererEmitter.sharedMesh = meshEmitter;
                }
                else {
                    Debug.LogWarning("[GPU P] Could not prepare skinned mesh.");
                }
            }
            else
            {
                Debug.LogWarning("[GPU P] The selected skinned mesh renderer does not have a mesh.");
            }
        }
        else
        {
            Debug.LogWarning("[GPU P] No skinned mesh renderer assigned.");
        }
    }

    public void UpdateSkinnedMeshTexture()
    {
        if (skinnedMeshRendererEmitter == null)
        {
            CreateSkinnedMeshCam();
        }

        if (skinnedMeshEmitterCam == null)
        {
            skinnedMeshEmitterCam = skinnedMeshRendererEmitter.GetComponent<Camera>();
            CreateSkinnedMeshCam();
        }

        int layer = skinnedMeshRendererEmitter.gameObject.layer;
        Material mat = skinnedMeshRendererEmitter.sharedMaterial;
        skinnedMeshRendererEmitter.sharedMaterial = skinnedMeshEmitterMaterial;
        skinnedMeshRendererEmitter.gameObject.layer = 31;
        skinnedMeshEmitterCam.Render();
        skinnedMeshRendererEmitter.gameObject.layer = layer;
        skinnedMeshRendererEmitter.sharedMaterial = mat;
    }

    private void CreateSkinnedMeshCam()
    {
        GameObject g = new GameObject("Skinned Mesh Renderer Camera");
        skinnedMeshEmitterCam = g.AddComponent<Camera>();
        skinnedMeshEmitterTransform = g.transform;
        skinnedMeshEmitterCam.enabled = false;
        skinnedMeshEmitterCam.cullingMask = Mathf.FloorToInt(Mathf.Pow(2, 31));
        skinnedMeshEmitterCam.clearFlags = CameraClearFlags.Color;
        skinnedMeshEmitterCam.backgroundColor = Color.black;
        skinnedMeshEmitterCam.targetTexture = skinnedMeshEmitterPositionTexture;
        skinnedMeshEmitterMaterial = new Material(Shader.Find("FX/SkinnedMeshSquare"));
    }
	*/

	private void UpdateInheritVelocity()
	{
		emitterVelocity = Vector3.Normalize(transform.position - previousEmitterPosition) * EvaluationHelper.EvaluateSingleFloatCurveBundle(inheritVelocityMultiplyer, progress) * customDeltaTime;
		previousEmitterPosition = transform.position;

		particleData.UpdateInheritVelocity(emitterVelocity);
	}

	private void UpdateAttractors()
	{
		if (attractors.Count > 0)
			particleData.UpdateAttractors(attractors, transform.position);
	}

	public void UpdateForceOverLifetime()
	{
		particleData.UpdateForceOverLifetime(forceOverLifetime, progress);
	}
    #endregion

    #region KeyWordManagement
    public void SetMainTextureKeyword(bool active)
    {
        if (active)
        {
            particleMaterial.EnableKeyword("MAINTEX");
        }
        else {
            particleMaterial.DisableKeyword("MAINTEX");
        }
    }

    public void SetTurbulenceKeyword()
    {
        if (turbulenceType != TurbulenceType.VectorField)
        {
            if (vectorFieldObject != null)
            {
                DestroyImmediate(vectorFieldObject.gameObject);
            }
        }
        else
        {
            if (vectorFieldObject == null)
            {
                GameObject g = new GameObject("Vector field transform");
                vectorFieldObject = g.transform;
            }
        }
        particleData.Turbulence(turbulenceType);
    }

    public void SetRotationKeyword()
    {
        if (useRotation)
        {
            particleMaterial.EnableKeyword("ROTATION");
        }
        else
        {
            particleMaterial.DisableKeyword("ROTATION");
        }
    }

    public void SetRotationOverLifetimeKeyword(bool active)
    {
        if (active)
        {
            particleMaterial.EnableKeyword("ROTATION");
        }
        else
        {
            particleMaterial.DisableKeyword("ROTATION");
        }
    }

    public void SetParticleTypeKeyword(ParticleType type)
    {
        particleMaterial.DisableKeyword("POINT");
        particleMaterial.DisableKeyword("TRIANGLE");
        particleMaterial.DisableKeyword("BILLBOARD");
        particleMaterial.DisableKeyword("H_BILLBORD");
        particleMaterial.DisableKeyword("V_BILLBOARD");
        particleMaterial.DisableKeyword("TS_BILLBOARD");
        particleMaterial.DisableKeyword("S_BILLBOARD");
        particleMaterial.DisableKeyword("MESH");
        particleMaterial.DisableKeyword("ANIMATED_MESH");

        switch (type)
        {
            case ParticleType.Point:
                particleMaterial.EnableKeyword("POINT");
                break;

            case ParticleType.Triangle:
                particleMaterial.EnableKeyword("TRIANGLE");
                break;

            case ParticleType.Billboard:
                particleMaterial.EnableKeyword("BILLBOARD");
                break;

            case ParticleType.HorizontalBillboard:
                particleMaterial.EnableKeyword("H_BILLBORD");
                break;

            case ParticleType.VerticalBillboard:
                particleMaterial.EnableKeyword("V_BILLBOARD");
                break;

            case ParticleType.StretchedTail:
                particleMaterial.EnableKeyword("TS_BILLBOARD");
                break;

            case ParticleType.StretchedBillboard:
                particleMaterial.EnableKeyword("S_BILLBOARD");
                break;
				/*
            case ParticleType.Mesh:
                particleMaterial.EnableKeyword("MESH");
                break;

            case ParticleType.AnimatedMesh:
                particleMaterial.EnableKeyword("ANIMATED_MESH");
                break;
				*/
        }
    }

    public void SetTextureSheetKeyword(TextureSheetMode mode)
    {
        switch (mode)
        {
            case TextureSheetMode.Off:
                particleMaterial.DisableKeyword("TEXTURESHEET");
                particleMaterial.DisableKeyword("TEXTURESHEET_MOTIONVECTORS");
                break;

            case TextureSheetMode.TextureSheet:
                particleMaterial.EnableKeyword("TEXTURESHEET");
                break;

            case TextureSheetMode.TextureSheetMotionVectors:
                particleMaterial.EnableKeyword("TEXTURESHEET_MOTIONVECTORS");
                break;
        }
    }

    public void SetRandomIndexKeyword(bool active)
    {
        if (active)
        {
            particleMaterial.EnableKeyword("RANDOMINDEX");
        }
        else
        {
            particleMaterial.DisableKeyword("RANDOMINDEX");
        }
    }

    public void SetSizeOverLifetimeKeyword(CurveMode mode)
    {
        particleMaterial.DisableKeyword("LINEAR_SIZE");
        particleMaterial.DisableKeyword("SMOOTH_SIZE");
        particleMaterial.DisableKeyword("CURVE_SIZE");
        particleMaterial.DisableKeyword("RANDOM2CURVES_SIZE");

        switch (mode)
        {
            case CurveMode.Linear:
                particleMaterial.EnableKeyword("LINEAR_SIZE");
                break;

            case CurveMode.Smooth:
                particleMaterial.EnableKeyword("SMOOTH_SIZE");
                break;

            case CurveMode.Curve:
                particleMaterial.EnableKeyword("CURVE_SIZE");
                break;

            case CurveMode.RandomTwoCurves:
                particleMaterial.EnableKeyword("RANDOM2CURVES_SIZE");
                break;
        }
    }

    public void SetRotationOverLifetimeKeyword(CurveMode mode)
    {
        particleMaterial.DisableKeyword("LINEAR_ROTATION");
        particleMaterial.DisableKeyword("SMOOTH_ROTATION");
        particleMaterial.DisableKeyword("CURVE_ROTATION");
        particleMaterial.DisableKeyword("RANDOM2CURVES_ROTATION");
        switch (mode)
        {
            case CurveMode.Linear:
                particleMaterial.EnableKeyword("LINEAR_ROTATION");
                break;

            case CurveMode.Smooth:
                particleMaterial.EnableKeyword("SMOOTH_ROTATION");
                break;

            case CurveMode.Curve:
                particleMaterial.EnableKeyword("CURVE_ROTATION");
                break;

            case CurveMode.RandomTwoCurves:
                particleMaterial.EnableKeyword("RANDOM2CURVES_ROTATION");
                break;
        }
    }

    public void SetSimulationSpace()
    {
        particleData.SimSpace(simulationSpace);

        if (simulationSpace == GPUSimulationSpace.Local)
        {
            foreach (GameObject g in particleMeshes)
            {
                if (g != null)
                {
                    g.transform.parent = transform;
                    g.transform.localPosition = Vector3.zero;
                    g.transform.localRotation = Quaternion.identity;
                    g.transform.localScale = Vector3.one;
                }
            }
        }
        else
        {
            foreach (GameObject g in particleMeshes)
            {
                if (g != null)
                {
                    g.transform.parent = null;
                    g.transform.position = Vector3.zero;
                    g.transform.localRotation = Quaternion.identity;
                    g.transform.localScale = Vector3.one;
                }
            }
        }
    }

    public void SetEmitterShapeKeyword()
    {
        particleData.EmitterShape(emitterShape);
    }

    public void SetLimitVelocity
	()
    {
        particleData.LimitVelocity(useMaxVelocity);
    }

    public void SetCircularForce()
    {
        particleData.CircularForce(useCircularForce);
    }

    public void SetBlendMode()
    {
        switch (blendMode)
        {
            case GPUParticleBlendMode.Alpha:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;

            case GPUParticleBlendMode.Additive:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.One);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.One);
                break;

            case GPUParticleBlendMode.Screen:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.DstColor);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.One);
                break;

            case GPUParticleBlendMode.Premultiplied:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.One);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;

            case GPUParticleBlendMode.Subtractive:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;

            case GPUParticleBlendMode.Multiply:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.DstColor);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;

            case GPUParticleBlendMode.Opaque:
                //particleMaterial.shader = Shader.Find("GPUParticles/ParticlePositionOpaque");
                break;

            case GPUParticleBlendMode.CutOff:
                //particleMaterial.shader = Shader.Find("GPUParticles/ParticlePositionStandard");
                break;

            //case GPUParticleBlendMode.SurfaceShader:
                //particleMaterial.shader = Shader.Find("GPUParticles/ParticlePositionStandard");
                //break;
        }
    }

    public void SetBlendMode(GPUParticleBlendMode blendMode)
    {
        this.blendMode = blendMode;

        switch (blendMode)
        {
            case GPUParticleBlendMode.Alpha:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;

            case GPUParticleBlendMode.Additive:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.One);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.One);
                break;

            case GPUParticleBlendMode.Screen:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.DstColor);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.One);
                break;

            case GPUParticleBlendMode.Premultiplied:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.One);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;

            case GPUParticleBlendMode.Subtractive:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;

            case GPUParticleBlendMode.Multiply:
                particleMaterial.shader = Shader.Find("GPUParticles/GPUParticles");
                particleMaterial.SetInt("_Src", (int)UnityEngine.Rendering.BlendMode.DstColor);
                particleMaterial.SetInt("_Dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;

            case GPUParticleBlendMode.Opaque:
                //particleMaterial.shader = Shader.Find("GPUParticles/ParticlePositionOpaque");
                break;

            case GPUParticleBlendMode.CutOff:
                //particleMaterial.shader = Shader.Find("GPUParticles/ParticlePositionStandard");
                break;

            //case GPUParticleBlendMode.SurfaceShader:
                //particleMaterial.shader = Shader.Find("GPUParticles/ParticlePositionStandard");
                //break;
        }
    }

    public void SetZBuffer()
    {
        if (useZbuffer)
        {
            particleMaterial.SetInt("_ZWrite", 1);
        }
        else
        {
            particleMaterial.SetInt("_ZWrite", 0);
        }
    }

    public void SetZBuffer(bool active)
    {
        if (active)
        {
            particleMaterial.SetInt("_ZWrite", 1);
        }
        else
        {
            particleMaterial.SetInt("_ZWrite", 0);
        }
    }

    public void SetEmitFromShell(bool active)
    {
        particleData.EmitFromShell(active);
    }

    public void SetEmitFromBase(bool active)
    {
        particleData.EmitFromBase(active);
    }

	public void SetAttractorKeyword()
	{
		if (attractors.Count > 0)
		{
			particleData.Attractors(true);
		}
		else {
			particleData.Attractors(false);
		}
	}
    #endregion

    #region TextureManagment
    public void SetUpDataTextures()
    {
        colorTexture = new Texture2D(particleColorPrecision, particleColorPrecision, TextureFormat.RGBAHalf, false);
        colorTexture.Apply(false);
    }
    #endregion
}
