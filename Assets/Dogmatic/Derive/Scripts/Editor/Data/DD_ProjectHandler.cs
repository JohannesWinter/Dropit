// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Derive
{
    public class DD_ProjectHandler : MonoBehaviour
    {
        [OnOpenAsset()]
        public static bool OpenDDProject(int instanceID, int line)
        {
            try
            {
                DD_ProjectTemplate template = (DD_ProjectTemplate)EditorUtility.InstanceIDToObject(instanceID) as DD_ProjectTemplate;

                if (template != null)
                {
                    DD_NodeEditorWindow[] dDWindows;
                    dDWindows = (DD_NodeEditorWindow[])Resources.FindObjectsOfTypeAll<DD_NodeEditorWindow>();

                    foreach (DD_NodeEditorWindow window in dDWindows)
                    {
                        if (window.m_currentProject == template)
                        {
                            window.Focus();
                            return false;
                        }
                    }

                    DD_NodeEditorWindow.InitEditorWindow(false);
                    DD_NodeEditorWindow currentWindow = (DD_NodeEditorWindow)EditorWindow.GetWindow<DD_NodeEditorWindow>();

                    if (currentWindow != null)
                    {
                        currentWindow.m_currentProject = template;
                    }
                }
            }
            catch
            {
                return false;
            } 

            return false;
        }
    }

    [CustomEditor(typeof(DD_ProjectTemplate))]
    public class DD_ProjectInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open in Derive"))
            {
                DD_NodeEditorWindow[] dDWindows;
                dDWindows = (DD_NodeEditorWindow[])Resources.FindObjectsOfTypeAll<DD_NodeEditorWindow>();

                foreach(DD_NodeEditorWindow window in dDWindows)
                {
                    if(window.m_currentProject == target)
                    {
                        window.Focus();
                        return;
                    }
                }

                DD_NodeEditorWindow.InitEditorWindow(false);
                DD_NodeEditorWindow currentWindow = (DD_NodeEditorWindow)EditorWindow.GetWindow<DD_NodeEditorWindow>();

                if (currentWindow != null)
                    currentWindow.m_currentProject = (DD_ProjectTemplate)target;
                else
                    EditorUtility.DisplayDialog("Error!", "Unable to open Derive editor", "Ok");

            }
        }
    }
}
#endif
