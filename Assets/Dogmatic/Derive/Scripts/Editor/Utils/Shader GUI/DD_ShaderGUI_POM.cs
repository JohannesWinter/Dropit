// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace DeriveUtils {

    public enum COLORSOURCE
    {
        AlbedoTexture,
        UniformColor
    }

    public class DD_ShaderGUI_POM : ShaderGUI
    {
        MaterialEditor m_materialEditor;
        MaterialProperty[] m_materialProperties;

        COLORSOURCE? m_colorSource = null;
        SPECULARCOLORSOURCE? m_specColorSpource = null;

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

            DrawAOSettings();
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

        void DrawAOSettings()
        {
            MaterialProperty aOMap = FindProperty("_AmbientOcclusionMap", m_materialProperties);
            MaterialProperty aO = FindProperty("_AmbientOcclusion", m_materialProperties);

            m_materialEditor.TexturePropertySingleLine(new GUIContent("Ambient Occlusion Map", "The AO map is read from the RGB channels"), aOMap, aO);
        }

        void DrawSpecularSettings()
        {
            MaterialProperty specularMap = FindProperty("_SpecularMap", m_materialProperties);
            MaterialProperty specularMapPresent = FindProperty("_SpecularMapPresent", m_materialProperties);
            MaterialProperty specularfromRGB = FindProperty("_SpecularfromRGB", m_materialProperties);
            MaterialProperty specularColor = FindProperty("_SpecularColor", m_materialProperties);
            MaterialProperty gloss = FindProperty("_Gloss", m_materialProperties);

            if (m_specColorSpource == null)
            {
                if (specularfromRGB.floatValue == 1) m_specColorSpource = SPECULARCOLORSOURCE.SpecularMapRGB;
                else m_specColorSpource = SPECULARCOLORSOURCE.uniformColor;
            }

            m_specColorSpource = (SPECULARCOLORSOURCE)EditorGUILayout.EnumPopup("Specular Color Source", m_specColorSpource);

            if (specularMap.textureValue == null) specularMapPresent.floatValue = 0;
            else specularMapPresent.floatValue = 1;

            if (m_specColorSpource == SPECULARCOLORSOURCE.SpecularMapRGB)
            {
                specularfromRGB.floatValue = 1;
                m_materialEditor.TexturePropertySingleLine(new GUIContent("Specular Map", "The specular map is read from the alpha channel"), specularMap, gloss);
            }
            else
            {
                specularfromRGB.floatValue = 0;
                m_materialEditor.TexturePropertySingleLine(new GUIContent("Specular Map", "The specular map is read from the alpha channel"), specularMap, specularColor, gloss);
            }
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