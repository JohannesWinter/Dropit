using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class GUIDReplaceMatching : EditorWindow
{
    private MonoScript replacementScript;
    private string targetGUID = "";

    [MenuItem("Tools/Replace Matching GUIDs")]
    public static void ShowWindow()
    {
        GetWindow<GUIDReplaceMatching>("Replace Matching GUIDs");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Select the replacement script:");
        replacementScript = (MonoScript)EditorGUILayout.ObjectField("Replacement Script", replacementScript, typeof(MonoScript), false);

        targetGUID = EditorGUILayout.TextField("Target GUID to replace:", targetGUID);

        if (replacementScript != null && !string.IsNullOrEmpty(targetGUID) && GUILayout.Button("Replace GUID"))
        {
            ReplaceMatchingGUIDs(targetGUID);
        }
    }

    private void ReplaceMatchingGUIDs(string guidToReplace)
    {
        string path = AssetDatabase.GetAssetPath(replacementScript);
        string metaPath = path + ".meta";

        if (!System.IO.File.Exists(metaPath))
        {
            Debug.LogError("Meta file not found for " + path);
            return;
        }

        string newGUID = null;
        foreach (var line in System.IO.File.ReadAllLines(metaPath))
        {
            if (line.StartsWith("guid: "))
            {
                newGUID = line.Substring(6).Trim();
                break;
            }
        }

        if (newGUID == null)
        {
            Debug.LogError("GUID not found in meta file: " + metaPath);
            return;
        }

        int count = 0;
        var roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var root in roots)
        {
            count += ReplaceGUIDRecursively(root, guidToReplace, newGUID);
        }

        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log($"GUID Replacement completed. {count} components updated: {guidToReplace} → {newGUID}");
    }

    private int ReplaceGUIDRecursively(GameObject go, string oldGUID, string newGUID)
    {
        int updated = 0;

        SerializedObject so = new SerializedObject(go);
        SerializedProperty components = so.FindProperty("m_Component");

        for (int i = 0; i < components.arraySize; i++)
        {
            var element = components.GetArrayElementAtIndex(i);
            var componentProp = element.FindPropertyRelative("component");

            if (componentProp != null && componentProp.FindPropertyRelative("guid") != null)
            {
                if (componentProp.FindPropertyRelative("guid").stringValue == oldGUID)
                {
                    componentProp.FindPropertyRelative("guid").stringValue = newGUID;
                    updated++;
                }
            }
        }

        so.ApplyModifiedProperties();

        foreach (Transform child in go.transform)
        {
            updated += ReplaceGUIDRecursively(child.gameObject, oldGUID, newGUID);
        }

        return updated;
    }
}
