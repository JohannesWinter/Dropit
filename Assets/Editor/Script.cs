using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class SceneGUIDScanner : EditorWindow
{
    private string scenePath;
    private Vector2 scrollPos;
    private string output = "";

    [MenuItem("Tools/Scan Scene GUIDs")]
    public static void ShowWindow()
    {
        GetWindow<SceneGUIDScanner>("Scene GUID Scanner");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Select Scene to scan for GameObject fileIDs and Component GUIDs:");

        if (GUILayout.Button("Use Active Scene"))
        {
            scenePath = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path;
            Debug.Log("Active Scene path: " + scenePath);
        }

        EditorGUILayout.LabelField("Scene Path:", scenePath ?? "No scene selected");

        if (!string.IsNullOrEmpty(scenePath) && GUILayout.Button("Scan Scene"))
        {
            ScanScene();
        }

        EditorGUILayout.LabelField("Output:");
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUILayout.TextArea(output, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Copy Output to Clipboard"))
        {
            EditorGUIUtility.systemCopyBuffer = output;
            Debug.Log("Output copied to clipboard!");
        }
    }

    private void ScanScene()
    {
        if (!File.Exists(scenePath))
        {
            Debug.LogError("Scene file not found: " + scenePath);
            return;
        }

        output = "";
        string[] lines = File.ReadAllLines(scenePath);
        string currentGOName = "";
        string currentGOFileID = "";
        bool inGameObject = false;

        Regex goHeaderRegex = new Regex(@"^--- !u!1 &(\d+)");
        Regex compLineRegex = new Regex(@"component: {fileID: (\d+)}");
        Regex nameLineRegex = new Regex(@"m_Name: (.+)");
        Regex guidLineRegex = new Regex(@"guid: ([a-f0-9]+)");

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            // GameObject Header
            var goMatch = goHeaderRegex.Match(line);
            if (goMatch.Success)
            {
                inGameObject = true;
                currentGOFileID = goMatch.Groups[1].Value;
                currentGOName = "";
                continue;
            }

            if (inGameObject)
            {
                // Name der GameObject
                var nameMatch = nameLineRegex.Match(line);
                if (nameMatch.Success)
                {
                    currentGOName = nameMatch.Groups[1].Value;
                    output += $"GameObject '{currentGOName}' (fileID={currentGOFileID}):\n";
                    continue;
                }

                // Komponenten
                var compMatch = compLineRegex.Match(line);
                if (compMatch.Success)
                {
                    string compFileID = compMatch.Groups[1].Value;
                    string compGUID = "none";

                    // Prüfe die nächsten paar Zeilen auf GUID
                    for (int j = i + 1; j < lines.Length && j < i + 20; j++)
                    {
                        var guidMatch = guidLineRegex.Match(lines[j]);
                        if (guidMatch.Success)
                        {
                            compGUID = guidMatch.Groups[1].Value;
                            break;
                        }
                    }

                    output += $"  Component fileID={compFileID}, guid={compGUID}\n";
                }

                // Ende GameObject erkennen
                if (line.StartsWith("--- !u!")) inGameObject = false;
            }
        }

        Debug.Log("Scene scan completed!");
    }
}
