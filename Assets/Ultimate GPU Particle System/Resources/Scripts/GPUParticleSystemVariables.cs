using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GPUParticleSystem
{
    #region Constants
    private const int particleColorPrecision = 256;
	private Vector3 safetyVector = new Vector3(0.01f,0f,0f);
    #endregion

    #region Enums
    public enum MeshBakeType {
        Vertex, Edge, Triangle
    };

    public enum GPUParticleBlendMode
    {
        Alpha, Additive, Screen, Premultiplied, Subtractive, Multiply, CutOff, Opaque//, SurfaceShader
    };

    public enum CurveMode
    {
        Off, Linear, Smooth, Curve, RandomTwoCurves
    };

    public enum GPUParticleSystemState {
        Uninitialized, Paused, Stopped, Playing
    };

    public enum EmitterShape {
        Point, Edge, Circle, Box, HemiSphere, Sphere, Cone//, Mesh, MeshFilter, SkinnedMeshRenderer
    };

    public enum ParticleType {
        Point, Triangle, Billboard, HorizontalBillboard, VerticalBillboard, StretchedTail, StretchedBillboard//, Mesh, AnimatedMesh
    };

    public enum TurbulenceType
    {
        Off, Texture, VectorField
    };

    public enum GPUSimulationSpace
    {
        Local, World
    };

    public enum ValueMode {
        Value, RandomTwoValues, Curve, RandomTwoCurves
    };

	public enum ColorMode
	{
		Color, RandomTwoColors, Gradient, RandomTwoGradients
	};

	public enum SimpleValueMode
    {
        Value, Curve
    };

    public enum TextureSheetMode
    {
        Off, TextureSheet, TextureSheetMotionVectors
    };
    
    public enum RenderTexturePrecision
    {
        Half, Float
    };
	#endregion

	#region Core
	private int instanceID = -1;
	public GPUParticleSystemBuffer particleData;
    public Material particleMaterial;
	public int renderQueue = 3000;
    private GameObject[] particleMeshes;
    public int bufferWidth = 256;
    public int bufferHeight = 256;
    private float customTime = 0f;
    private float customDeltaTime = 0f;
    private float previousFrameTime = 0f;
    public RenderTexturePrecision precision = RenderTexturePrecision.Float;
    public bool useFixedDeltaTime = false;
    public float fixedDeltaTime = 0.016666666f;
	private int burstNum = 0;
    #endregion

    #region Spawning
    private float startID = 0f;
    private float endID = 0f;
	#endregion

	#region General
	/*[System.NonSerialized]*/public GPUParticleSystemState state = GPUParticleSystemState.Uninitialized;
    public bool playOnAwake = true;
    public bool loop = true;
    public bool emit = true;
    private float effectStartTime = 0f;
    public float effectLength = 2f;
    [System.NonSerialized]public float progress = 0f;
    #endregion

    #region Emitter
    public EmitterShape emitterShape = EmitterShape.Cone;
                                                //Cone      //Box       //Sphere    //Circle    //Edge      //HemiSphere
    public float param1 = 1f;                   //Radius1   //X Size    //Radius    //Radius    //Length    //Radius
    public float param2 = 25f;                  //Angle     //Y Size    
    public float param3 = 5f;                   //Length    //Z Size
    public float param4 = 1f;                   //Radius2
    public float randomness = 0f;
    public bool emitFromShell = false;
    public bool emitFromBase = true;            //Only Cone
	public GPUSimulationSpace simulationSpace = GPUSimulationSpace.World;

	//Mesh Emitter
	public Mesh originalMesh;
	public Mesh meshEmitter;
	public MeshFilter meshFilterEmitter;
	public int meshEmitterResolution = 16;
	public MeshBakeType meshBakeType = MeshBakeType.Vertex;
	public Texture2D meshEmitterPositionTexture;
	//public Texture2D 

	//Skinned mesh emitter
	//public SkinnedMeshRenderer skinnedMeshRendererEmitter;
	//public Transform skinnedMeshEmitterTransform;
	//public Camera skinnedMeshEmitterCam;
    //private Material skinnedMeshEmitterMaterial;
	//private RenderTexture skinnedMeshEmitterPositionTexture;
	#endregion

	#region Emission
	public FloatCurveBundle emissionRate = new FloatCurveBundle(2500f, 2500f);
    public int maxParticles = 65536;
    #endregion

    #region StartValues
    public FloatCurveBundle startLifetime = new FloatCurveBundle(5f, 5f);
    public FloatCurveBundle startSize = new FloatCurveBundle(.1f, .22f);
	public FloatCurveBundle startSpeed = new FloatCurveBundle(5f,5f);
	public FloatCurveBundle startRotation = new FloatCurveBundle(0f,0f);
	public bool useRotation = false;
	#endregion

	#region LifetimeValues
	public ShaderCurveBundle sizeOverLifetime = new ShaderCurveBundle();
    public ShaderCurveBundle rotationOverLifetime = new ShaderCurveBundle();
	public ColorGradientBundle colorOverLifetime = new ColorGradientBundle();
	public SingleFloatCurveBundle colorIntensityOverLifetime = new SingleFloatCurveBundle(1f);
	public SingleFloatCurveBundle maxVelocity = new SingleFloatCurveBundle(25f);
	public bool useMaxVelocity = false;
    private Texture2D colorTexture;
    #endregion

    #region ParticleShape
    public ParticleType particleType = ParticleType.Triangle;
    #endregion

    #region ParticleMesh
    public Mesh meshParticle;
	#endregion

	#region Forces
	public SingleFloatCurveBundle gravity = new SingleFloatCurveBundle(0f);
	private Vector3 previousEmitterPosition = Vector3.zero;
	private Vector3 emitterVelocity = Vector3.zero;
	public SingleFloatCurveBundle inheritVelocityMultiplyer = new SingleFloatCurveBundle(0f);
    public bool useInheritVelocity = false;
	public SingleFloatCurveBundle airResistance = new SingleFloatCurveBundle(0f);
	public Vector3CurveBundle forceOverLifetime = new Vector3CurveBundle();
    public bool useCircularForce = false;
	public Vector3CurveBundle circularForce = new Vector3CurveBundle();
    [SerializeField] public Transform circularForceCenter;
    #endregion

    #region Turbulence
    public TurbulenceType turbulenceType = TurbulenceType.Off;
    public TextAsset fgaFile;
    public Texture3D vectorField;
    public Texture2D vectorNoise;
	public float Tightness;
	public Vector3CurveBundle turbulenceAmplitude = new Vector3CurveBundle();
    public Vector3CurveBundle turbulenceFrequency = new Vector3CurveBundle();
	public Vector3CurveBundle turbulenceOffset = new Vector3CurveBundle();
	public Vector3CurveBundle turbulenceRotation = new Vector3CurveBundle();
	[SerializeField] private Transform vectorFieldObject;
	#endregion

	#region Attractors
	public List<AttractorListElement> attractors = new List<AttractorListElement>();
	#endregion

	#region Rendering
	public Texture2D mainTexture;
	public TextureSheetMode textureSheetMode = TextureSheetMode.Off;
    public bool textureSheetRandomIndex = false;
    public GPUParticleBlendMode blendMode = GPUParticleBlendMode.Additive;
    public bool useZbuffer = false;
    public float stretchMultiplier = 1f;
	#endregion

	#region Editor
#if UNITY_EDITOR
	public bool GeneralsTab = true;
	public bool EmitterTab = false;
	public bool EmissionTab = false;
	public bool StartValuesTab = false;
	public bool LifetimeValuesTab = false;
	public bool ForcesTab = false;
	public bool TurbulenceTab = false;
	public bool AttractorsTab = false;
	public bool RenderingTab = false;
	public bool MaterialTab = false;
#endif
	#endregion

	#region Debug
	public void DebugOn()
	{
		foreach (GameObject g in Object.FindObjectsOfType(typeof(GameObject)))
		{
			g.hideFlags = HideFlags.None;
		}

		if (circularForceCenter != null)
			circularForceCenter.gameObject.hideFlags = HideFlags.None;

		if (vectorFieldObject != null)
			vectorFieldObject.gameObject.hideFlags = HideFlags.None;
	}

	public void DebugOff()
	{
		for (int i = 0; i < particleMeshes.Length; i++)
		{
			particleMeshes[i].hideFlags = HideFlags.HideInHierarchy;
		}

		if (circularForceCenter != null)
			circularForceCenter.gameObject.hideFlags = HideFlags.HideInHierarchy;

		if (vectorFieldObject != null)
			vectorFieldObject.gameObject.hideFlags = HideFlags.HideInHierarchy;
	}
	#endregion
}
