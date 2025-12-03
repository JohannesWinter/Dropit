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
        Texture tex = raw.texture;
        Color color = raw.color;

        // Neues Image hinzufügen
        Image img = raw.gameObject.AddComponent<Image>();

        // Sprite zuweisen (falls möglich)
        Sprite sprite = null;

        if (tex != null)
        {
            // Asset-Pfad der Texture holen
            string path = AssetDatabase.GetAssetPath(tex);

            if (!string.IsNullOrEmpty(path))
            {
                // Versuchen das Sprite aus dem Asset zu laden
                sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            }
        }

        if (sprite == null && tex != null)
        {
            Debug.LogWarning($"Warning - Konnte kein Sprite für Texture '{tex.name}' finden. " +
                             $"Stelle sicher, dass der Import Mode auf 'Sprite (2D and UI)' steht.", raw);
        }

        if (sprite != null)
        {
            img.sprite = sprite;
        }   
        img.color = color;
        img.preserveAspect = true;

        // RawImage löschen
        DestroyImmediate(raw, true);

        EditorUtility.SetDirty(img);
    }
}

