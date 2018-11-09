using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public partial class GPUParticleSystem : MonoBehaviour
{
    public void OnEnable()
    {
#if UNITY_EDITOR
		CheckInstanceID();

		if (!Application.isPlaying)
        {
            EditorApplication.update += Update;
        }
#endif
		PrepareParticleData();
		MakeMeshes();
		Initialize();
		SetRenderQueue();

		if (playOnAwake)
		{
			state = GPUParticleSystemState.Playing;
		}
		else
		{
			state = GPUParticleSystemState.Stopped;
		}
	}

    private void OnDisable()
    {
		ClearParticleData();
		ClearMeshes();

#if UNITY_EDITOR
		if (!Application.isPlaying)
        {
            EditorApplication.update -= Update;
        }
#endif
    }

#if UNITY_EDITOR
	private void CheckInstanceID()
	{
		if (instanceID == -1)
		{
			particleMaterial = new Material(Shader.Find("GPUParticles/GPUParticles"));
			instanceID = gameObject.GetInstanceID();
		}
	}
#endif

	private void Awake()
    {
		ReSeed();

        if (playOnAwake)
        {
            Play();
        }
    }
	
    private void Update()
    {
        if (state == GPUParticleSystemState.Playing)
        {
            UpdateCustomTime();
            InternalEmit();
            InternalStep();
        }
	}

    private void UpdateCustomTime()
    {
        if (Application.isPlaying)
        {
            if (useFixedDeltaTime)
            {
                customDeltaTime = fixedDeltaTime;
            }
            else {
                customDeltaTime = Time.smoothDeltaTime;
            }
        }
        else
		{
            if (useFixedDeltaTime)
            {
                customDeltaTime = fixedDeltaTime;
            }
            else
            {
                customDeltaTime = Time.realtimeSinceStartup - previousFrameTime;//Fixed Delta Time at 60FPS
                customDeltaTime = Mathf.Clamp(customDeltaTime, 0f, 1.667f);
            }
        }

        previousFrameTime = Time.realtimeSinceStartup;

        customTime += customDeltaTime;

        UpdateTimeInMaterials();
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        progress = Mathf.Clamp01((customTime - effectStartTime) / effectLength);

        if (progress == 1f && loop)
        {
            effectStartTime = customTime;
            emit = true;
        }

        if (progress == 1f && !loop)
        {
            emit = false;
        } 
    }

	public void SetRenderQueue()
	{
		particleMaterial.renderQueue = renderQueue;
	}

	#region API
	public void Play()
    {
        state = GPUParticleSystemState.Playing;
        effectStartTime = customTime = Time.realtimeSinceStartup;
    }

    public void Pause()
    {
        state = GPUParticleSystemState.Paused;
	}

    public void Stop()
    {
        state = GPUParticleSystemState.Stopped;
        particleData.Reset();

		previousFrameTime = 0f;
		customTime = 0f;
		startID = 0f;
        endID = 0f;
        progress = 0f;
    }

    public void Restart()
    {
        Stop();
        Play();
    }

    public void Emit(int numParticles)
    {
        EmitNumParticles(numParticles);
    }

    public void UpdateEmitter()
    {
        Vector4 emitterPosition = new Vector4(transform.position.x, transform.position.y, transform.position.z, 1);

        if (simulationSpace == GPUParticleSystem.GPUSimulationSpace.Local)
        {
            emitterPosition =  Vector3.zero;
        }

        param4 = Mathf.Tan(param2 * Mathf.Deg2Rad) * param3;
        Vector4 emitterParameters = new Vector4(param1, param2, param3, param1 + param4);

        particleData.UpdateEmitterParameters(emitterPosition, emitterParameters);
    }

    public void ReSeed()
    {
        emissionRate.seed = Random.Range(0f,1f);
        startSpeed.seed = Random.Range(0f, 1f);
        startLifetime.seed = Random.Range(0f, 1f);
        startSize.seed = Random.Range(0f, 1f);
        startRotation.seed = Random.Range(0f, 1f);
        sizeOverLifetime.seed = Random.Range(0f, 1f);
        rotationOverLifetime.seed = Random.Range(0f, 1f);
        colorOverLifetime.seed = Random.Range(0f, 1f);
    }
    #endregion

    /*
    private void OnGUI()
    {
        if(particleData.positionBuffer_1 != null)
            GUI.DrawTexture(new Rect(0f,0f, skinnedMeshEmitterPositionTexture.width, skinnedMeshEmitterPositionTexture.height), particleData.metaBuffer_1);
    }
    */
}
