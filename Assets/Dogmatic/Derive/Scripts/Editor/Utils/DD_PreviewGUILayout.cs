// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace DeriveUtils
{
    /// <summary>
    /// Contains GUILayout specifically set up to work with the preview
    /// Similar to DD_GUILayout, but value names and values are displayed under another instead of next to another
    /// </summary>
    public static class DD_PreviewGUILayout
    {
        /// <summary>
        /// Draws and controls a float field
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float FloatField(string name, float value)
        {
            float labelWidthCache = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 5;

            EditorGUILayout.Space(5);

            GUILayout.Label(new GUIContent(name), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"));
            value = EditorGUILayout.FloatField(new GUIContent(" "), value, DD_EditorUtils.editorSkin.GetStyle("TextInput"));

            EditorGUILayout.Space(5);

            EditorGUIUtility.labelWidth = labelWidthCache;

            return value;
        }

        /// <summary>
        /// Draws and controls a color field
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Color ColorField(string name, Color value)
        {
            float labelWidthCache = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 5;

            EditorGUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent(name), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"));
            value = EditorGUILayout.ColorField(new GUIContent(" "), value);
            GUILayout.EndHorizontal();

            EditorGUILayout.Space(15);

            EditorGUIUtility.labelWidth = labelWidthCache;

            return value;
        }

        /// <summary>
        /// Draws a title label
        /// A title label is different in appearance from a normal label
        /// </summary>
        /// <param name="value"></param>
        /// <param name="width"></param>
        public static void TitleLabel(string value, float width)
        {
            GUILayout.Space(10);
            GUILayout.Label(value, DD_EditorUtils.editorSkin.GetStyle("TitleLabel"), GUILayout.Width(width - 25));
            GUILayout.Space(10);
        }

        /// <summary>
        /// Draws a label
        /// </summary>
        /// <param name="value"></param>
        /// <param name="width"></param>
        public static void Label(string value, float width)
        {
            EditorGUILayout.Space(5);
            GUILayout.Label(new GUIContent(value), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(width - 25));
            EditorGUILayout.Space(5);
        }

        /// <summary>
        /// Draws and controls a slider
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static float Slider(string name, string tooltip, float value, float minValue, float maxValue, float width)
        {
            GUIStyle styleCache = new GUIStyle(EditorStyles.numberField);
            EditorStyles.numberField.normal = DD_EditorUtils.editorSkin.GetStyle("TextInput").normal;

            GUISkin skinCache = GUI.skin;
            GUI.skin = DD_EditorUtils.editorSkin;

            EditorGUILayout.Space(5);

            EditorGUILayout.LabelField(new GUIContent(name, null, tooltip), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(width - 25));
            value = EditorGUILayout.Slider(value, minValue, maxValue, GUILayout.Width(width - 25));

            EditorGUILayout.Space(10);

            EditorStyles.numberField.normal = styleCache.normal;
            GUI.skin = skinCache;

            return value;
        }

        /// <summary>
        /// Draws and controls an object field for a mesh
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static Mesh MeshField(string name, Mesh value, float width)
        {
            GUIStyle styleCache = new GUIStyle(EditorStyles.objectField);

            EditorStyles.objectField.normal = DD_EditorUtils.editorSkin.GetStyle("TextInput").normal;
            EditorStyles.objectField.border = DD_EditorUtils.editorSkin.GetStyle("TextInput").border;

            EditorGUILayout.Space(5);

            EditorGUILayout.LabelField(name, DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(width - 25));
            value = (Mesh)EditorGUILayout.ObjectField(value, typeof(Mesh), false, GUILayout.Width(width - 25));

            EditorGUILayout.Space(10);

            EditorStyles.objectField.normal = styleCache.normal;
            EditorStyles.objectField.border = styleCache.border;

            return value;
        }
    }
}
#endif