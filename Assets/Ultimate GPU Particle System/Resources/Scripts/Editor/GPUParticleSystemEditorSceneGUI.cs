using UnityEngine;
using UnityEditor;
using System.Collections;

public partial class GPUParticleSystemEditor
{
    private void OnSceneGUI()
    {
        if (particleSystem == null)
            return;

        switch (particleSystem.emitterShape)
        {
            case GPUParticleSystem.EmitterShape.Edge:
                DrawEdge();
                break;
            case GPUParticleSystem.EmitterShape.Circle:
                DrawCircle();
                break;
            case GPUParticleSystem.EmitterShape.Sphere:
                DrawSphere();
                break;
            case GPUParticleSystem.EmitterShape.Box:
                DrawBox();
                break;
            case GPUParticleSystem.EmitterShape.Cone:
                DrawConeEmitter();
                break;
            case GPUParticleSystem.EmitterShape.HemiSphere:
                DrawHemiSphere();
                break;
            default:
                DrawCross();
                break;
        }

        DrawWindows();
        SceneView.RepaintAll();
    }

    private const int ControlWindowId = 1 << 0;
    private const float ControlWindowWidth = 220;
    private const float ControlWindowHeight = 120;
    private Rect ControlWindowRect;

    private void DrawWindows()
    {
        if (ControlWindowRect.width <= 0)
        {
            ControlWindowRect = new Rect(Camera.current.pixelWidth - ControlWindowWidth, (Camera.current.pixelHeight - ControlWindowHeight) / 2, ControlWindowWidth, ControlWindowHeight);
        }
        DrawControlWindow();
    }

    private void DrawControlWindow()
    {
        ControlWindowRect.x = Camera.current.pixelWidth - ControlWindowRect.width - 10f;
        ControlWindowRect.y = Camera.current.pixelHeight - Mathf.FloorToInt(ControlWindowRect.height - 5f);
        ControlWindowRect = GUI.Window(ControlWindowId, ControlWindowRect, DoControlWindow, "GPU Particle System");
    }

    private void DoControlWindow(int windowID)
    {
        //ShowControlWindow = EditorGUI.Foldout(new Rect(0, 0, 10, EditorGUIUtility.singleLineHeight), ShowControlWindow, new GUIContent("Layers"));
        EditorGUI.ProgressBar(new Rect(2f, EditorGUIUtility.singleLineHeight, ControlWindowWidth - 2f, EditorGUIUtility.singleLineHeight), particleSystem.progress, "Time " + (particleSystem.progress * 100f).ToString("f1") + "%");

        if (particleSystem.state == GPUParticleSystem.GPUParticleSystemState.Playing)
        {
            if (GUI.Button(new Rect(2f, EditorGUIUtility.singleLineHeight * 2, ControlWindowWidth / 3f, EditorGUIUtility.singleLineHeight), new GUIContent("Pause"), EditorStyles.miniButtonLeft))
            {
                particleSystem.Pause();
            }
        }
        else
        {
            if (GUI.Button(new Rect(2f, EditorGUIUtility.singleLineHeight * 2, ControlWindowWidth / 3f, EditorGUIUtility.singleLineHeight), new GUIContent("Play"), EditorStyles.miniButtonLeft))
            {
                particleSystem.Play();
            }
        }

        if (GUI.Button(new Rect(ControlWindowWidth / 3f - 2f, EditorGUIUtility.singleLineHeight * 2, ControlWindowWidth / 3f, EditorGUIUtility.singleLineHeight), new GUIContent("Restart"), EditorStyles.miniButtonMid))
            particleSystem.Restart();

        if (GUI.Button(new Rect(ControlWindowWidth / 1.5f - 2f, EditorGUIUtility.singleLineHeight * 2, ControlWindowWidth / 3f, EditorGUIUtility.singleLineHeight), new GUIContent("Stop"), EditorStyles.miniButtonRight))
            particleSystem.Stop();

        if (GUI.Button(new Rect(2f, EditorGUIUtility.singleLineHeight * 3, ControlWindowWidth / 5f, EditorGUIUtility.singleLineHeight), new GUIContent("5"), EditorStyles.miniButtonLeft))
            particleSystem.Emit(5);
        
        if (GUI.Button(new Rect(ControlWindowWidth / 5f * 1f, EditorGUIUtility.singleLineHeight * 3, ControlWindowWidth / 5f, EditorGUIUtility.singleLineHeight), new GUIContent("50"), EditorStyles.miniButtonMid))
            particleSystem.Emit(50);
        
        if (GUI.Button(new Rect(ControlWindowWidth / 5f * 2f, EditorGUIUtility.singleLineHeight * 3, ControlWindowWidth / 5f, EditorGUIUtility.singleLineHeight), new GUIContent("500"), EditorStyles.miniButtonMid))
            particleSystem.Emit(500);

		if (GUI.Button(new Rect(ControlWindowWidth / 5f * 3f, EditorGUIUtility.singleLineHeight * 3, ControlWindowWidth / 5f, EditorGUIUtility.singleLineHeight), new GUIContent("5k"), EditorStyles.miniButtonMid))
			particleSystem.Emit(5000);

		if (GUI.Button(new Rect(ControlWindowWidth / 5f * 4f - 2f, EditorGUIUtility.singleLineHeight * 3, ControlWindowWidth / 5f, EditorGUIUtility.singleLineHeight), new GUIContent("50k"), EditorStyles.miniButtonRight))
            particleSystem.Emit(50000);
            
        GUI.DragWindow();
    }
}
