using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditorInternal;

[CustomEditor(typeof(GPUParticleSystem))]
public partial class GPUParticleSystemEditor : Editor
{
    GPUParticleSystem particleSystem;

	#region Properties
#pragma warning disable 414
	SerializedProperty playOnAwake;
    SerializedProperty loop;
    SerializedProperty effectLength;
    SerializedProperty maxParticles;
    SerializedProperty bufferWidth;
    SerializedProperty bufferHeight;
    SerializedProperty useFixedDeltaTime;
    SerializedProperty fixedDeltaTime;

	SerializedProperty param1;
    SerializedProperty param2;
    SerializedProperty param3;
    SerializedProperty param4;
    SerializedProperty randomness;
    SerializedProperty emitFromShell;
    SerializedProperty emitFromBase;

    SerializedProperty useRotation;
    SerializedProperty useMaxVelocity;
    SerializedProperty useCircularForce;
    SerializedProperty circularForceCenter;
	SerializedProperty useInheritVelocity;

	SerializedProperty gravity;
    SerializedProperty inheritVelocity;
    SerializedProperty airResistance;

    SerializedProperty mainTexture;
    SerializedProperty useZbuffer;

    SerializedProperty fgaFile;
    SerializedProperty vectorNoise;
    SerializedProperty vectorField;

	//Enums
	SerializedProperty particleType;
	SerializedProperty blendMode;
	SerializedProperty simulationSpace;
	SerializedProperty emitterShape;
	SerializedProperty precision;
	SerializedProperty turbulenceType;
	SerializedProperty tightness;
	//Editor
	SerializedProperty GeneralsTab;
	SerializedProperty EmitterTab;
	SerializedProperty EmissionTab;
	SerializedProperty StartValuesTab;
	SerializedProperty LifetimeValuesTab;
	SerializedProperty ForcesTab;
	SerializedProperty TurbulenceTab;
	SerializedProperty AttractorsTab;
	SerializedProperty RenderingTab;
	SerializedProperty MaterialTab;

	SerializedProperty renderQueue;

	ReorderableList attractors;

#pragma warning restore 414
	#endregion

	[MenuItem("GameObject/Effects/GPU Particle System", false, 2000)]
    public static void NewGPUParticleSystem()
    {
        GameObject g = new GameObject("GPU Particle System");
        g.AddComponent<GPUParticleSystem>();
        g.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }

    private void OnEnable()
    {
        playOnAwake = serializedObject.FindProperty("playOnAwake");
        loop = serializedObject.FindProperty("loop");
        effectLength = serializedObject.FindProperty("effectLength");
        maxParticles = serializedObject.FindProperty("maxParticles");
        bufferWidth = serializedObject.FindProperty("bufferWidth");
        bufferHeight = serializedObject.FindProperty("bufferHeight");
        useFixedDeltaTime = serializedObject.FindProperty("useFixedDeltaTime");
        fixedDeltaTime = serializedObject.FindProperty("fixedDeltaTime");

        param1 = serializedObject.FindProperty("param1");
        param2 = serializedObject.FindProperty("param2");
        param3 = serializedObject.FindProperty("param3");
        param4 = serializedObject.FindProperty("param4");
        randomness = serializedObject.FindProperty("randomness");
        emitFromShell = serializedObject.FindProperty("emitFromShell");
        emitFromBase = serializedObject.FindProperty("emitFromBase");

        useRotation = serializedObject.FindProperty("useRotation");
        useMaxVelocity = serializedObject.FindProperty("useMaxVelocity");
        useCircularForce = serializedObject.FindProperty("useCircularForce");
		circularForceCenter = serializedObject.FindProperty("circularForceCenter");
		useInheritVelocity = serializedObject.FindProperty("useInheritVelocity");
		
		gravity = serializedObject.FindProperty("gravity");
        inheritVelocity = serializedObject.FindProperty("inheritVelocity");
        airResistance = serializedObject.FindProperty("airResistance");

        mainTexture = serializedObject.FindProperty("mainTexture");
        useZbuffer = serializedObject.FindProperty("useZbuffer");

        vectorNoise = serializedObject.FindProperty("vectorNoise");
        fgaFile = serializedObject.FindProperty("fgaFile");
        vectorField = serializedObject.FindProperty("vectorField");

		GeneralsTab = serializedObject.FindProperty("GeneralsTab");
		EmitterTab = serializedObject.FindProperty("EmitterTab");
		EmissionTab = serializedObject.FindProperty("EmissionTab");
		StartValuesTab = serializedObject.FindProperty("StartValuesTab");
		LifetimeValuesTab = serializedObject.FindProperty("LifetimeValuesTab");
		ForcesTab = serializedObject.FindProperty("ForcesTab");
		TurbulenceTab = serializedObject.FindProperty("TurbulenceTab");
		tightness = serializedObject.FindProperty("Tightness");

		AttractorsTab = serializedObject.FindProperty("AttractorsTab");
		RenderingTab = serializedObject.FindProperty("RenderingTab");
		MaterialTab = serializedObject.FindProperty("MaterialTab");

		particleType = serializedObject.FindProperty("particleType");
		blendMode = serializedObject.FindProperty("blendMode");
		simulationSpace = serializedObject.FindProperty("simulationSpace");
		emitterShape = serializedObject.FindProperty("emitterShape");
		precision = serializedObject.FindProperty("precision");
		turbulenceType = serializedObject.FindProperty("turbulenceType");

		renderQueue = serializedObject.FindProperty("renderQueue");

		attractors = new ReorderableList(serializedObject,
								   serializedObject.FindProperty("attractors"),
								   true, true, true, true);

		attractors.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
		{
			var element = attractors.serializedProperty.GetArrayElementAtIndex(index);

			rect.y += 2;

			float elementWidth = Screen.width / 3f - 22f;

			element.FindPropertyRelative("attractorPosition").objectReferenceValue = EditorGUI.ObjectField(
				new Rect(rect.x, rect.y, elementWidth, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("attractorPosition").objectReferenceValue, typeof(Transform), true);

			element.FindPropertyRelative("attractionStrength").floatValue = EditorGUI.FloatField(
				new Rect(rect.x + elementWidth, rect.y, elementWidth, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("attractionStrength").floatValue);

			element.FindPropertyRelative("attenuation").floatValue = EditorGUI.FloatField(
				new Rect(rect.x + 2f * elementWidth, rect.y, elementWidth, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("attenuation").floatValue);

			if (GUI.Button(new Rect(rect.x + 3f * elementWidth, rect.y, 22f, EditorGUIUtility.singleLineHeight), "+", EditorStyles.toolbarButton))
			{
				GameObject g = new GameObject("New attractor");
				element.FindPropertyRelative("attractorPosition").objectReferenceValue = g.transform;
			}
		};
		
		attractors.drawHeaderCallback = (Rect rect) =>
		{
			float elemetWidth = Screen.width / 3f - 12f;

			EditorGUI.LabelField(
				new Rect(rect.x, rect.y, elemetWidth, EditorGUIUtility.singleLineHeight),
				"Transform", EditorStyles.boldLabel);

			EditorGUI.LabelField(
				new Rect(rect.x + elemetWidth + 5f, rect.y, elemetWidth, EditorGUIUtility.singleLineHeight),
				"Strength", EditorStyles.boldLabel);

			EditorGUI.LabelField(
				new Rect(rect.x + 2f * elemetWidth + 5f, rect.y, elemetWidth, EditorGUIUtility.singleLineHeight),
				"Attenuation", EditorStyles.boldLabel);
		};
	}

    public override void OnInspectorGUI()
    {
        particleSystem = (GPUParticleSystem)target;

		if (particleSystem.gameObject.scene.name == null)
		{
			EditorGUILayout.HelpBox("Can't edit prefabs at this point!", MessageType.Info);
			return;
		}

		#region Bugreporter
		EditorGUILayout.BeginVertical("Box");
		{
			EditorGUILayout.LabelField("BETA - Bug reporter", EditorStyles.boldLabel);
			EditorGUILayout.HelpBox("This tool is currently in Beta version 0.1b. If you experience any bugs or have suggestions on how to improve the functionality or want to request a feature, please use the button below to report it." +
				" Upcoming features, fixes and known issues can be found on the Roadmap.", MessageType.Info);

			EditorGUILayout.BeginHorizontal();
			{
				/*
				if (GUILayout.Button("Report a problem", EditorStyles.toolbarButton))
				{
					GPUParticleSystemBugReporter.ShowWindow();
				}

				if (GUILayout.Button("Help", EditorStyles.toolbarButton))
				{
					GPUParticleSystemHelperWindow.ShowWindow();
				}
				*/
				if (GUILayout.Button("Roadmap", EditorStyles.toolbarButton))
				{
					Application.OpenURL("https://docs.google.com/spreadsheets/d/16KmluoemHBrA_BGpw8YBms8Ib7YX7rQSAvvCeVgSmPc/edit?usp=sharing");
				}

				if (GUILayout.Button("Recreate", EditorStyles.toolbarButton))
				{
					particleSystem.ForceRecreateParticles();
				}

				if (GUILayout.Button("Debug on", EditorStyles.toolbarButton))
				{
					particleSystem.DebugOn();
					EditorApplication.RepaintHierarchyWindow();
				}

				if (GUILayout.Button("Debug off", EditorStyles.toolbarButton))
				{
					particleSystem.DebugOff();
					EditorApplication.RepaintHierarchyWindow();
				}
			}
			EditorGUILayout.EndHorizontal();
			/*
			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Recreate", EditorStyles.toolbarButton))
				{
					particleSystem.ForceRecreateParticles();
				}

				if (GUILayout.Button("Debug on", EditorStyles.toolbarButton))
				{
					particleSystem.DebugOn();
					EditorApplication.RepaintHierarchyWindow();
				}

				if (GUILayout.Button("Debug off", EditorStyles.toolbarButton))
				{
					particleSystem.DebugOff();
					EditorApplication.RepaintHierarchyWindow();
				}
			}
			EditorGUILayout.EndHorizontal();
			*/
		}
		EditorGUILayout.EndVertical();
		#endregion

		#region GeneralsTab
		if (GUILayout.Button("General", EditorStyles.toolbarDropDown))
		{
			GeneralsTab.boolValue = !GeneralsTab.boolValue;
		}

		if (GeneralsTab.boolValue == true)
		{
			GUILayout.Space(-4f);
			EditorGUILayout.BeginVertical("Box");
			{
				effectLength.floatValue = EditorGUILayout.FloatField("Effect length", effectLength.floatValue);
				loop.boolValue = DrawOnOffToggle(loop.boolValue, "Loop");
				playOnAwake.boolValue = DrawOnOffToggle(playOnAwake.boolValue, "Play on awake");

				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(simulationSpace);

				if (EditorGUI.EndChangeCheck())
				{
					particleSystem.SetSimulationSpace();
				}
				GUILayout.Space(15f);

				EditorGUI.BeginChangeCheck();
				bufferWidth.intValue = EditorGUILayout.DelayedIntField("Buffer Width", bufferWidth.intValue);
				bufferHeight.intValue = EditorGUILayout.DelayedIntField("Buffer Height", bufferHeight.intValue);
				EditorGUILayout.LabelField("Max Particles:", maxParticles.intValue.ToString());

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.PrepareParticleData();
					particleSystem.ForceRecreateParticles();
				}

				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(precision);

				if (EditorGUI.EndChangeCheck())
				{
					particleSystem.ForceRecreateParticles();
				}

				useFixedDeltaTime.boolValue = DrawOnOffToggle(useFixedDeltaTime.boolValue, "Use fixed delta time");
				EditorGUI.BeginDisabledGroup(!useFixedDeltaTime.boolValue);
				{
					fixedDeltaTime.floatValue = EditorGUILayout.FloatField("Fixed delta time", fixedDeltaTime.floatValue);
				}
				EditorGUI.EndDisabledGroup();
			}
			EditorGUILayout.EndHorizontal();
		}

		#endregion

		#region EmitterTab
		if (GUILayout.Button("Emitter", EditorStyles.toolbarDropDown))
		{
			EmitterTab.boolValue = !EmitterTab.boolValue;
		}

		if (EmitterTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(emitterShape);

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.SetEmitterShapeKeyword();
				}

				DrawEmitterOptions(particleSystem.emitterShape);
			}
			EditorGUILayout.EndHorizontal();
		}

		#endregion

		#region EmissionTab
		if (GUILayout.Button("Emission", EditorStyles.toolbarDropDown))
		{
			EmissionTab.boolValue = !EmissionTab.boolValue;
		}

		if (EmissionTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUI.BeginChangeCheck();
				DrawFloatCurveBundel(particleSystem.emissionRate, "emissionRate", "Rate");

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		#endregion

		#region StartValuesTab
		if (GUILayout.Button("Start values", EditorStyles.toolbarDropDown))
		{
			StartValuesTab.boolValue = !StartValuesTab.boolValue;
		}

		if (StartValuesTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUI.BeginChangeCheck();
				DrawFloatCurveBundel(particleSystem.startSpeed, "startSpeed", "Start speed");
				DrawFloatCurveBundel(particleSystem.startLifetime, "startLifetime", "Start lifetime");

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.UpdateStartSpeedAndLifetime();
				}

				EditorGUI.BeginChangeCheck();
				DrawFloatCurveBundel(particleSystem.startSize, "startSize", "Start size");

				GUILayout.Space(5f);

				EditorGUILayout.BeginVertical();
				{
					EditorGUI.BeginChangeCheck();
					useRotation.boolValue = DrawOnOffToggleBundle(useRotation.boolValue, "Enable Rotation");

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.SetRotationKeyword();
					}

					if(useRotation.boolValue)
					{
						DrawFloatCurveBundel(particleSystem.startRotation, "startRotation", "Start rotation");
					}

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateStartSizeAndRotation();
					}
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();
		}

		#endregion

		#region LifetimeValuesTab
		if (GUILayout.Button("Lifetime values", EditorStyles.toolbarDropDown))
		{
			LifetimeValuesTab.boolValue = !LifetimeValuesTab.boolValue;
		}

		if (LifetimeValuesTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUI.BeginChangeCheck();
				DrawShaderCurveBundel(particleSystem.sizeOverLifetime, "sizeOverLifetime", "Size over lifetime");
				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.UpdateSizeOverLifetime();
				}

				EditorGUI.BeginDisabledGroup(!useRotation.boolValue);
				{
					EditorGUI.BeginChangeCheck();
					DrawShaderCurveBundel(particleSystem.rotationOverLifetime, "rotationOverLifetime", "Rotation over lifetime");

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateRotationOverLifetime();
					}
				}
				EditorGUI.EndDisabledGroup();

				EditorGUI.BeginChangeCheck();
				DrawColorGradientBundel(particleSystem.colorOverLifetime, "colorOverLifetime", "Color over lifetime");

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.UpdateColorOverLifeTime();
				}

				EditorGUI.BeginChangeCheck();
				DrawSingleFloatCurveBundel(particleSystem.colorIntensityOverLifetime, "colorIntensityOverLifetime", "Color intensity over lifetime");

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.UpdateColorIntensity();
				}

				GUILayout.Space(5f);
				
					EditorGUI.BeginChangeCheck();
					useMaxVelocity.boolValue = DrawOnOffToggleBundle(useMaxVelocity.boolValue, "Enable max velocity");

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.SetLimitVelocity();
					}
				
					EditorGUI.BeginDisabledGroup(!useMaxVelocity.boolValue);
					{
						EditorGUI.BeginChangeCheck();
						DrawSingleFloatCurveBundel(particleSystem.maxVelocity, "maxVelocity", "Max velocity");

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateMaxVelocityOverLifetime();
						}
					}
					EditorGUI.EndDisabledGroup();
				
			}
			EditorGUILayout.EndHorizontal();
		}
		#endregion

		#region Forces
		if (GUILayout.Button("Forces", EditorStyles.toolbarDropDown))
		{
			ForcesTab.boolValue = !ForcesTab.boolValue;
		}

		if (ForcesTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUI.BeginChangeCheck();
				DrawSingleFloatCurveBundel(particleSystem.gravity, "gravity", "Gravity");
				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.UpdateGravity();
				}

				EditorGUI.BeginChangeCheck();
				DrawSingleFloatCurveBundel(particleSystem.airResistance, "airResistance", "Air resistance (drag)");
				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.UpdateAirResistance();
				}

				EditorGUI.BeginChangeCheck();
				DrawVector3CurveBundel(particleSystem.forceOverLifetime, "forceOverLifetime", "Force over lifetime");
				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.UpdateForceOverLifetime();
				}

				GUILayout.Space(5f);

				EditorGUI.BeginChangeCheck();
				useInheritVelocity.boolValue = DrawOnOffToggleBundle(useInheritVelocity.boolValue, "Enable inherit velocity");

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
				}

				EditorGUI.BeginDisabledGroup(!useInheritVelocity.boolValue);
				{
					EditorGUI.BeginChangeCheck();
					DrawSingleFloatCurveBundel(particleSystem.inheritVelocityMultiplyer, "inheritVelocityMultiplyer", "Inherit velocity");
					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
					}
				}
				EditorGUI.EndDisabledGroup();

				GUILayout.Space(5f);

				EditorGUI.BeginChangeCheck();
				useCircularForce.boolValue = DrawOnOffToggleBundle(useCircularForce.boolValue, "Enable circular force");

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.SetCircularForce();
				}

				EditorGUI.BeginDisabledGroup(!useCircularForce.boolValue);
				{
					EditorGUI.BeginChangeCheck();
					DrawVector3CurveBundel(particleSystem.circularForce, "circularForce", "Circular force");

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateCircularForceOverLifetime();
					}

					circularForceCenter.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Center", "Assign a transform or use default."), circularForceCenter.objectReferenceValue, typeof(Transform), false);
				}
				EditorGUI.EndDisabledGroup();

			}
			EditorGUILayout.EndVertical();
		}
		#endregion

		#region Turbulence
		if (GUILayout.Button("Turbulence", EditorStyles.toolbarDropDown))
		{
			TurbulenceTab.boolValue = !TurbulenceTab.boolValue;
		}

		if (TurbulenceTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(turbulenceType);

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.SetTurbulenceKeyword();

					if (particleSystem.turbulenceType == GPUParticleSystem.TurbulenceType.Texture)
					{
						particleSystem.UpdateTurbulenceTexture();
					}

					if (particleSystem.turbulenceType == GPUParticleSystem.TurbulenceType.VectorField)
					{
						particleSystem.UpdateVectorfieldFile();
						particleSystem.UpdateVectorField();
					}
				}

				if (particleSystem.turbulenceType == GPUParticleSystem.TurbulenceType.Texture)
				{
					EditorGUI.BeginChangeCheck();
					vectorNoise.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Noise (RGBA)", "Assign a texture to enable."), vectorNoise.objectReferenceValue, typeof(Texture2D), false);

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateTurbulenceTexture();
					}
					EditorGUI.BeginChangeCheck();
					DrawVector3CurveBundel(particleSystem.turbulenceAmplitude, "turbulenceAmplitude", "Amplitude");
					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateAmplitude();
					}
					EditorGUI.BeginChangeCheck();
					DrawVector3CurveBundel(particleSystem.turbulenceFrequency, "turbulenceFrequency", "Frequency");
					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateFrequency();
					}
					EditorGUI.BeginChangeCheck();
					DrawVector3CurveBundel(particleSystem.turbulenceOffset, "turbulenceOffset", "Offset (speed)");
					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						particleSystem.UpdateOffset();
					}
				}

				if (particleSystem.turbulenceType == GPUParticleSystem.TurbulenceType.VectorField)
				{
					if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android ||
						EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS ||
						EditorUserBuildSettings.activeBuildTarget == BuildTarget.WebGL)
					{
						EditorGUILayout.HelpBox("Vector field turbulence is not supported on your current build target.", MessageType.Warning);
					}

					EditorGUI.BeginChangeCheck();
					fgaFile.objectReferenceValue = EditorGUILayout.ObjectField("Vectorfield file (fga)", fgaFile.objectReferenceValue, typeof(TextAsset), false);
					if (EditorGUI.EndChangeCheck())
					{
						particleSystem.UpdateVectorfieldFile();
						particleSystem.UpdateVectorField();
					}

					EditorGUI.BeginDisabledGroup(true);
					{
						EditorGUI.BeginChangeCheck();
						vectorField.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Noise (RGBA)", "Assign a texture to enable."), vectorField.objectReferenceValue, typeof(Texture3D), false);

						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateVectorField();
						}
					}
					EditorGUI.EndDisabledGroup();

					EditorGUILayout.BeginVertical("Box");
					{
						EditorGUI.BeginChangeCheck();
						DrawVector3CurveBundel(particleSystem.turbulenceAmplitude, "turbulenceAmplitude", "Amplitude");
						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateAmplitude();
						}

						EditorGUI.BeginChangeCheck();
						DrawVector3CurveBundel(particleSystem.turbulenceFrequency, "turbulenceFrequency", "Frequency");
						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateFrequency();
						}

						EditorGUI.BeginChangeCheck();
						DrawVector3CurveBundel(particleSystem.turbulenceRotation, "turbulenceRotation", "Rotation");
						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateVectorFieldMatrix();
						}

						EditorGUI.BeginChangeCheck();
						tightness.floatValue = EditorGUILayout.Slider("Tightness", tightness.floatValue, 0f, 1f);
						if (EditorGUI.EndChangeCheck())
						{
							serializedObject.ApplyModifiedProperties();
							particleSystem.UpdateTightness();
						}
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();
		}
		#endregion

		#region Attractors
		if (GUILayout.Button("Attractors", EditorStyles.toolbarDropDown))
		{
			AttractorsTab.boolValue = !AttractorsTab.boolValue;
		}

		if (AttractorsTab.boolValue == true)
		{
			//GUILayout.Space(-4f);

			EditorGUI.BeginChangeCheck();
			attractors.DoLayoutList();

			if (EditorGUI.EndChangeCheck())
				particleSystem.SetAttractorKeyword();
			
		}
		#endregion

		#region Rendering
		if (GUILayout.Button("Rendering", EditorStyles.toolbarDropDown))
		{
			RenderingTab.boolValue = !RenderingTab.boolValue;
		}

		if (RenderingTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{

				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(particleType);

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.ForceRecreateParticles();
					particleSystem.SetParticleTypeKeyword(particleSystem.particleType);
					
				}

				DrawParticleTypeOptions(particleSystem.particleType);

				GUILayout.Space(15f);
			}
			EditorGUILayout.EndVertical();
		}
		#endregion

		#region Material
		if (GUILayout.Button("Material options", EditorStyles.toolbarDropDown))
		{
			MaterialTab.boolValue = !MaterialTab.boolValue;
		}

		if (MaterialTab.boolValue == true)
		{
			GUILayout.Space(-4f);

			EditorGUILayout.BeginVertical("Box");
			{
				EditorGUI.BeginChangeCheck();
				useZbuffer.boolValue = DrawOnOffToggle(useZbuffer.boolValue, "Z-Buffer");

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.SetZBuffer();
				}

				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(blendMode);

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.SetBlendMode();
				}

				EditorGUI.BeginChangeCheck();
				mainTexture.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Main Texture", "Assign a texture to enable."), mainTexture.objectReferenceValue, typeof(Texture2D), false);

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.SetMainTexture();
				}

				EditorGUI.BeginChangeCheck();
				renderQueue.intValue = EditorGUILayout.IntField("Render Queue", renderQueue.intValue);

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					particleSystem.SetRenderQueue();
				}
			}
			EditorGUILayout.EndVertical();
		}
		#endregion

		GUILayout.Space(10f);

		serializedObject.ApplyModifiedProperties();
    }
}
