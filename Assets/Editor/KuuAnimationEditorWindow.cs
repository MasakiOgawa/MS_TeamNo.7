using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class KuuAnimationEditorWindow : EditorWindow
{
    [MenuItem("Kuu/Animation/Init Animator Transition")]
    public static void InitAnimatorTransition()
    {
        int num = 0;
        foreach (Object obj in Selection.objects)
        {
            if (obj is UnityEditor.Animations.AnimatorController)
            {
                Undo.RecordObject(obj, "Init Animator Transition");
                AssetDatabase.StartAssetEditing();
                var controller = (obj as UnityEditor.Animations.AnimatorController);
                while (controller.parameters.Length > 0)
                {
                    controller.RemoveParameter(controller.parameters[0]);
                }
                List<string> triggers = new List<string>();

                for (int i = 0; i < controller.layers.Length; ++i)
                {
                    var sm = controller.layers[i].stateMachine;
                    while (sm.anyStateTransitions.Length > 0)
                    {
                        sm.RemoveAnyStateTransition(sm.anyStateTransitions[0]);
                    }


                    const string cParamName = "AnimIdx";
                    if (sm.states.Length > 0)
                    {
                        controller.AddParameter(cParamName, AnimatorControllerParameterType.Int);
                    }

                    foreach (var sta in sm.states)
                    {
                        while (sta.state.transitions.Length > 0)
                        {
                            sta.state.RemoveTransition(sta.state.transitions[0]);
                        }


                        var triggerName = sta.state.name;
                        if (triggerName.Contains("."))
                        {
                            triggerName = triggerName.Substring(0, triggerName.LastIndexOf('.'));
                        }
                        controller.AddParameter(triggerName, AnimatorControllerParameterType.Trigger);
                        var tran = sm.AddAnyStateTransition(sta.state);
                        tran.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, triggers.Count, cParamName);
                        if (sm.defaultState == sta.state)
                        {
                            controller.parameters[0].defaultInt = triggers.Count;
                        }
                        triggers.Add(triggerName);

                        tran.canTransitionToSelf = !sta.state.motion.isLooping;
                    }
                }

                num++;
                AssetDatabase.StopAssetEditing();

                if (triggers.Count > 0)
                {
                    var path = AssetDatabase.GetAssetPath(obj);
                    var className = RemoveControllerSuffix(System.IO.Path.GetFileNameWithoutExtension(path)) + "Define";
                    var directory = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(obj));
                    var fileName = className + ".cs";

                    string outStr = "";
                    outStr += "public class " + className + " {\n";
                    outStr += "\tpublic enum Idx {\n";
                    for (int i = 0; i < triggers.Count; ++i)
                    {
                        outStr += "\t\t";
                        if (i > 0) { outStr += ","; }
                        outStr += triggers[i] + " = " + i.ToString() + "\n";
                    }
                    outStr += "\t\t,SIZE = " + triggers.Count.ToString() + "\n";
                    outStr += "\t}\n";
                    outStr += "}\n";
                    System.IO.File.WriteAllText(directory + "/" + fileName, outStr, System.Text.Encoding.UTF8);
                }

                break;
            }
        }
        Debug.Log(num.ToString() + " Animator Initialized");
    }

    public static string RemoveControllerSuffix(string objName)
    {
        return System.Text.RegularExpressions.Regex.Replace(objName, "Controller", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
    }

}