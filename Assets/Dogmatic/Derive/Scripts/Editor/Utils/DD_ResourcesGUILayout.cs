// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace DeriveUtils
{
    public static class DD_ResourcesGUILayout
    {
        /// <summary>
        /// Draws a foldout group and returns a bool
        /// This can be used in GUILayouts
        /// </summary>
        /// <param name="value"></param>
        /// <param name="foldoutName"></param>
        /// <returns></returns>
        public static bool FoldOut(bool value, string foldoutName, bool highlight)
        {
            if (DD_EditorUtils.resourcesFrameRect.width != 0)
            {
                DD_EditorUtils.editorSkin.GetStyle("FoldoutBackgroundResources").overflow.left = -(int)DD_EditorUtils.resourcesRect.width + 6;
                DD_EditorUtils.editorSkin.GetStyle("FoldoutBackgroundResourcesHighlighted").overflow.left = -(int)DD_EditorUtils.resourcesRect.width + 6;
            }
                

            EditorGUILayout.BeginHorizontal();

            if(highlight) GUILayout.Label("", DD_EditorUtils.editorSkin.GetStyle("FoldoutBackgroundResourcesHighlighted"), GUILayout.Width(0), GUILayout.Height(32));
            else GUILayout.Label("", DD_EditorUtils.editorSkin.GetStyle("FoldoutBackgroundResources"), GUILayout.Width(0), GUILayout.Height(32));

            if (value)
            {
                if (GUILayout.Button("", DD_EditorUtils.editorSkin.GetStyle("ArrowDown2"), GUILayout.Width(24), GUILayout.Height(32)))
                    value = false;
            }
            else
            {
                if (GUILayout.Button("", DD_EditorUtils.editorSkin.GetStyle("ArrowRight2"), GUILayout.Width(24), GUILayout.Height(32)))
                    value = true;
            }

            GUILayout.Label(new GUIContent(foldoutName), DD_EditorUtils.editorSkin.GetStyle("FoldoutLabel"));

            EditorGUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Draws a foldout group and returns a bool
        /// This can be used in GUILayouts
        /// Level 2 means it's suited for subtabs
        /// </summary>
        /// <param name="value"></param>
        /// <param name="foldoutName"></param>
        /// <returns></returns>
        public static bool FoldOutLevel2(bool value, string foldoutName, bool highlight)
        {
            if (DD_EditorUtils.resourcesFrameRect.width != 0)
            {
                DD_EditorUtils.editorSkin.GetStyle("FoldoutBackgroundResourcesHighlighted").overflow.left = -(int)DD_EditorUtils.resourcesRect.width + 6;
                DD_EditorUtils.editorSkin.GetStyle("FoldoutBackgroundResourcesLevel2").overflow.left = -(int)DD_EditorUtils.resourcesRect.width + 6;
            }
                
            EditorGUILayout.BeginHorizontal();

            if (highlight) GUILayout.Label("", DD_EditorUtils.editorSkin.GetStyle("FoldoutBackgroundResourcesHighlighted"), GUILayout.Width(0), GUILayout.Height(32));
            else GUILayout.Label("", DD_EditorUtils.editorSkin.GetStyle("FoldoutBackgroundResourcesLevel2"), GUILayout.Width(0), GUILayout.Height(32));

            if (value)
            {
                if (GUILayout.Button("", DD_EditorUtils.editorSkin.GetStyle("ArrowDown2Level2"), GUILayout.Width(24), GUILayout.Height(32)))
                    value = false;
            }
            else
            {
                if (GUILayout.Button("", DD_EditorUtils.editorSkin.GetStyle("ArrowRight2Level2"), GUILayout.Width(24), GUILayout.Height(32)))
                    value = true;
            }

            GUILayout.Label(new GUIContent(foldoutName), DD_EditorUtils.editorSkin.GetStyle("FoldoutLabel"));

            EditorGUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Draws a foldout group and returns a bool
        /// This can be used in GUILayouts
        /// Level 3 means it's suited for subtabs of Level 2
        /// </summary>
        /// <param name="value"></param>
        /// <param name="foldoutName"></param>
        /// <returns></returns>
        public static bool FoldOutLevel3(bool value, string foldoutName)
        {
            if (DD_EditorUtils.resourcesFrameRect.width != 0)
                DD_EditorUtils.editorSkin.GetStyle("FoldoutBackgroundResourcesLevel3").overflow.left = -(int)DD_EditorUtils.resourcesRect.width + 6;
                

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("", DD_EditorUtils.editorSkin.GetStyle("FoldoutBackgroundResourcesLevel3"), GUILayout.Width(0), GUILayout.Height(32));

            if (value)
            {
                if (GUILayout.Button("", DD_EditorUtils.editorSkin.GetStyle("ArrowDown2Level3"), GUILayout.Width(24), GUILayout.Height(32)))
                    value = false;
            }
            else
            {
                if (GUILayout.Button("", DD_EditorUtils.editorSkin.GetStyle("ArrowRight2Level3"), GUILayout.Width(24), GUILayout.Height(32)))
                    value = true;
            }

            GUILayout.Label(new GUIContent(foldoutName), DD_EditorUtils.editorSkin.GetStyle("FoldoutLabel"));

            EditorGUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Draws and controls a simple label field
        /// </summary>
        /// <param name="value"></param>
        public static void Label(string value)
        {
            EditorGUILayout.Space(5);
            GUILayout.Label(new GUIContent(value), DD_EditorUtils.editorSkin.GetStyle("BigLabel"));
            EditorGUILayout.Space(5);
        }

        /// <summary>
        /// Draws and controls a simple label field with a fixed width
        /// </summary>
        /// <param name="value"></param>
        public static void LabelDownloadResources(string value, float width, float height)
        {
            EditorGUILayout.Space(5);
            GUILayout.Label(new GUIContent(value), DD_EditorUtils.editorSkin.GetStyle("DownloadResourcesLabel"), GUILayout.Width(width), GUILayout.Width(height));
            EditorGUILayout.Space(5);
        }

        /// <summary>
        /// Draws and controls a text box with line break
        /// </summary>
        /// <param name="value"></param>
        public static void TextBox(string value)
        {
            EditorGUILayout.Space(5);
            GUILayout.Label(new GUIContent(value), DD_EditorUtils.editorSkin.GetStyle("TextBox"));
            EditorGUILayout.Space(5);
        }

        public static void HorizontalSeparator(float spacing)
        {
            EditorGUILayout.Space(spacing);

            Rect rect = EditorGUILayout.GetControlRect(false, 1);

            rect.height = 1;

            EditorGUI.DrawRect(rect, new Color(0.8f, 0.8f, 0.8f, 1));

            EditorGUILayout.Space(spacing);
        }
    }
}
#endif