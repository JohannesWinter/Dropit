using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ReplaceRawImages : EditorWindow
{
    [MenuItem("Tools/Replace RawImages Under Parent")]
    public static void ReplaceRawImagesUnderParent()
    {
        GameObject parent = Selection.activeGameObject;

        if (parent == null)
        {
            EditorUtility.DisplayDialog("Fehler", "Bitte ein Parent-Objekt in der Hierarchy auswählen.", "OK");
            return;
        }

        int count = 0;

        RawImage[] rawImages = parent.GetComponentsInChildren<RawImage>(true);

        foreach (var raw in rawImages)
        {
            ConvertRawImage(raw);
            count++;
        }

        EditorUtility.DisplayDialog("Fertig",
            $"Ersetzt: {count} RawImages durch Images.",
            "OK");
    }

    private static void ConvertRawImage(RawImage raw)
    {
        if (raw == null)
            return;

        // --- Schritt 1: Daten auslesen ---
        Color oldColor = raw.color;
        Texture oldTexture = raw.texture;
        Sprite sprite = null;

        if (oldTexture != null)
        {
            string path = AssetDatabase.GetAssetPath(oldTexture);
            if (!string.IsNullOrEmpty(path))
            {
                sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            }
        }

        // --- Schritt 2: RawImage zuerst löschen ---
        GameObject go = raw.gameObject;
        DestroyImmediate(raw, true);

        // Wichtig, damit Unity die Komponentenliste aktualisiert
        EditorUtility.SetDirty(go);

        // --- Schritt 3: Neues Image hinzufügen ---
        Image img = go.AddComponent<Image>();

        // --- Schritt 4: Einstellungen übertragen ---
        img.color = oldColor;
        img.sprite = sprite;
        img.preserveAspect = true;

        EditorUtility.SetDirty(img);
    }

}

