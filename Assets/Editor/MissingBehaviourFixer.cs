using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class GUIDFixerWindow : EditorWindow
{
    private Vector2 scrollPos;
    private int page = 0;
    private const int pageSize = 10;

    private class GUIDFixerEntry
    {
        public GameObject go;
        public MonoScript selectedScript;
    }

    private List<GUIDFixerEntry> entries = new List<GUIDFixerEntry>();

    [MenuItem("Tools/GUID Fixer Paginated")]
    public static void ShowWindow()
    {
        GetWindow<GUIDFixerWindow>("GUID Fixer");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Scan Hierarchy"))
        {
            ScanHierarchy();
            page = 0;
        }

        if (entries.Count == 0)
        {
            EditorGUILayout.LabelField("No GameObjects scanned yet.");
            return;
        }

        // Pagination Buttons
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Prev") && page > 0) page--;
        EditorGUILayout.LabelField($"Page {page + 1} / {Mathf.CeilToInt(entries.Count / (float)pageSize)}", GUILayout.Width(150));
        if (GUILayout.Button("Next") && (page + 1) * pageSize < entries.Count) page++;
        EditorGUILayout.EndHorizontal();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        int start = page * pageSize;
        int end = Mathf.Min(start + pageSize, entries.Count);

        for (int i = start; i < end; i++)
        {
            var entry = entries[i];

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("GameObject: " + GetFullPath(entry.go));

            entry.selectedScript = (MonoScript)EditorGUILayout.ObjectField(
                "Select Script", entry.selectedScript, typeof(MonoScript), false);

            if (entry.selectedScript != null && GUILayout.Button("Apply GUID"))
            {
                SetGUID(entry.go, entry.selectedScript);
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
    }

    private void ScanHierarchy()
    {
        entries.Clear();
        var roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var root in roots)
        {
            CollectGameObjects(root);
        }
    }

    private void CollectGameObjects(GameObject go)
    {
        // Prefabs aus FactoryConnections ignorieren
        GameObject prefabRoot = PrefabUtility.GetCorrespondingObjectFromSource(go);
        if (prefabRoot != null)
        {
            string prefabPath = AssetDatabase.GetAssetPath(prefabRoot);
            if (prefabPath.StartsWith("Assets/FactoryConnections/"))
                return;
        }

        entries.Add(new GUIDFixerEntry { go = go, selectedScript = null });

        foreach (Transform child in go.transform)
            CollectGameObjects(child.gameObject);
    }

    private string GetFullPath(GameObject go)
    {
        return go.transform.parent == null ? go.name : GetFullPath(go.transform.parent.gameObject) + "/" + go.name;
    }

    private void SetGUID(GameObject go, MonoScript newScript)
    {
        string path = AssetDatabase.GetAssetPath(newScript);
        string metaPath = path + ".meta";

        if (!System.IO.File.Exists(metaPath))
        {
            Debug.LogError("Meta file not found for " + path);
            return;
        }

        string guid = null;
        foreach (var line in System.IO.File.ReadAllLines(metaPath))
        {
            if (line.StartsWith("guid: "))
            {
                guid = line.Substring(6).Trim();
                break;
            }
        }

        if (guid == null)
        {
            Debug.LogError("GUID not found in meta file: " + metaPath);
            return;
        }

        SerializedObject so = new SerializedObject(go);
        SerializedProperty components = so.FindProperty("m_Component");

        for (int i = 0; i < components.arraySize; i++)
        {
            var element = components.GetArrayElementAtIndex(i);
            var componentProp = element.FindPropertyRelative("component");

            if (componentProp != null && componentProp.FindPropertyRelative("fileID") != null)
            {
                componentProp.FindPropertyRelative("guid").stringValue = guid;
            }
        }

        so.ApplyModifiedProperties();
        EditorSceneManager.MarkSceneDirty(go.scene);

        Debug.Log($"GUID für {GetFullPath(go)} auf {guid} gesetzt");
    }
}
