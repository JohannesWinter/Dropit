// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace DeriveUtils
{
    public class DD_ShaderGUI_Mobile : ShaderGUI
    {
        MaterialEditor m_materialEditor;
        MaterialProperty[] m_materialProperties;

        COLORSOURCE? m_colorSource = null;

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            //base.OnGUI(materialEditor, properties);

            m_materialEditor = materialEditor;
            m_materialProperties = properties;

            DrawMainSettings();
            DrawSeparator();

            GUILayout.Label("Map Settings", EditorStyles.boldLabel);
            GUILayout.Space(5);

            DrawAlbedoSettings();

            DrawNormalSettings();

            DrawDisplacementSettings();
            GUILayout.Space(10);

            DrawSpecularSettings();
        }

        void DrawMainSettings()
        {
            MaterialProperty tiling = FindProperty("_Tiling", m_materialProperties);

            m_materialEditor.ShaderProperty(tiling, new GUIContent("Tiling", "Uniform tiling for all maps"));
        }

        void DrawAlbedoSettings()
        {
            MaterialProperty albedo = FindProperty("_MainTex", m_materialProperties);
            MaterialProperty mainColor = FindProperty("_MainColor", m_materialProperties);
            MaterialProperty albedoPresent = FindProperty("_AlbedoPresent", m_materialProperties);

            if (m_colorSource == null)
            {
                if (albedoPresent.floatValue == 1) m_colorSource = COLORSOURCE.AlbedoTexture;
                else m_colorSource = COLORSOURCE.UniformColor;
            }

            m_colorSource = (COLORSOURCE)EditorGUILayout.EnumPopup(new GUIContent("Main Color Source"), m_colorSource);

            if (m_colorSource == COLORSOURCE.AlbedoTexture)
            {
                albedoPresent.floatValue = 1;
                m_materialEditor.TexturePropertySingleLine(new GUIContent("Albedo"), albedo);
            }
            else
            {
                albedoPresent.floatValue = 0;
                m_materialEditor.ShaderProperty(mainColor, new GUIContent("Main Color"));
            }
        }

        void DrawNormalSettings()
        {
            MaterialProperty normalMap = FindProperty("_NormalMap", m_materialProperties);
            MaterialProperty normalStrength = FindProperty("_NormalStrength", m_materialProperties);

            m_materialEditor.TexturePropertySingleLine(new GUIContent("Normal Map", "Packed normal map"), normalMap, normalStrength);
        }

        void DrawDisplacementSettings()
        {
            MaterialProperty displacementMap = FindProperty("_DisplacementMap", m_materialProperties);
            MaterialProperty displacement = FindProperty("_Displacement", m_materialProperties);

            m_materialEditor.TexturePropertySingleLine(new GUIContent("Displacement Map", "(AKA) Grayscale height map. Height data is read from the RGB channels"), displacementMap, displacement);
        }

        void DrawSpecularSettings()
        {
            MaterialProperty specularMap = FindProperty("_SpecularMap", m_materialProperties);
            MaterialProperty specularMapPresent = FindProperty("_SpecularMapPresent", m_materialProperties);
            MaterialProperty specularColor = FindProperty("_SpecColor", m_materialProperties);
            MaterialProperty gloss = FindProperty("_Gloss", m_materialProperties);

            m_materialEditor.TexturePropertySingleLine(new GUIContent("Specular Map", "The specular map is read from the alpha channel"), specularMap, specularColor, gloss);
        }

        void DrawSeparator()
        {
            EditorGUILayout.Space(10);

            Rect separatorRect = EditorGUILayout.GetControlRect(false, 1);
            EditorGUI.DrawRect(separatorRect, Color.gray);

            EditorGUILayout.Space(10);
        }
    }
}
#endif