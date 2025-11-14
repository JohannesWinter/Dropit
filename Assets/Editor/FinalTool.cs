using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class ReplaceMissingGUID : EditorWindow
{
    private GameObject exampleObject;
    private MonoScript replacementScript;

    [MenuItem("Tools/Replace Missing Script GUID")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceMissingGUID>("Replace Missing Script GUID");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Step 1: Select a GameObject that has the Missing Script");
        exampleObject = (GameObject)EditorGUILayout.ObjectField("Example Object", exampleObject, typeof(GameObject), true);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Step 2: Select the replacement Script (MonoBehaviour)");
        replacementScript = (MonoScript)EditorGUILayout.ObjectField("Replacement Script", replacementScript, typeof(MonoScript), false);

        EditorGUILayout.Space();

        if (exampleObject != null && replacementScript != null && GUILayout.Button("Replace All Matching GUIDs"))
        {
            ReplaceAllGUIDs();
        }
    }

    private void ReplaceAllGUIDs()
    {
        // Alte GUID vom Missing Script auslesen
        SerializedObject soExample = new SerializedObject(exampleObject);
        SerializedProperty components = soExample.FindProperty("m_Component");

        string oldGUID = null;

        for (int i = 0; i < components.arraySize; i++)
        {
            var element = components.GetArrayElementAtIndex(i);
            var compProp = element.FindPropertyRelative("component");
            if (compProp != null && !string.IsNullOrEmpty(compProp.FindPropertyRelative("guid")?.stringValue))
            {
                oldGUID = compProp.FindPropertyRelative("guid").stringValue;
                break;
            }
        }

        if (string.IsNullOrEmpty(oldGUID))
        {
            Debug.LogError("Could not find a GUID on the example GameObject. Make sure it has a Missing Script component.");
            return;
        }

        // Neue GUID vom ausgewählten Script auslesen
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

        if (string.IsNullOrEmpty(newGUID))
        {
            Debug.LogError("Could not read GUID from replacement script meta file.");
            return;
        }

        int count = 0;
        var roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var root in roots)
        {
            count += ReplaceGUIDRecursively(root, oldGUID, newGUID);
        }

        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log($"Replaced {count} components with new GUID {newGUID}.");
    }

    private int ReplaceGUIDRecursively(GameObject go, string oldGUID, string newGUID)
    {
        int updated = 0;

        SerializedObject so = new SerializedObject(go);
        SerializedProperty components = so.FindProperty("m_Component");

        for (int i = 0; i < components.arraySize; i++)
        {
            var element = components.GetArrayElementAtIndex(i);
            var compProp = element.FindPropertyRelative("component");

            if (compProp != null && compProp.FindPropertyRelative("guid") != null)
            {
                if (compProp.FindPropertyRelative("guid").stringValue == oldGUID)
                {
                    compProp.FindPropertyRelative("guid").stringValue = newGUID;
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
