using UnityEngine;
using UnityEditor;
using System.Text;

public class ShowScriptGUID : EditorWindow
{
    private MonoScript targetScript;
    private GameObject targetGameObject;

    [MenuItem("Tools/Show GUIDs")]
    public static void ShowWindow()
    {
        GetWindow<ShowScriptGUID>("Show GUIDs");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("1) Select a MonoScript to see its GUID:");
        targetScript = (MonoScript)EditorGUILayout.ObjectField("Script", targetScript, typeof(MonoScript), false);

        if (targetScript != null && GUILayout.Button("Show Script GUID and Copy"))
        {
            DisplayAndCopyScriptGUID();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("2) Select a GameObject to list all readable component GUIDs:");
        targetGameObject = (GameObject)EditorGUILayout.ObjectField("GameObject", targetGameObject, typeof(GameObject), true);

        if (targetGameObject != null && GUILayout.Button("List All Component GUIDs"))
        {
            DisplayAllComponentGUIDs();
        }
    }

    private void DisplayAndCopyScriptGUID()
    {
        string path = AssetDatabase.GetAssetPath(targetScript);
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

        if (!string.IsNullOrEmpty(guid))
        {
            Debug.Log($"GUID of script {targetScript.name}: {guid}");
            EditorGUIUtility.systemCopyBuffer = guid;
            Debug.Log("GUID copied to clipboard!");
        }
        else
        {
            Debug.LogError("GUID not found in meta file!");
        }
    }

    private void DisplayAllComponentGUIDs()
    {
        SerializedObject so = new SerializedObject(targetGameObject);
        SerializedProperty components = so.FindProperty("m_Component");

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"GUIDs for GameObject '{targetGameObject.name}':");

        for (int i = 0; i < components.arraySize; i++)
        {
            var element = components.GetArrayElementAtIndex(i);
            var compProp = element.FindPropertyRelative("component");
            if (compProp != null && compProp.FindPropertyRelative("guid") != null)
            {
                string guid = compProp.FindPropertyRelative("guid").stringValue;
                int fileID = compProp.FindPropertyRelative("fileID").intValue;
                sb.AppendLine($"Component {i}: fileID={fileID}, guid={guid}");
            }
        }

        string output = sb.ToString();
        Debug.Log(output);
        EditorGUIUtility.systemCopyBuffer = output;
        Debug.Log("All component GUIDs copied to clipboard!");
    }
}
