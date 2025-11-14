// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace DeriveUtils
{
    /// <summary>
    /// Contains GUILayout methods specifically for the property view of the editor and with customized design
    /// </summary>
    public static class DD_GUILayOut
    {
        /// <summary>
        /// Draws and Controls a float field
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float FloatField(string name, float value)
        {
            EditorGUILayout.Space(5);

            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent(name), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(100));
            value = EditorGUILayout.FloatField(new GUIContent(" "), value, DD_EditorUtils.editorSkin.GetStyle("TextInput"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 150));

            GUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            return value;
        }

        /// <summary>
        /// Draws and Controls an int field
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float IntField(string name, int value)
        {
            EditorGUILayout.Space(5);

            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent(name), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(100));
            value = EditorGUILayout.IntField(new GUIContent(" "), value, DD_EditorUtils.editorSkin.GetStyle("TextInput"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 150));

            GUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            return value;
        }

        /// <summary>
        /// Draws and Controls an int field
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector2 Vector2Field(string name, Vector2 value)
        {
            EditorGUILayout.Space(5);

            GUIStyle styleCache = new GUIStyle(EditorStyles.numberField);
            EditorStyles.numberField.normal = DD_EditorUtils.editorSkin.GetStyle("TextInput").normal;

            GUISkin skinCache = GUI.skin;
            GUI.skin = DD_EditorUtils.editorSkin;

            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent(name), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(100));
            value = EditorGUILayout.Vector2Field(new GUIContent(""), value, GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 153));

            GUILayout.EndHorizontal();

            EditorStyles.numberField.normal = styleCache.normal;
            GUI.skin = skinCache;

            EditorGUILayout.Space(5);

            return value;
        }

        /// <summary>
        /// Draws and controls a slider
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="leftValue"></param>
        /// <param name="rightValue"></param>
        /// <returns></returns>
        public static float Slider(string name, float value, float leftValue, float rightValue)
        {
            EditorGUILayout.Space(10);

            GUIStyle styleCache = new GUIStyle(EditorStyles.numberField);
            EditorStyles.numberField.normal = DD_EditorUtils.editorSkin.GetStyle("TextInput").normal;

            GUISkin skinCache = GUI.skin;
            GUI.skin = DD_EditorUtils.editorSkin;

            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent(name), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(100));
            value = EditorGUILayout.Slider("", value, leftValue, rightValue, GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 150));

            GUILayout.EndHorizontal();

            EditorStyles.numberField.normal = styleCache.normal;
            GUI.skin = skinCache;

            EditorGUILayout.Space(10);

            return value;
        }

        /// <summary>
        /// Draws and controls a min-max range slider
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="leftValue"></param>
        /// <param name="rightValue"></param>
        /// <returns></returns>
        public static Vector2 MinMaxSlider(string name, float minValue, float maxValue, float minLimit, float maxLimit)
        {

            EditorGUILayout.Space(10);

            GUIStyle styleCache = new GUIStyle(EditorStyles.numberField);
            EditorStyles.numberField.normal = DD_EditorUtils.editorSkin.GetStyle("TextInput").normal;

            GUISkin skinCache = GUI.skin;
            GUI.skin = DD_EditorUtils.editorSkin;

            GUILayout.Label(new GUIContent(name), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(100));
            EditorGUILayout.Space(15);

            GUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            minValue = EditorGUILayout.FloatField(new GUIContent(""), minValue, DD_EditorUtils.editorSkin.GetStyle("TextInput"), GUILayout.Width(80));
            EditorGUILayout.Space(10);
            EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, minLimit, maxLimit, GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 250));
            EditorGUILayout.Space(10);
            maxValue = EditorGUILayout.FloatField(new GUIContent(""), maxValue, DD_EditorUtils.editorSkin.GetStyle("TextInput"), GUILayout.Width(80));
            EditorGUILayout.Space();
            GUILayout.EndHorizontal();

            EditorGUILayout.Space(10);

            EditorStyles.numberField.normal = styleCache.normal;
            GUI.skin = skinCache;

            EditorGUILayout.Space(10);

            return new Vector2(minValue, maxValue);
        }

        /// <summary>
        /// Draws and Controls a bool field
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool BoolField(string name, bool value, bool enabled)
        {
            EditorGUILayout.Space(5);

            GUILayout.BeginHorizontal();

            string targetStyle;

            if (enabled) targetStyle = "CheckBox";
            else targetStyle = "CheckBoxDisabled";

            GUIStyle cachedStyle = new GUIStyle(EditorStyles.toggle);
            EditorStyles.toggle.normal = DD_EditorUtils.editorSkin.GetStyle(targetStyle).normal;
            EditorStyles.toggle.hover = DD_EditorUtils.editorSkin.GetStyle(targetStyle).hover;
            EditorStyles.toggle.active = DD_EditorUtils.editorSkin.GetStyle(targetStyle).active;
            EditorStyles.toggle.onNormal = DD_EditorUtils.editorSkin.GetStyle(targetStyle).onNormal;
            EditorStyles.toggle.onHover = DD_EditorUtils.editorSkin.GetStyle(targetStyle).onHover;
            EditorStyles.toggle.onActive = DD_EditorUtils.editorSkin.GetStyle(targetStyle).onActive;

            GUILayout.Label(new GUIContent(name), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(Mathf.Min(DD_EditorUtils.propertyRect.width - 32, 175)));
            value = EditorGUILayout.Toggle(new GUIContent(" "), value, GUILayout.Width(21));

            EditorStyles.toggle.normal = cachedStyle.normal;
            EditorStyles.toggle.hover = cachedStyle.hover;
            EditorStyles.toggle.active = cachedStyle.active;
            EditorStyles.toggle.onNormal = cachedStyle.onNormal;
            EditorStyles.toggle.onHover = cachedStyle.onHover;
            EditorStyles.toggle.onActive = cachedStyle.onActive;

            GUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

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
            EditorGUILayout.Space(5);

            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent(name), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(104));
            value = EditorGUILayout.ColorField(new GUIContent(""), value, true, true, false, GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 158));

            GUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            return value;
        }

        /// <summary>
        /// Draws and controls a float field on a node
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="width"></param>
        /// <param name="floatFieldstyle"></param>
        /// <param name="labelStyle"></param>
        /// <returns></returns>
        public static float FloatFieldOnNode(string name, float value, float width, GUIStyle floatFieldstyle, GUIStyle labelStyle)
        {


            EditorGUILayout.Space(5 * DD_EditorUtils.zoomFactor);

            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent(name), labelStyle, GUILayout.Width(10));
            value = EditorGUILayout.FloatField(new GUIContent(" "), value, floatFieldstyle, GUILayout.Width(width), GUILayout.Height(20 * DD_EditorUtils.zoomFactor));

            GUILayout.EndHorizontal();

            EditorGUILayout.Space(5 * DD_EditorUtils.zoomFactor);

            return value;
        }

        /// <summary>
        /// Draws 2 labels next to each other (they cannot be edited by the user of the editor)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void DoubleLabel(string name, string value)
        {
            EditorGUILayout.Space(5);

            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent(name), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(100));

            string cachedValue = value;

            ///////////////

            Font font = DD_EditorUtils.editorSkin.font;
            float fontSize = DD_EditorUtils.editorSkin.GetStyle("DefaultLabel").fontSize;
            CharacterInfo characterInfo = new CharacterInfo();

            char[] charArray = value.ToCharArray();
            
            int pixelLength = 0;

            foreach(char c in charArray)
            {
                font.GetCharacterInfo(c, out characterInfo);
                pixelLength += characterInfo.advance;
            }

            while((float)pixelLength >= DD_EditorUtils.propertyRect.width - 125)
            {
                if (value.Length == 1) break;

                value = value.Substring(0, value.Length - 1);

                charArray = value.ToCharArray();

                pixelLength = 0;

                foreach (char c in charArray)
                {
                    font.GetCharacterInfo(c, out characterInfo);
                    pixelLength += characterInfo.advance;
                }
            }
            ///////////////

            if (value.Length < cachedValue.Length) value = value + "...";

            GUILayout.Label(new GUIContent(value), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.size.x - 150));

            GUILayout.EndHorizontal();

            EditorGUILayout.Space(5);
        }

        /// <summary>
        /// Draws and controls a simple label field
        /// </summary>
        /// <param name="value"></param>
        public static void Label(string value)
        {
            EditorGUILayout.Space(5);
            GUILayout.Label(new GUIContent(value), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"));
            EditorGUILayout.Space(5);
        }

        /// <summary>
        /// Draws and controls a text field
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TextField(string name, string value)
        {
            EditorGUILayout.Space(5);

            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent(name), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(100));
            value = EditorGUILayout.TextField(" ", value, DD_EditorUtils.editorSkin.GetStyle("TextInput"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.size.x - 150));

            GUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            return value;
        }

        /// <summary>
        /// Draws a title label
        /// The title label is just a label but with different parameters for the appearance
        /// </summary>
        /// <param name="value"></param>
        public static void TitleLabel(string value)
        {
            GUILayout.Space(10);
            GUILayout.Label(value, DD_EditorUtils.editorSkin.GetStyle("TitleLabel"));
            GUILayout.Space(10);
        }

        /// <summary>
        /// Draws a 2D Texture as a field in an EditorGUILayout or a GUILayout
        /// This is used for squared textures
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="areaAboveTexture"></param>
        public static void DrawTexture(Texture2D texture, Rect areaAboveTexture)
        {
            if (texture != null)
            {
                int resolution = (int)Mathf.Min(DD_EditorUtils.propertyRect.width - 30, DD_EditorUtils.currentProject.m_projectSettings.resolution);
                Rect outputPreviewRect = new Rect(areaAboveTexture.position.x + areaAboveTexture.width / 2 - resolution / 2 + 8, areaAboveTexture.position.y + areaAboveTexture.height + 20, resolution - 16, resolution - 16);

                EditorGUI.DrawPreviewTexture(outputPreviewRect, texture);
                GUI.Box(outputPreviewRect, "", DD_EditorUtils.editorSkin.GetStyle("TextureFrame"));

                int numberOfSpaces = (int)(outputPreviewRect.height + 40) / 100;
                float restSpacing = (outputPreviewRect.height + 40) % 100;

                for (int i = 0; i < numberOfSpaces; i++)
                {
                    EditorGUILayout.Space(100);
                }
                EditorGUILayout.Space(restSpacing);
            }
        }

        /// <summary>
        /// Draws a texture that is not squared.
        /// In order to use the space more effectively without stretching the texture,
        /// this method calculates the rect for the texture based on the current width of the property view
        /// and the texture's aspect ratio
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="areaAboveTexture"></param>
        /// <returns></returns>
        public static Rect DrawRectTexture(Texture2D texture, Rect areaAboveTexture)
        {
            if (texture != null)
            {
                float aspectRatio = (float)texture.width / (float)texture.height;

                float resolutionX = Mathf.Min(DD_EditorUtils.propertyRect.width - 30, texture.width);
                float resolutionY = resolutionX / aspectRatio;

                Rect outputPreviewRect = new Rect(areaAboveTexture.position.x + areaAboveTexture.width / 2 - resolutionX / 2 + 8, areaAboveTexture.position.y + areaAboveTexture.height + 20, resolutionX - 16, resolutionY - 16);

                EditorGUI.DrawPreviewTexture(outputPreviewRect, texture);
                GUI.Box(outputPreviewRect, "", DD_EditorUtils.editorSkin.GetStyle("TextureFrame"));

                int numberOfSpaces = (int)(outputPreviewRect.height + 40) / 100;
                float restSpacing = (outputPreviewRect.height + 40) % 100;

                for (int i = 0; i < numberOfSpaces; i++)
                {
                    EditorGUILayout.Space(100);
                }
                EditorGUILayout.Space(restSpacing);

                return outputPreviewRect;
            }
            return Rect.zero;
        }

        /// <summary>
        /// Draws a foldout group and returns a bool
        /// This can be used in GUILayouts
        /// </summary>
        /// <param name="value"></param>
        /// <param name="foldoutName"></param>
        /// <returns></returns>
        public static bool FoldOut(bool value, string foldoutName)
        {
            if (DD_EditorUtils.propertyFrameRect.width != 0)
                DD_EditorUtils.editorSkin.GetStyle("FoldoutBackground").overflow.left = -(int)DD_EditorUtils.propertyFrameRect.width + 8;

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("", DD_EditorUtils.editorSkin.GetStyle("FoldoutBackground"), GUILayout.Width(0), GUILayout.Height(32));

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
        /// Draws an object field for textures into a layout
        /// This is used to draw the field on a node
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Texture2D Texture2DFieldOnNode(Texture2D value)
        {
            GUIStyle styleCache = new GUIStyle(EditorStyles.objectField);

            EditorStyles.objectField.normal = DD_EditorUtils.editorSkin.GetStyle("TextInput").normal;
            EditorStyles.objectField.border = DD_EditorUtils.editorSkin.GetStyle("TextInput").border;

            value = (Texture2D)EditorGUILayout.ObjectField(value, typeof(Texture2D), false);

            EditorStyles.objectField.normal = styleCache.normal;
            EditorStyles.objectField.border = styleCache.border;

            return value;
        }

        /// <summary>
        /// Draws an object field for textures into a layout
        /// This is used to draw the field into the property view
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Texture2D Texture2DField(string name, Texture2D value)
        {
            GUIStyle styleCache = new GUIStyle(EditorStyles.objectField);

            EditorStyles.objectField.normal = DD_EditorUtils.editorSkin.GetStyle("TextInput").normal;
            EditorStyles.objectField.border = DD_EditorUtils.editorSkin.GetStyle("TextInput").border;

            EditorGUILayout.Space(5);

            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent(name), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(104));
            value = (Texture2D)EditorGUILayout.ObjectField(value, typeof(Texture2D), false, GUILayout.Width(DD_EditorUtils.viewRect_propertyView.size.x - 158));

            GUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            EditorStyles.objectField.normal = styleCache.normal;
            EditorStyles.objectField.border = styleCache.border;

            return value;
        }
    }
}
#endif